using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Blade.SkillSystem.Upgrade.Editor
{
    [CustomEditor(typeof(SkillUpgradeSO))]
    public class SkillUpgradeEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset view = default;
        
        private SkillUpgradeSO _targetSO;
        private VisualElement _root;
        private VisualElement _fieldInfoBox;
        private VisualElement _methodInfoBox;
        
        public override VisualElement CreateInspectorGUI()
        {
            _targetSO = target as SkillUpgradeSO;
            _root = new VisualElement();
            //기본 에디터 채워넣기(HIde된 녀석들은 제외)
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);
            view.CloneTree(_root); //커스텀 클론해서 넣어주고
            
            //토글 시키기 위해서 각 박스들을 가져온다.
            _fieldInfoBox = _root.Q<VisualElement>("FieldInfoBox");
            _methodInfoBox = _root.Q<VisualElement>("MethodInfoBox");

            MakeSkillDropdownView();
            RegisterChangeEvent();

            _root.Q<Button>("ValidateButton").clicked += () =>
            {
                EditorUtility.DisplayDialog("Message", _targetSO.InitializeUpgrade(), "OK");
            };
            return _root;
        }

        private void MakeSkillDropdownView()
        {
            DropdownField skillDropdown = _root.Q<DropdownField>("TargetSkillDropdown");
            skillDropdown.choices.Clear();
            
            Type skillParentType = typeof(Skill);
            List<string> derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(skillParentType) && type != skillParentType)
                .Select(type => $"{type.AssemblyQualifiedName}")
                .ToList();
            // 가산점 0.5점.
            
            skillDropdown.choices.AddRange(derivedTypes);

            if (skillDropdown.choices.Contains(_targetSO.targetSkillName) == false)
            {
                _targetSO.targetSkillName = skillDropdown.choices.Count > 0 
                    ? skillDropdown.choices.First() : string.Empty;
                EditorUtility.SetDirty(_targetSO);
            }
    
            UpdateReflectionInfo();
        }

        private void RegisterChangeEvent()
        {
            EnumField upgradeTypeField = _root.Q<EnumField>("UpgradeTypeEnum");
            upgradeTypeField.RegisterValueChangedCallback(evt => UpdateReflectionInfo());
            
            EnumField fieldTypeField = _root.Q<EnumField>("FieldTypeEnum");
            fieldTypeField.RegisterValueChangedCallback(evt => UpdateReflectionInfo());
        }

        private void UpdateReflectionInfo()
        {
            if (string.IsNullOrEmpty(_targetSO.targetSkillName))
            {
                EditorUtility.DisplayDialog("Error", "스킬 타입을 먼저 골라야 합니다.", "OK");
                return;
            }

            switch (_targetSO.upgradeType)
            {
                case UpgradeType.FieldUpdate:
                    _fieldInfoBox.style.display = DisplayStyle.Flex;
                    _methodInfoBox.style.display = DisplayStyle.None;
                    UpdateFieldInfo();
                    break;
                case UpgradeType.MethodCall:
                    _fieldInfoBox.style.display = DisplayStyle.None;
                    _methodInfoBox.style.display = DisplayStyle.Flex;
                    UpdateMethodInfo();
                    break;
            }
            //모든 에셋을 저장검사하지 않고 지정된 녀석이 더티하다면 저장하는식으로 해서 비용을 아낀다.
            AssetDatabase.SaveAssetIfDirty(_targetSO);
        }

        private void UpdateFieldInfo()
        {
            DropdownField fieldDropdown = _root.Q<DropdownField>("FieldListDropdown");
            Type skillType = Type.GetType(_targetSO.targetSkillName);
            
            FieldInfo[] fieldInfos = skillType.GetFields(_targetSO.bindingFlags);

            fieldDropdown.choices = _targetSO.fieldType switch
            {
                FieldType.Float => fieldInfos.Where(field => field.FieldType == typeof(float))
                    .Select(field => field.Name).ToList(),
                FieldType.Int32 => fieldInfos.Where(field => field.FieldType == typeof(int))
                    .Select(field => field.Name).ToList(),
                FieldType.Boolean => fieldInfos.Where(field => field.FieldType == typeof(bool))
                    .Select(field => field.Name).ToList(),
                _ => fieldDropdown.choices
            };

            if (fieldDropdown.choices.Contains(_targetSO.fieldName) == false)
            {
                _targetSO.fieldName = fieldDropdown.choices.Count > 0 
                    ? fieldDropdown.choices.First() : string.Empty;
                EditorUtility.SetDirty(_targetSO);
            }
            
            FloatField floatField = _root.Q<FloatField>("FloatValue");
            IntegerField integerField = _root.Q<IntegerField>("IntegerValue");

            switch (_targetSO.fieldType)
            {
                case FieldType.Float:
                    floatField.style.display = DisplayStyle.Flex;
                    integerField.style.display = DisplayStyle.None;
                    break;
                case FieldType.Int32:
                    floatField.style.display = DisplayStyle.None;
                    integerField.style.display = DisplayStyle.Flex;
                    break;
                case FieldType.Boolean:
                    floatField.style.display = DisplayStyle.None;
                    integerField.style.display = DisplayStyle.None;
                    break;
            }
        }
        
        private void UpdateMethodInfo()
        {
            DropdownField upgradeDropdown = _root.Q<DropdownField>("UpgradeMethodNameDropdown");
            DropdownField rollbackDropdown = _root.Q<DropdownField>("RollbackMethodNameDropdown");

            //이 매서드가 실행되는 시점에는 SO에 AssemblyFullname이 들어간다.
            Type skillType = Type.GetType(_targetSO.targetSkillName);
            
            MethodInfo[] methodInfos = skillType.GetMethods(_targetSO.bindingFlags);
            
            //그중에서 리턴타입이 void인 녀석들이 업그레이드 관련 매서드일 수 있으니
            upgradeDropdown.choices
                 = methodInfos.Where(methodInfo => methodInfo.ReturnType == typeof(void))
                     .Select(methodInfo => methodInfo.Name).ToList();
            rollbackDropdown.choices = upgradeDropdown.choices; //같은거니까 넣어준다.

            if (upgradeDropdown.choices.Contains(_targetSO.upgradeMethodName) == false)
            {
                _targetSO.upgradeMethodName = upgradeDropdown.choices.Count > 0 
                    ? upgradeDropdown.choices.First() : string.Empty;
                EditorUtility.SetDirty(_targetSO);
            }

            if (rollbackDropdown.choices.Contains(_targetSO.rollbackMethodName) == false)
            {
                _targetSO.rollbackMethodName = rollbackDropdown.choices.Count > 0 
                    ? rollbackDropdown.choices.First() : string.Empty;
                EditorUtility.SetDirty(_targetSO);
            }
            
        }
    }
}