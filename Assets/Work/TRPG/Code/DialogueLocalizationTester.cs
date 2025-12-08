using UnityEngine;
using Work.TRPG.Dialogue;

namespace Work.TRPG.Code
{
    public class DialogueLocalizationTester : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private DialogueContainerSO testContainer;
        [SerializeField] private DialogueLocalizationSettings localizationSettings;
        [SerializeField] private string testKey = "test_dialogue";
        [SerializeField] private string testKeyWithPlaceholders = "test_dialogue_with_placeholders";
        
        [Header("Test Results")]
        [SerializeField] private string resolvedText;
        [SerializeField] private string resolvedTextWithPlaceholders;
        [SerializeField] private bool basicTestPassed;
        [SerializeField] private bool placeholderTestPassed;
        [SerializeField] private bool allTestsPassed;
        
        private void Start()
        {
            RunTest();
        }
        
        [ContextMenu("Run Test")]
        private void RunTest()
        {
            if (testContainer == null)
            {
                Debug.LogError("Test container is not assigned!");
                basicTestPassed = false;
                placeholderTestPassed = false;
                allTestsPassed = false;
                return;
            }
            
            if (localizationSettings == null)
            {
                Debug.LogError("Localization settings are not assigned!");
                basicTestPassed = false;
                placeholderTestPassed = false;
                allTestsPassed = false;
                return;
            }
            
            // Initialize the resolver
            DialogueTextResolver.Initialize(localizationSettings);
            
            // Test basic resolution
            RunBasicTest();
            
            // Test {} format resolution
            RunPlaceholderTest();
            
            // Test cache functionality
            RunCacheTest();
            
            // Overall result
            allTestsPassed = basicTestPassed && placeholderTestPassed;
            Debug.Log($"=== ALL TESTS: {(allTestsPassed ? "PASSED" : "FAILED")} ===");
        }
        
        private void RunBasicTest()
        {
            resolvedText = DialogueTextResolver.ResolveText(testKey, testContainer);
            basicTestPassed = !string.IsNullOrEmpty(resolvedText) && resolvedText != testKey;
            
            Debug.Log($"=== Basic Resolution Test: {(basicTestPassed ? "PASSED" : "FAILED")} ===");
            Debug.Log($"Test Key: {testKey}");
            Debug.Log($"Resolved Text: {resolvedText}");
        }
        
        private void RunPlaceholderTest()
        {
            resolvedTextWithPlaceholders = DialogueTextResolver.ResolveText(testKeyWithPlaceholders, testContainer);
            
            // Check if placeholders were resolved (text should be different from original key)
            bool hasPlaceholders = resolvedTextWithPlaceholders != testKeyWithPlaceholders;
            // Check if any {} patterns remain (they shouldn't if reference tables are set up correctly)
            bool hasUnresolvedPlaceholders = System.Text.RegularExpressions.Regex.IsMatch(resolvedTextWithPlaceholders, @"\{[^}]+\}");
            
            placeholderTestPassed = hasPlaceholders && !hasUnresolvedPlaceholders;
            
            Debug.Log($"=== Placeholder Resolution Test: {(placeholderTestPassed ? "PASSED" : "FAILED")} ===");
            Debug.Log($"Test Key with Placeholders: {testKeyWithPlaceholders}");
            Debug.Log($"Resolved Text: {resolvedTextWithPlaceholders}");
            Debug.Log($"Has Placeholders: {hasPlaceholders}");
            Debug.Log($"Has Unresolved Placeholders: {hasUnresolvedPlaceholders}");
        }
        
        private void RunCacheTest()
        {
            var cachedResult = DialogueTextResolver.ResolveText(testKey, testContainer);
            bool cacheWorks = cachedResult == resolvedText;
            Debug.Log($"Cache Test: {(cacheWorks ? "PASSED" : "FAILED")}");
        }
        
