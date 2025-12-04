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
        private VisualElement _tableSettingsContainer;
        private PropertyField _mainTableField;
        private PropertyField _relatedTablesField;
        private SerializedObject _containerSerializedObject;

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
            var toolbar = new Toolbar
            {
                name = "dialogue-editor-toolbar"
            };

            _containerField = new ObjectField("Container")
            {
                objectType = typeof(DialogueContainerSO),
                allowSceneObjects = false,
                value = _activeContainer
            };
            _containerField.RegisterValueChangedCallback(evt =>
            {
                SetActiveContainer(evt.newValue as DialogueContainerSO);
            });
            toolbar.Add(_containerField);

            var saveButton = new Button(SaveData) { text = "Save" };
            var loadButton = new Button(LoadData) { text = "Load" };
            var clearButton = new Button(() => _graphView.ClearGraph()) { text = "Clear" };

            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);

            rootVisualElement.Insert(0, toolbar);
            CreateLocalizationControls();
        }

        private void CreateLocalizationControls()
        {
            if (_tableSettingsContainer != null)
            {
                rootVisualElement.Remove(_tableSettingsContainer);
                _tableSettingsContainer = null;
            }

            var container = new VisualElement
            {
                name = "dialogue-table-settings"
            };
            container.style.flexDirection = FlexDirection.Column;
            container.style.paddingLeft = 6f;
            container.style.paddingRight = 6f;
            container.style.paddingBottom = 4f;
            container.style.paddingTop = 4f;
            //container.style.rowGap = 2f;

            var header = new Label("Localization Tables")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginBottom = 2f
                }
            };
            container.Add(header);

            _mainTableField = new PropertyField
            {
                label = "Main Table",
                bindingPath = "mainTable"
            };
            container.Add(_mainTableField);

            _relatedTablesField = new PropertyField
            {
                label = "Related Tables",
                bindingPath = "relatedTables"
            };
            container.Add(_relatedTablesField);

            _tableSettingsContainer = container;
            rootVisualElement.Insert(1, container);

            UpdateContainerBindings();
        }

        private void SetActiveContainer(DialogueContainerSO container)
        {
            if (_activeContainer == container)
            {
                return;
            }

            _activeContainer = container;
            UpdateContainerBindings();
            _graphView.PopulateFromContainer(_activeContainer);
        }

        private void UpdateContainerBindings()
        {
            if (_mainTableField == null || _relatedTablesField == null)
            {
                return;
            }

            _mainTableField.Unbind();
            _relatedTablesField.Unbind();
            _containerSerializedObject = null;

            bool hasContainer = _activeContainer != null;
            _mainTableField.SetEnabled(hasContainer);
            _relatedTablesField.SetEnabled(hasContainer);

            if (!hasContainer)
            {
                return;
            }

            _containerSerializedObject = new SerializedObject(_activeContainer);
            _mainTableField.Bind(_containerSerializedObject);
            _relatedTablesField.Bind(_containerSerializedObject);
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

