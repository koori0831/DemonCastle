using System;

namespace Work.TRPG.Dialogue
{
    public struct DialogueState
    {
        public string CurrentNodeGuid;
        public DialogueNodeType NodeType;
        public NodeData NodeData;
    }

    public interface IDialogueEvents
    {
        event Action<DialogueState> OnNodeEntered;
        event Action OnDialogueEnded;
        event Action<CheckNodeData> OnCheckNodeRequested;
        event Action<ChoiceNodeData> OnChoiceNodePresented;
    }
}
