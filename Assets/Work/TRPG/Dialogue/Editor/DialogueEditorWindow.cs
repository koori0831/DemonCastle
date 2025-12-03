using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Work.TRPG.Dialogue.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueGraphView _graphView;
        private DialogueContainerSO _activeContainer;
        private ObjectField _containerField;

        [MenuItem("TRPG/Dialogue Editor")]
        public static void Open()
        {
            var window = GetWindow<DialogueEditorWindow>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void OnDisable()
        {
            if (_graphView != null)
            {
                rootVisualElement.Remove(_graphView);
                _graphView.DisposeSearchProvider();
            }
        }

        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView(this)
            {
                name = "Dialogue Graph"
            };

            _graphView.style.flexGrow = 1f;
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            _containerField = new ObjectField("Container")
            {
                objectType = typeof(DialogueContainerSO),
                allowSceneObjects = false,
                value = _activeContainer
            };
            _containerField.RegisterValueChangedCallback(evt =>
            {
                _activeContainer = evt.newValue as DialogueContainerSO;
                _graphView.PopulateFromContainer(_activeContainer);
            });
            toolbar.Add(_containerField);

            var saveButton = new Button(SaveData) { text = "Save" };
            var loadButton = new Button(LoadData) { text = "Load" };
            var clearButton = new Button(() => _graphView.ClearGraph()) { text = "Clear" };

            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);

            rootVisualElement.Insert(0, toolbar);
        }

        private void SaveData()
        {
            if (_activeContainer == null)
            {
                EditorUtility.DisplayDialog("Save Dialogue", "Assign a DialogueContainer asset before saving.", "OK");
                return;
            }

            var links = _graphView.BuildLinkData();
            var nodes = _graphView.BuildNodeData(links);
            var startGuid = _graphView.FindStartNodeGuid();

            Undo.RecordObject(_activeContainer, "Save Dialogue Graph");
            _activeContainer.SetLinks(links);
            _activeContainer.SetNodes(nodes);
            _activeContainer.SetStartNode(startGuid);

            EditorUtility.SetDirty(_activeContainer);
            AssetDatabase.SaveAssets();
        }

        private void LoadData()
        {
            if (_activeContainer == null)
            {
                EditorUtility.DisplayDialog("Load Dialogue", "Assign a DialogueContainer asset before loading.", "OK");
                return;
            }

            _graphView.PopulateFromContainer(_activeContainer);
        }
    }
}
