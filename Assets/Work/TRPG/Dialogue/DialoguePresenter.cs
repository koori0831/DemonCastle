using System.Collections.Generic;
using UnityEngine;

namespace Work.TRPG.Dialogue
{
    public class DialoguePresenter : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private DialogueView dialogueView;
        [SerializeField] private DialogueModel dialogueModel = new();

        private void Awake()
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindAnyObjectByType<DialogueManager>();
            }
        }

        private void OnEnable()
        {
            if (dialogueManager == null || dialogueView == null)
            {
                return;
            }

            dialogueManager.OnNodeEntered += HandleNodeEntered;
            dialogueManager.OnChoiceNodePresented += HandleChoiceNode;
            dialogueManager.OnDialogueEnded += HandleDialogueEnded;

            dialogueModel.SpeakerName.OnValueChanged += dialogueView.SetSpeakerName;
            dialogueModel.BodyText.OnValueChanged += dialogueView.SetBodyText;
            dialogueModel.Choices.OnValueChanged += dialogueView.SetChoices;

            dialogueView.OnChoiceClicked += OnChoiceSelected;
            dialogueView.OnProceedRequested += OnProceed;
        }

        private void OnDisable()
        {
            if (dialogueManager != null)
            {
                dialogueManager.OnNodeEntered -= HandleNodeEntered;
                dialogueManager.OnChoiceNodePresented -= HandleChoiceNode;
                dialogueManager.OnDialogueEnded -= HandleDialogueEnded;
            }

            if (dialogueView != null)
            {
                dialogueModel.SpeakerName.OnValueChanged -= dialogueView.SetSpeakerName;
                dialogueModel.BodyText.OnValueChanged -= dialogueView.SetBodyText;
                dialogueModel.Choices.OnValueChanged -= dialogueView.SetChoices;
                dialogueView.OnChoiceClicked -= OnChoiceSelected;
                dialogueView.OnProceedRequested -= OnProceed;
            }
        }

        private void HandleNodeEntered(DialogueState state)

        {
            switch (state.NodeType)
            {
                case DialogueNodeType.Dialogue:
                    UpdateDialogue(state.NodeData as DialogueNodeData);
                    break;
                case DialogueNodeType.Start:
                    dialogueModel.Clear();
                    dialogueView?.ClearChoiceButtons();
                    break;
                case DialogueNodeType.Check:
                    dialogueModel.SetChoices(new List<ChoiceButtonModel>());
                    break;
                case DialogueNodeType.End:
                    dialogueModel.Clear();
                    dialogueView?.ClearChoiceButtons();
                    break;
            }
        }

        private void HandleChoiceNode(ChoiceNodeData node)
        {
            var buffer = new List<ChoiceButtonModel>();
            foreach (var choice in node.Choices)
            {
                buffer.Add(new ChoiceButtonModel(choice.ChoiceGuid, choice.TextKey));
            }

            dialogueModel.SetChoices(buffer);
        }

        private void HandleDialogueEnded()
        {
            dialogueModel.Clear();
            dialogueView?.ClearChoiceButtons();
        }

        private void UpdateDialogue(DialogueNodeData node)
        {
            if (node == null)
            {
                return;
            }

            dialogueModel.SpeakerName.Value = node.SpeakerId;
            dialogueModel.BodyText.Value = node.TextKey;
        }

        private void OnChoiceSelected(string choiceGuid)
        {
            dialogueManager?.SelectChoice(choiceGuid);
        }

        private void OnProceed()
        {
            dialogueManager?.Proceed();
        }
    }
}
