using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Work.TRPG.Dialogue.Editor
{
    public class DialogueGraphView : GraphView
    {
        private readonly DialogueEditorWindow _editorWindow;
        private readonly List<DialogueNode> _nodes = new();
        private DialogueNodeSearchProvider _searchProvider;

        public IReadOnlyList<DialogueNode> Nodes => _nodes;
        public IEnumerable<Edge> Edges => edges.ToList();

        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            style.flexGrow = 1f;
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);

            graphViewChanged = OnGraphViewChanged;

            InitializeSearchWindow();
        }

        public void DisposeSearchProvider()
        {
            if (_searchProvider != null)
            {
                ScriptableObject.DestroyImmediate(_searchProvider);
                _searchProvider = null;
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    if (element is DialogueNode node)
                    {
                        _nodes.Remove(node);
                    }
                }
            }

            return change;
        }

        private void InitializeSearchWindow()
        {
            _searchProvider = ScriptableObject.CreateInstance<DialogueNodeSearchProvider>();
            _searchProvider.hideFlags = HideFlags.HideAndDontSave;
            _searchProvider.Initialize(_editorWindow, this);

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchProvider);
        }

        public DialogueNode CreateNode(DialogueNodeType nodeType, string guid = null, Vector2? position = null)
        {
            var node = new DialogueNode(nodeType, guid);
            if (position.HasValue)
            {
                node.SetNodePosition(position.Value);
            }

            AddElement(node);
            _nodes.Add(node);
            return node;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(port =>
                    port != startPort &&
                    port.node != startPort.node &&
                    port.direction != startPort.direction)
                .ToList();
        }

        public void ClearGraph()
        {
            foreach (var edge in Edges.ToList())
            {
                RemoveElement(edge);
            }

            foreach (var node in _nodes.ToList())
            {
                RemoveElement(node);
            }

            _nodes.Clear();
        }

        public DialogueNode FindNode(string guid)
        {
            return _nodes.FirstOrDefault(node => node.Guid == guid);
        }

        public string FindStartNodeGuid()
        {
            return _nodes.FirstOrDefault(n => n.NodeType == DialogueNodeType.Start)?.Guid ?? string.Empty;
        }

        public List<NodeLinkData> BuildLinkData()
        {
            List<NodeLinkData> links = new List<NodeLinkData>();
            foreach (var edge in Edges)
            {
                if (edge.output?.node is not DialogueNode baseNode)
                {
                    continue;
                }

                if (edge.input?.node is not DialogueNode targetNode)
                {
                    continue;
                }

                string portId = edge.output.userData as string ?? edge.output.portName;
                links.Add(new NodeLinkData(baseNode.Guid, targetNode.Guid, portId));
            }

            return links;
        }

        public List<NodeData> BuildNodeData(IReadOnlyList<NodeLinkData> links)
        {
            List<NodeData> nodeData = new List<NodeData>(_nodes.Count);
            foreach (var node in _nodes)
            {
                switch (node.NodeType)
                {
                    case DialogueNodeType.Start:
                        var startData = new StartNodeData();
                        startData.SetGuid(node.Guid);
                        startData.SetPosition(node.GetNodePosition());
                        nodeData.Add(startData);
                        break;
                    case DialogueNodeType.End:
                        var endData = new EndNodeData();
                        endData.SetGuid(node.Guid);
                        endData.SetPosition(node.GetNodePosition());
                        nodeData.Add(endData);
                        break;
                    case DialogueNodeType.Dialogue:
                        var dialogueData = new DialogueNodeData();
                        dialogueData.SetGuid(node.Guid);
                        dialogueData.SetPosition(node.GetNodePosition());
                        dialogueData.SetSpeaker(node.SpeakerField?.value);
                        dialogueData.SetTextKey(node.TextKeyField?.Value);
                        nodeData.Add(dialogueData);
                        break;
                    case DialogueNodeType.Choice:
                        var choiceData = new ChoiceNodeData();
                        choiceData.SetGuid(node.Guid);
                        choiceData.SetPosition(node.GetNodePosition());
                        choiceData.SetChoices(node.BuildChoiceData(links));
                        nodeData.Add(choiceData);
                        break;
                    case DialogueNodeType.Check:
                        var checkData = new CheckNodeData();
                        checkData.SetGuid(node.Guid);
                        checkData.SetPosition(node.GetNodePosition());
                        checkData.SetTargetStat(node.GetTargetStat());
                        checkData.SetOverride(node.GetOverrideValue());
                        nodeData.Add(checkData);
                        break;
                }
            }

            return nodeData;
        }

public void PopulateFromContainer(DialogueContainerSO container)
        {
            ClearGraph();
            if (container == null)
            {
                return;
            }

            var linkList = container.NodeLinks?.ToList() ?? new List<NodeLinkData>();

            foreach (var nodeData in container.NodeDataList)
            {
                var node = CreateNode(nodeData.NodeType, nodeData.Guid, nodeData.Position);
                node.LoadFromData(nodeData);
                node.UpdateTextKeyOptions(container);
            }

            foreach (var link in linkList)
            {
                var baseNode = FindNode(link.BaseNodeGuid);
                var targetNode = FindNode(link.TargetNodeGuid);
                if (baseNode == null || targetNode == null)
                {
                    continue;
                }

                var outputPort = baseNode.GetOutputPort(link.PortName);
                var inputPort = targetNode.InputPort;
                if (outputPort == null || inputPort == null)
                {
                    continue;
                }

                var edge = outputPort.ConnectTo(inputPort);
                AddElement(edge);
            }

            var startNode = FindNode(container.StartNodeGuid);
            if (startNode != null)
            {
                FrameNode(startNode);
            }
        }

public void UpdateAllTextKeyOptions(DialogueContainerSO container)
        {
            foreach (var node in _nodes)
            {
                node.UpdateTextKeyOptions(container);
            }
        }

        private void FrameNode(GraphElement element)
        {
            if (element == null)
            {
                return;
            }

            EditorApplication.delayCall += () =>
            {
                ClearSelection();
                AddToSelection(element);
                FrameSelection();
            };
        }
    }
}

