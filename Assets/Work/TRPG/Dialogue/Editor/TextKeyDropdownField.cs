using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Work.TRPG.Dialogue;

namespace Work.TRPG.Dialogue.Editor
{
    public class TextKeyDropdownField : VisualElement
    {
        private readonly DropdownField _dropdownField;
        private readonly TextField _textField;
        private readonly Toggle _useDropdownToggle;
        private DialogueContainerSO _container;
        private List<string> _availableKeys = new();
        
        public string Value
        {
            get => _useDropdownToggle.value ? _dropdownField.value : _textField.value;
            set
            {
                if (_availableKeys.Contains(value))
                {
                    _useDropdownToggle.value = true;
                    _dropdownField.value = value;
                    _textField.value = value;
                }
                else
                {
                    _useDropdownToggle.value = false;
                    _textField.value = value;
                    _dropdownField.value = _availableKeys.FirstOrDefault() ?? "";
                }
                
                UpdateVisibility();
            }
        }
        
        public event System.Action<string> OnValueChanged;
        
        public TextKeyDropdownField(string label)
        {
            style.flexDirection = FlexDirection.Column;
            
            // Header with toggle
            var headerRow = new VisualElement
            {
                style = { flexDirection = FlexDirection.Row, alignItems = Align.Center }
            };
            
            var labelElement = new Label(label);
            headerRow.Add(labelElement);
            
            _useDropdownToggle = new Toggle("Use Dropdown") { value = true };
            _useDropdownToggle.RegisterValueChangedCallback(evt => UpdateVisibility());
            headerRow.Add(_useDropdownToggle);
            
            Add(headerRow);
            
            // Dropdown field
            _dropdownField = new DropdownField("Select Key", _availableKeys, 0);
            _dropdownField.RegisterValueChangedCallback(evt => 
            {
                _textField.value = evt.newValue;
                OnValueChanged?.Invoke(evt.newValue);
            });
            Add(_dropdownField);
            
            // Text field for manual entry
            _textField = new TextField("Or Enter Key");
            _textField.RegisterValueChangedCallback(evt => 
            {
                if (!_useDropdownToggle.value)
                {
                    OnValueChanged?.Invoke(evt.newValue);
                }
            });
            Add(_textField);
            
            UpdateVisibility();
        }
        
        public void SetContainer(DialogueContainerSO container)
        {
            _container = container;
            UpdateAvailableKeys();
        }
        
        public void UpdateAvailableKeys()
        {
            string currentValue = Value;
            
            _availableKeys = DialogueTextResolver.GetAvailableKeys(_container);
            _dropdownField.choices = _availableKeys;
            
            if (_availableKeys.Count > 0)
            {
                _dropdownField.index = 0;
                _dropdownField.value = _availableKeys[0];
            }
            
            // Restore current value if it still exists
            if (!string.IsNullOrEmpty(currentValue))
            {
                Value = currentValue;
            }
        }
        
        private void UpdateVisibility()
        {
            bool useDropdown = _useDropdownToggle.value;
            _dropdownField.style.display = useDropdown ? DisplayStyle.Flex : DisplayStyle.None;
            _textField.style.display = useDropdown ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}