        [ContextMenu("Clear Cache")]
        private void ClearCache()
        {
            DialogueTextResolver.ClearCache();
            Debug.Log("Dialogue text cache cleared.");
        }
        
        [ContextMenu("List Available Keys")]
        private void ListAvailableKeys()
        {
            if (testContainer == null)
            {
                Debug.LogError("Test container is not assigned!");
                return;
            }
            
            var keys = DialogueTextResolver.GetAvailableKeys(testContainer);
            Debug.Log($"=== Available Main Table Keys ({keys.Count}): ===");
            foreach (var key in keys)
            {
                Debug.Log($"  - {key}");
            }
        }
        
        [ContextMenu("List Reference Keys")]
        private void ListReferenceKeys()
        {
            if (testContainer == null)
            {
                Debug.LogError("Test container is not assigned!");
                return;
            }
            
            var keys = DialogueTextResolver.GetReferenceKeys(testContainer);
            Debug.Log($"=== Available Reference Table Keys ({keys.Count}): ===");
            foreach (var key in keys)
            {
                Debug.Log($"  - {key}");
            }
        }
        
        [ContextMenu("Test Specific Key")]
        private void TestSpecificKey()
        {
            if (testContainer == null)
            {
                Debug.LogError("Test container is not assigned!");
                return;
            }
            
            string keyToTest = "player_name"; // Example reference key
            string resolved = DialogueTextResolver.ResolveText(keyToTest, testContainer);
            Debug.Log($"Testing reference key '{keyToTest}': '{resolved}'");
        }
        
        [ContextMenu("Test Edge Cases")]
        private void TestEdgeCases()
        {
            if (testContainer == null || localizationSettings == null)
            {
                Debug.LogError("Test setup incomplete!");
                return;
            }
            
            DialogueTextResolver.Initialize(localizationSettings);
            
            Debug.Log("=== Testing Edge Cases ===");
            
            // Test empty key
            var result1 = DialogueTextResolver.ResolveText("", testContainer);
            Debug.Log($"Empty key: '{result1}' (should be empty)");
            
            // Test null key
            var result2 = DialogueTextResolver.ResolveText(null, testContainer);
            Debug.Log($"Null key: '{result2}' (should be empty)");
            
            // Test null container
            var result3 = DialogueTextResolver.ResolveText("test_key", null);
            Debug.Log($"Null container: '{result3}' (should be empty)");
            
            // Test key with malformed placeholders
            var result4 = DialogueTextResolver.ResolveText("Hello {world", testContainer);
            Debug.Log($"Malformed placeholder: '{result4}'");
            
            // Test key with multiple placeholders
            var result5 = DialogueTextResolver.ResolveText("Hello {name}, you have {gold} gold", testContainer);
            Debug.Log($"Multiple placeholders: '{result5}'");
            
            // Test key with non-existent reference
            var result6 = DialogueTextResolver.ResolveText("Hello {nonexistent_key}", testContainer);
            Debug.Log($"Non-existent reference: '{result6}'");
        }
        
        [ContextMenu("Test Placeholder Examples")]
        private void TestPlaceholderExamples()
        {
            if (testContainer == null || localizationSettings == null)
            {
                Debug.LogError("Test setup incomplete!");
                return;
            }
            
            DialogueTextResolver.Initialize(localizationSettings);
            
            Debug.Log("=== Testing Placeholder Examples ===");
            
            string[] testExamples = {
                "Hello {player_name}!",
                "You have {gold_amount} gold coins.",
                "The {enemy_name} attacks for {damage} damage!",
                "Welcome to {location_name}, {player_title}.",
                "Multiple refs: {item_name} x{quantity}"
            };
            
            foreach (var example in testExamples)
            {
                var resolved = DialogueTextResolver.ResolveText(example, testContainer);
                Debug.Log($"Input:  {example}");
                Debug.Log($"Output: {resolved}");
                Debug.Log("---");
            }
        }
    }
}