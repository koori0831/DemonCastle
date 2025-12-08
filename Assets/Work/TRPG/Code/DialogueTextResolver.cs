using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;

namespace Work.TRPG.Dialogue
{
    public static class DialogueTextResolver
    {
        private static DialogueLocalizationSettings settings;
        private static Dictionary<string, string> textCache = new Dictionary<string, string>();
        
        public static void Initialize(DialogueLocalizationSettings settings)
        {
            DialogueTextResolver.settings = settings;
            ClearCache();
        }
        
        public static string ResolveText(string key, DialogueContainerSO container)
        {
            if (string.IsNullOrEmpty(key) || container == null)
                return string.Empty;
            
            // Check cache first
            string cacheKey = GetCacheKey(key, container);
            if (settings.enableCaching && textCache.TryGetValue(cacheKey, out string cachedText))
                return cachedText;
            
            // Resolve from main table
            string resolvedText = ResolveFromMainTable(key, container);
            
            // Process {} format for reference tables
            if (!string.IsNullOrEmpty(resolvedText))
            {
                resolvedText = ProcessReferenceTables(resolvedText, container);
            }
            
            // Cache the result
            if (settings.enableCaching && !string.IsNullOrEmpty(resolvedText))
            {
                if (textCache.Count >= settings.maxCacheSize)
                    ClearCache();
                
                textCache[cacheKey] = resolvedText;
            }
            
            return resolvedText;
        }
        
        private static string ResolveFromMainTable(string key, DialogueContainerSO container)
        {
            if (container.MainTable == null)
            {
                if (settings.logMissingTranslations)
                    Debug.LogWarning($"No MainTable assigned to container {container.name}");
                return settings.enableFallbackToKey ? key : string.Empty;
            }
            
            var currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale == null)
                currentLocale = settings.defaultLocale;
            
            var table = container.MainTable.GetTable(currentLocale.Identifier.Code) as StringTable;
            if (table == null)
            {
                if (settings.logMissingTranslations)
                    Debug.LogWarning($"No table found for locale {currentLocale.Identifier.Code} in MainTable");
                return settings.enableFallbackToKey ? key : string.Empty;
            }
            
            var entry = table.GetEntry(key);
            if (entry == null)
            {
                if (settings.logMissingTranslations)
                    Debug.LogWarning($"No entry found for key '{key}' in MainTable");
                return settings.enableFallbackToKey ? key : string.Empty;
            }
            
            return entry.GetLocalizedString();
        }
        
        private static string ProcessReferenceTables(string text, DialogueContainerSO container)
        {
            if (container.RelatedTables == null || container.RelatedTables.Count == 0)
                return text;
            
            // Match {} patterns
            var pattern = @"\{([^}]+)\}";
            var matches = Regex.Matches(text, pattern);
            
            string result = text;
            foreach (Match match in matches)
            {
                string referenceKey = match.Groups[1].Value;
                string replacement = ResolveFromReferenceTables(referenceKey, container);
                
                if (!string.IsNullOrEmpty(replacement))
                {
                    result = result.Replace(match.Value, replacement);
                }
            }
            
            return result;
        }
        
        private static string ResolveFromReferenceTables(string key, DialogueContainerSO container)
        {
            if (container.RelatedTables == null)
                return string.Empty;
            
            var currentLocale = LocalizationSettings.SelectedLocale;
            if (currentLocale == null)
                currentLocale = settings.defaultLocale;
            
            // Search through all related tables
            foreach (var table in container.RelatedTables)
            {
                if (table == null) continue;
                
                var localeTable = table.GetTable(currentLocale.Identifier.Code) as StringTable;
                if (localeTable == null) continue;
                
                var entry = localeTable.GetEntry(key);
                if (entry != null)
                {
                    return entry.GetLocalizedString();
                }
            }
            
            if (settings.logMissingTranslations)
                Debug.LogWarning($"No entry found for reference key '{key}' in any related table");
            
            return string.Empty;
        }
        
        private static string GetCacheKey(string key, DialogueContainerSO container)
        {
            var locale = LocalizationSettings.SelectedLocale?.Identifier.Code ?? "default";
            return $"{container.name}_{locale}_{key}";
        }
        
        public static void ClearCache()
        {
            textCache.Clear();
        }
        
        public static void RefreshCache()
        {
            ClearCache();
        }
        
        // Editor helper methods
        public static List<string> GetAvailableKeys(DialogueContainerSO container)
        {
            var keys = new List<string>();
            
            if (container?.MainTable?.SharedData != null)
            {
                foreach (var entry in container.MainTable.SharedData.Entries)
                {
                    keys.Add(entry.Key);
                }
            }
            
            return keys;
        }
        
        public static List<string> GetReferenceKeys(DialogueContainerSO container)
        {
            var keys = new List<string>();
            
            if (container?.RelatedTables != null)
            {
                foreach (var table in container.RelatedTables)
                {
                    if (table?.SharedData != null)
                    {
                        foreach (var entry in table.SharedData.Entries)
                        {
                            if (!keys.Contains(entry.Key))
                                keys.Add(entry.Key);
                        }
                    }
                }
            }
            
            return keys;
        }
    }
}