using UnityEngine;
using UnityEditor;

namespace Work.TRPG.Dialogue
{
    public static class DialogueLocalizationSettingsCreator
    {
        [MenuItem("Assets/Create/TRPG/Dialogue/Localization Settings")]
        public static void CreateSettings()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save Dialogue Localization Settings",
                "DialogueLocalizationSettings",
                "asset",
                "Save Dialogue Localization Settings");
            
            if (!string.IsNullOrEmpty(path))
            {
                var settings = ScriptableObject.CreateInstance<DialogueLocalizationSettings>();
                AssetDatabase.CreateAsset(settings, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settings;
            }
        }
    }
}