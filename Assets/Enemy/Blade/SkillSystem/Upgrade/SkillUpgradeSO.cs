using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Blade.SkillSystem.Upgrade
{
    public enum UpgradeType
    {
        FieldUpdate = 0, MethodCall = 1
    }

    public enum FieldType
    {
        Float = 0, Int32 = 1, Boolean = 2
    }
    
    [CreateAssetMenu(fileName = "skill upgrade", menuName = "SO/Combat/Skill upgrade", order = 0)]
    public class SkillUpgradeSO : ScriptableObject
    {
        public Sprite upgradeIcon;
        public string upgradeTitle;
        [TextArea]
        public string upgradeDescription;

        public int maxUpgradeCount = 1;

        public List<SkillUpgradeSO> needUpgradeList = new();
        public List<SkillUpgradeSO> dontNeedUpgradeList = new();
        
        public BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [HideInInspector] public string targetSkillName;
        [HideInInspector] public UpgradeType upgradeType;

        #region Field update info section

        [HideInInspector] public string fieldName;
        [HideInInspector] public FieldType fieldType;
        [HideInInspector] public float floatValue;
        [HideInInspector] public int intValue;

        #endregion

        #region Method call info section

        [HideInInspector] public string upgradeMethodName;
        [HideInInspector] public string upgradeParameters;
        [HideInInspector] public string rollbackMethodName;
        [HideInInspector] public string rollbackParameters;
        
        #endregion

        private Skill _skillInstance;
        private Action<Skill> _fieldUpgradeAction;
        private Action<Skill> _fieldRollbackAction;
        private Action<Skill> _upgradeMethodCallAction;
        private Action<Skill> _rollbackMethodCallAction;

        private void OnEnable()
        {
            InitializeUpgrade();
        }

        public string InitializeUpgrade()
        {
            if (upgradeType == UpgradeType.FieldUpdate)
                return FieldUpdateFactory();
            if (upgradeType == UpgradeType.MethodCall)
                return MethodCallFactory();

            return "Unknown upgrade type";
        }

        private string FieldUpdateFactory()
        {
            Type skillParentType = typeof(Skill);  //Skill 타입
            Type skillType = Type.GetType(targetSkillName);  // ex) BlueBulletSkill 

            if (string.IsNullOrEmpty(fieldName))
            {
                return $"Target field name is empty check {name}";
            }
            
            FieldInfo targetField = skillType.GetField(fieldName, bindingFlags);  //DM
            if (targetField == null)
            {
                return $"Target field is not exist check {name} : {fieldName}";
            }

            ParameterExpression parameter = Expression.Parameter(skillParentType, "skillType");
            UnaryExpression castedParameter = Expression.Convert(parameter, skillType);
            MemberExpression fieldAccess = Expression.Field(castedParameter, targetField);
            
            Expression upgradeExpression = null;
            Expression rollbackExpression = null;

            switch (fieldType)
            {
                case FieldType.Float:
                    upgradeExpression = Expression.Add(fieldAccess, Expression.Constant(floatValue, typeof(float)));
                    rollbackExpression = Expression.Add(fieldAccess, Expression.Constant(-floatValue, typeof(float)));
                    break;
                case FieldType.Int32:
                    upgradeExpression = Expression.Add(fieldAccess, Expression.Constant(intValue, typeof(int)));
                    rollbackExpression = Expression.Add(fieldAccess, Expression.Constant(-intValue, typeof(int)));
                    break;
                case FieldType.Boolean:
                    upgradeExpression = Expression.Constant(true, typeof(bool));
                    rollbackExpression = Expression.Constant(false, typeof(bool));
                    break;
            }

            BinaryExpression upgradeAssign = Expression.Assign(fieldAccess, upgradeExpression);
            BinaryExpression rollbackAssign = Expression.Assign(fieldAccess, rollbackExpression);
            
            _fieldUpgradeAction = Expression.Lambda<Action<Skill>>(upgradeAssign, parameter).Compile();
            _fieldRollbackAction = Expression.Lambda<Action<Skill>>(rollbackAssign, parameter).Compile();
            
            return "Success";
        }

        private string MethodCallFactory()
        {
            Type skillParentType = typeof(Skill);  //Skill 타입
            Type skillType = Type.GetType(targetSkillName);  // ex) BlueBulletSkill 
            
            if(string.IsNullOrEmpty(upgradeMethodName))
                return $"Target method name is empty check {name}";

            if(skillType == null)
                return $"Target skill type is not exist check {name}";
            
            MethodInfo upgradeMethod = skillType.GetMethod(upgradeMethodName, bindingFlags);
            MethodInfo rollbackMethod = skillType.GetMethod(rollbackMethodName, bindingFlags);

            if (upgradeMethod == null || rollbackMethod == null)
                return $"Method is null! check method name : {upgradeMethodName}, {rollbackMethodName}";

            ParameterExpression skillParam = Expression.Parameter(skillParentType, "skill");
            UnaryExpression castedSkillParam = Expression.Convert(skillParam, skillType);

            try
            {
                Expression[] upgradeParams = GetMethodParameters(upgradeMethod, upgradeParameters);
                Expression[] rollbackParams = GetMethodParameters(rollbackMethod, rollbackParameters);
                
                var upgradeCall = Expression.Call(castedSkillParam, upgradeMethod, upgradeParams);
                var rollbackCall = Expression.Call(castedSkillParam, rollbackMethod, rollbackParams);
                _upgradeMethodCallAction = Expression.Lambda<Action<Skill>>(upgradeCall, skillParam).Compile();
                _rollbackMethodCallAction = Expression.Lambda<Action<Skill>>(rollbackCall, skillParam).Compile();
                
                return "Success";
            }
            catch (Exception e)
            {
                return $"Error in Method Call factory check parameter : {e.Message}";
            }
        }

        private Expression[] GetMethodParameters(MethodInfo method, string inputParams)
        {
            //32, 4f, true, hello
            string[] paramValues = inputParams.Split(",").Select(param => param.Trim()).ToArray();
            ParameterInfo[] methodParams = method.GetParameters();
            
            Debug.Assert(methodParams.Length == 0 || methodParams.Length == paramValues.Length,
                "Parameter count is not match!");
            
            Expression[] args = new Expression[methodParams.Length];
            for (int i = 0; i < methodParams.Length; i++)
            {
                Type paramType = methodParams[i].ParameterType;
                object convertedValue = Convert.ChangeType(paramValues[i], paramType);
                args[i] = Expression.Constant(convertedValue, paramType);
            }

            return args;
        }


        #region Runtime upgrade section

        public void UpgradeSkill(Skill skill)
        {
            if(upgradeType == UpgradeType.FieldUpdate)
                _fieldUpgradeAction?.Invoke(skill);
            else if(upgradeType == UpgradeType.MethodCall)
                _upgradeMethodCallAction?.Invoke(skill);
        }

        public void RollbackSkill(Skill skill)
        {
            if(upgradeType == UpgradeType.FieldUpdate)
                _fieldRollbackAction?.Invoke(skill);
            else if(upgradeType == UpgradeType.MethodCall)
                _rollbackMethodCallAction?.Invoke(skill);
        }

        #endregion
    }
}