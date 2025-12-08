using System.Collections.Generic;
using UnityEngine;
using Work.TRPG.Code;
using Work.TRPG.Dialogue;

namespace Work.TRPG.Dialogue
{
    public class DialoguePresenter : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private DialogueView dialogueView;
        [SerializeField] private DialogueModel dialogueModel = new();
        [SerializeField] private DialogueLocalizationSettings localizationSettings;

private void Awake()
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindAnyObjectByType<DialogueManager>();
            }
            
            // Initialize the text resolver
            if (localizationSettings != null)
            {
                DialogueTextResolver.Initialize(localizationSettings);
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
            var currentContainer = GetCurrentContainer();
            
            foreach (var choice in node.Choices)
            {
                string resolvedText = DialogueTextResolver.ResolveText(choice.TextKey, currentContainer);
                buffer.Add(new ChoiceButtonModel(choice.ChoiceGuid, resolvedText));
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

            var currentContainer = GetCurrentContainer();
            string resolvedText = DialogueTextResolver.ResolveText(node.TextKey, currentContainer);
            
            dialogueModel.SpeakerName.Value = node.SpeakerId;
            dialogueModel.BodyText.Value = resolvedText;
        }

        private void OnChoiceSelected(string choiceGuid)
        {
            dialogueManager?.SelectChoice(choiceGuid);
        }

        [ContextMenu("Proceed Dialogue")]
private void OnProceed()
        {
            dialogueManager?.Proceed();
        }
        
        private DialogueContainerSO GetCurrentContainer()
        {
            // We need to access the current container from DialogueManager
            // For now, let's add a public property to DialogueManager
            return dialogueManager?.CurrentContainer;
        }
    }
}
