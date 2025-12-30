using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSystem/Stat", order = 0)]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float currentValue, float previousValue);
        public event ValueChangeHandler OnValueChange;

        public string statName;
        public string description;
        public Sprite icon;
        public string displayName;
        public float incrementStep = 1f;
        
        [SerializeField] private float baseValue;
        [field:SerializeField] public float MinValue {get; set;}
        [field:SerializeField] public float MaxValue {get; set;}

        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();

        [field: SerializeField] public bool IsPercent { get; private set; }

        private float _modifiedValue;

        public float Value => Mathf.Clamp(baseValue + _modifiedValue, MinValue, MaxValue);
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float previousValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChange(Value, previousValue);
            }
        }

        public bool CanIncrementStep()
        {
            return BaseValue + incrementStep <= MaxValue;
        }
        
        private void TryInvokeValueChange(float currentValue, float previousValue)
        {
            if (Mathf.Approximately(currentValue, previousValue) == false)
            {
                OnValueChange?.Invoke(this, currentValue, previousValue);
            }
        }

        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKey.ContainsKey(key)) return; //이미 추가된 키라면 무시
            float previousValue = Value;
            
            _modifiedValue += value;
            _modifyValueByKey.Add(key, value);
            
            TryInvokeValueChange(Value, previousValue);
        }
        
        public void RemoveModifier(object key)
        {
            if (!_modifyValueByKey.TryGetValue(key, out float value)) return; //존재하지 않는 키라면 무시
            float previousValue = Value;
            
            _modifiedValue -= value;
            _modifyValueByKey.Remove(key);
            
            TryInvokeValueChange(Value, previousValue);
        }
        
        public void ClearModifiers()
        {
            float previousValue = Value;
            _modifyValueByKey.Clear();
            _modifiedValue = 0f;
            TryInvokeValueChange(Value, previousValue);
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}