using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Work.TRPG.Dialogue.Editor
{
    public class DialogueNode : Node
    {
        private const float NodeWidth = 320f;
        private const float NodeHeight = 200f;

        private readonly Dictionary<string, TextKeyDropdownField> _choiceFields = new();
        private Port _inputPort;
        private readonly Dictionary<string, Port> _outputPorts = new();

        public string Guid { get; private set; }
        public DialogueNodeType NodeType { get; }

public TextField SpeakerField { get; private set; }
        public TextKeyDropdownField TextKeyField { get; private set; }
        public EnumField StatField { get; private set; }
        public IntegerField StatOverrideField { get; private set; }

        public Port InputPort => _inputPort;

        public DialogueNode(DialogueNodeType type, string guid = null)
        {
            Guid = string.IsNullOrEmpty(guid) ? DialogueGuidUtility.CreateGuid() : guid;
            NodeType = type;
            title = type.ToString();
            SetPosition(new Rect(Vector2.zero, new Vector2(NodeWidth, NodeHeight)));
            capabilities |= Capabilities.Movable | Capabilities.Deletable | Capabilities.Selectable;

            switch (type)
            {
                case DialogueNodeType.Start:
                    AddOutputPort("Next");
                    break;
                case DialogueNodeType.End:
                    AddInputPort();
                    break;
                case DialogueNodeType.Dialogue:
                    AddInputPort();
                    AddOutputPort("Next");
                    CreateDialogueFields();
                    break;
                case DialogueNodeType.Choice:
                    AddInputPort();
                    CreateChoicePorts();
                    break;
                case DialogueNodeType.Check:
                    AddInputPort();
                    CreateCheckPorts();
                    CreateCheckFields();
                    break;
            }

            RefreshExpandedState();
            RefreshPorts();
        }

        public void SetNodePosition(Vector2 position)
        {
            SetPosition(new Rect(position, new Vector2(NodeWidth, NodeHeight)));
        }

        public Vector2 GetNodePosition()
        {
            return GetPosition().position;
        }

        public void LoadFromData(NodeData data)
        {
            if (data == null)
            {
                return;
            }

            Guid = data.Guid;
            SetNodePosition(data.Position);

            switch (data)
            {
                case DialogueNodeData dialogue:
                    if (SpeakerField != null)
                    {
                        SpeakerField.value = dialogue.SpeakerId;
                    }

                    if (TextKeyField != null)
                    {
                        TextKeyField.Value = dialogue.TextKey;
                    }

                    break;
                case ChoiceNodeData choice:
                    LoadChoices(choice);
                    break;
                case CheckNodeData check:
                    if (StatField != null)
                    {
                        StatField.value = check.TargetStat;
                    }

                    if (StatOverrideField != null)
                    {
                        StatOverrideField.value = check.StatOverride;
                    }

                    break;
            }
        }

        private void LoadChoices(ChoiceNodeData choiceData)
        {
            foreach (var guid in _choiceFields.Keys.ToList())
            {
                RemoveChoicePort(guid);
            }

            if (choiceData?.Choices == null)
            {
                return;
            }

            foreach (var choice in choiceData.Choices)
            {
                AddChoicePort(choice.TextKey, choice.ChoiceGuid);
            }
        }

private void CreateDialogueFields()
        {
            SpeakerField = new TextField("Speaker ID");
            extensionContainer.Add(SpeakerField);

            TextKeyField = new TextKeyDropdownField("Text Key");
            extensionContainer.Add(TextKeyField);
        }

        private void CreateChoicePorts()
        {
            AddChoicePort("Choice 1");
            AddChoicePort("Choice 2");

            var addButton = new Button(() => AddChoicePort()) { text = "+" };
            var removeButton = new Button(RemoveLastChoice) { text = "-" };

            var controlRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    marginTop = 4,
                    marginBottom = 4
                }
            };

            controlRow.Add(addButton);
            controlRow.Add(removeButton);
            mainContainer.Add(controlRow);
        }

        private void RemoveLastChoice()
        {
            if (_choiceFields.Count == 0)
            {
                return;
            }

            var last = _choiceFields.Keys.Last();
            RemoveChoicePort(last);
        }

        private void CreateCheckFields()
        {
            StatField = new EnumField("Target Stat", StatType.None);
            extensionContainer.Add(StatField);

            StatOverrideField = new IntegerField("Override (-1)") { value = -1 };
            extensionContainer.Add(StatOverrideField);
        }

        private void CreateCheckPorts()
        {
            AddOutputPort("Critical");
            AddOutputPort("Success");
            AddOutputPort("Failure");
            AddOutputPort("Fumble");
        }

        private void AddInputPort(string portName = "Input")
        {
            if (_inputPort != null)
            {
                return;
            }

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            _inputPort.portName = portName;
            inputContainer.Add(_inputPort);
        }

        private Port AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(float));
            port.portName = portName;
            port.userData = portName;
            outputContainer.Add(port);
            _outputPorts[portName] = port;
            return port;
        }

private void AddChoicePort(string label = "Choice", string guid = null)
        {
            var port = AddOutputPort(guid ?? DialogueGuidUtility.CreateGuid());
            string key = port.portName;

            var field = new TextKeyDropdownField("Text Key") { Value = label };
            field.style.flexGrow = 1f;
            _choiceFields[key] = field;

            var choiceRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center
                }
            };
            choiceRow.style.marginTop = 2f;
            choiceRow.style.marginBottom = 2f;
            choiceRow.style.paddingLeft = 2f;
            choiceRow.style.paddingRight = 2f;

            choiceRow.Add(field);
            var removeButton = new Button(() => RemoveChoicePort(key)) { text = "X" };
            choiceRow.Add(removeButton);

            port.contentContainer.Add(choiceRow);
        }

        private void RemoveChoicePort(string guid)
        {
            if (!_outputPorts.TryGetValue(guid, out var port))
            {
                return;
            }

            port.DisconnectAll();
            outputContainer.Remove(port);
            _outputPorts.Remove(guid);
            _choiceFields.Remove(guid);
        }

        public Port GetOutputPort(string portName)
        {
            return _outputPorts.TryGetValue(portName, out var port) ? port : null;
        }

        public StatType GetTargetStat()
        {
            return StatField != null ? (StatType)StatField.value : StatType.None;
        }

        public int GetOverrideValue()
        {
            return StatOverrideField != null ? StatOverrideField.value : -1;
        }

public void UpdateTextKeyOptions(DialogueContainerSO container)
        {
            if (TextKeyField != null)
            {
                TextKeyField.SetContainer(container);
                TextKeyField.UpdateAvailableKeys();
            }

            foreach (var choiceField in _choiceFields.Values)
            {
                choiceField.SetContainer(container);
                choiceField.UpdateAvailableKeys();
            }
        }

        public IEnumerable<ChoiceData> BuildChoiceData(IEnumerable<NodeLinkData> links)
        {
            if (NodeType != DialogueNodeType.Choice)
            {
                yield break;
            }

            foreach (var pair in _choiceFields)
            {
                var nextLink = links?.FirstOrDefault(l => l.BaseNodeGuid == Guid && l.PortName == pair.Key);
                string nextGuid = nextLink?.TargetNodeGuid;
                yield return new ChoiceData(pair.Key, pair.Value.Value, nextGuid);
            }
        }
    }
}
