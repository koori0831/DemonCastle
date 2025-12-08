using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.TRPG.Dialogue
{
    [Serializable]
    public struct ChoiceData
    {
        [SerializeField] private string choiceGuid;
        [SerializeField] private string textKey;
        [SerializeField] private string nextNodeGuid;

        public string ChoiceGuid => choiceGuid;
        public string TextKey => textKey;
        public string NextNodeGuid => nextNodeGuid;

        public ChoiceData(string textKey, string nextNodeGuid)
        {
            choiceGuid = DialogueGuidUtility.CreateGuid();
            this.textKey = textKey;
            this.nextNodeGuid = nextNodeGuid;
        }

        public ChoiceData(string choiceGuid, string textKey, string nextNodeGuid)
        {
            this.choiceGuid = string.IsNullOrEmpty(choiceGuid) ? DialogueGuidUtility.CreateGuid() : choiceGuid;
            this.textKey = textKey;
            this.nextNodeGuid = nextNodeGuid;
        }

        public ChoiceData WithNextNode(string guid)
        {
            return new ChoiceData(choiceGuid, textKey, guid);
        }
    }

    [Serializable]
    public struct NodeLinkData
    {
        [SerializeField] private string baseNodeGuid;
        [SerializeField] private string targetNodeGuid;
        [SerializeField] private string portName;

        public string BaseNodeGuid => baseNodeGuid;
        public string TargetNodeGuid => targetNodeGuid;
        public string PortName => portName;

        public NodeLinkData(string baseGuid, string targetGuid, string portName)
        {
            baseNodeGuid = baseGuid;
            targetNodeGuid = targetGuid;
            this.portName = portName;
        }
    }

    [Serializable]
    public abstract class NodeData
    {
        [SerializeField] protected string guid;
        [SerializeField] protected Vector2 position;
        [SerializeField] protected DialogueNodeType nodeType;

        public string Guid => guid;
        public Vector2 Position => position;
        public DialogueNodeType NodeType => nodeType;

        protected NodeData()
        {
            guid = DialogueGuidUtility.CreateGuid();
            nodeType = DialogueNodeType.None;
        }

        protected NodeData(DialogueNodeType type) : this()
        {
            nodeType = type;
        }

        public void SetGuid(string nodeGuid)
        {
            guid = string.IsNullOrEmpty(nodeGuid) ? DialogueGuidUtility.CreateGuid() : nodeGuid;
        }

        public void SetPosition(Vector2 nodePosition)
        {
            position = nodePosition;
        }

        public void SetNodeType(DialogueNodeType type)
        {
            nodeType = type;
        }
    }

    [Serializable]
    public sealed class StartNodeData : NodeData
    {
        public StartNodeData() : base(DialogueNodeType.Start) { }
    }

    [Serializable]
    public sealed class EndNodeData : NodeData
    {
        public EndNodeData() : base(DialogueNodeType.End) { }
    }

    [Serializable]
    public sealed class DialogueNodeData : NodeData
    {
        [SerializeField] private string speakerId;
        [SerializeField] private string textKey;

        public string SpeakerId => speakerId;
        public string TextKey => textKey;

        public DialogueNodeData() : base(DialogueNodeType.Dialogue) { }

        public DialogueNodeData(string speaker, string textKey, Vector2 position)
            : base(DialogueNodeType.Dialogue)
        {
            speakerId = speaker;
            this.textKey = textKey;
            SetPosition(position);
        }

        public void SetSpeaker(string speaker)
        {
            speakerId = speaker;
        }

        public void SetTextKey(string key)
        {
            textKey = key;
        }
    }

    [Serializable]
    public sealed class ChoiceNodeData : NodeData
    {
        [SerializeField] private List<ChoiceData> choices = new List<ChoiceData>();

        public IReadOnlyList<ChoiceData> Choices => choices;

        public ChoiceNodeData() : base(DialogueNodeType.Choice) { }

        public ChoiceNodeData(IEnumerable<ChoiceData> choiceList, Vector2 position)
            : base(DialogueNodeType.Choice)
        {
            SetChoices(choiceList);
            SetPosition(position);
        }

        public void SetChoices(IEnumerable<ChoiceData> choiceList)
        {
            choices.Clear();
            if (choiceList == null)
            {
                return;
            }

            choices.AddRange(choiceList);
        }

        public void AddChoice(ChoiceData choice)
        {
            choices.Add(choice);
        }

        public void RemoveChoice(string choiceGuid)
        {
            choices.RemoveAll(choice => choice.ChoiceGuid == choiceGuid);
        }
    }

    [Serializable]
    public sealed class CheckNodeData : NodeData
    {
        [SerializeField] private StatType targetStat = StatType.None;
        [SerializeField] private int statOverride = -1;

        public StatType TargetStat => targetStat;
        public int StatOverride => statOverride;

        public CheckNodeData() : base(DialogueNodeType.Check) { }

        public CheckNodeData(StatType statType, Vector2 position, int overrideValue = -1)
            : base(DialogueNodeType.Check)
        {
            targetStat = statType;
            statOverride = overrideValue;
            SetPosition(position);
        }

        public void SetTargetStat(StatType statType)
        {
            targetStat = statType;
        }

        public void SetOverride(int value)
        {
            statOverride = value;
        }
    }
}
