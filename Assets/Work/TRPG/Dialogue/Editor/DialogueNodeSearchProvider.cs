using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Work.TRPG.Dialogue.Editor
{
    public class DialogueNodeSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView _graphView;
        private EditorWindow _editorWindow;

        public void Initialize(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0)
            };

            foreach (DialogueNodeType nodeType in Enum.GetValues(typeof(DialogueNodeType)))
            {
                if (nodeType == DialogueNodeType.None)
                {
                    continue;
                }

                tree.Add(new SearchTreeEntry(new GUIContent(nodeType.ToString()))
                {
                    level = 1,
                    userData = nodeType
                });
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (_graphView == null || _editorWindow == null || entry.userData is not DialogueNodeType nodeType)
            {
                return false;
            }

            Vector2 screenMousePosition = context.screenMousePosition;
            Vector2 windowMousePosition = screenMousePosition - _editorWindow.position.position;
            var windowRoot = _editorWindow.rootVisualElement;
            Vector2 contentMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, windowMousePosition);
            Vector2 graphMousePosition = _graphView.contentViewContainer.WorldToLocal(contentMousePosition);

            _graphView.CreateNode(nodeType, position: graphMousePosition);
            return true;
        }
    }
}
