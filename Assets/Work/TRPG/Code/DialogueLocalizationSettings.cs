using UnityEngine;
using UnityEngine.Localization;

namespace Work.TRPG.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueLocalizationSettings", menuName = "Dialogue/Localization Settings")]
    public class DialogueLocalizationSettings : ScriptableObject
    {
        [Header("Language Configuration")]
        public Locale defaultLocale;
        public Locale[] supportedLocales;
        
        [Header("Fallback Behavior")]
        public bool enableFallbackToKey = true;
        public bool logMissingTranslations = true;
        
        [Header("Performance")]
        public bool enableCaching = true;
        public int maxCacheSize = 1000;
        
        private void OnValidate()
        {
            if (supportedLocales == null || supportedLocales.Length == 0)
            {
                supportedLocales = new Locale[] { defaultLocale };
            }
        }
    }
}