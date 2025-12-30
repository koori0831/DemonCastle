using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Blade.Combat.Editor
{
    [CustomPropertyDrawer(typeof(LevelData))]
    public class LevelDataDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            
            IntegerField levelField = new IntegerField();
            levelField.style.flexGrow = 1;
            levelField.bindingPath = "level";
            levelField.label = "Level";
            levelField.Q<Label>().style.minWidth = 20;
            levelField.style.marginRight = 10;
            
            root.Add(levelField);
            
            IntegerField expField = new IntegerField();
            expField.style.flexGrow = 1;
            expField.bindingPath = "requireExp";
            expField.label = "Exp";
            expField.Q<Label>().style.minWidth = 20;
            root.Add(expField);
            
            return root;
        }
    }
}