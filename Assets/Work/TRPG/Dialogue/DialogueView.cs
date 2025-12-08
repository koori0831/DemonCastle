using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.TRPG.Dialogue
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text speakerText;
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private Button choiceButtonPrefab;

        public event Action<string> OnChoiceClicked;
        public event Action OnProceedRequested;

        private readonly List<Button> _choiceButtons = new();

        public void SetSpeakerName(string speaker)
        {
            if (speakerText != null)
            {
                speakerText.text = speaker;
            }
        }

        public void SetBodyText(string text)
        {
            if (bodyText != null)
            {
                bodyText.text = text;
            }
        }

        public void SetChoices(IReadOnlyList<ChoiceButtonModel> choices)
        {
            ClearChoiceButtons();
            if (choices == null || choiceRoot == null || choiceButtonPrefab == null)
            {
                return;
            }

            foreach (var choice in choices)
            {
                var button = Instantiate(choiceButtonPrefab, choiceRoot);
                if (button.TryGetComponent(out TMP_Text buttonText))
                {
                    buttonText.text = choice.Text;
                }
                else if (button.GetComponentInChildren<TMP_Text>() is TMP_Text childText)
                {
                    childText.text = choice.Text;
                }

                string guid = choice.ChoiceGuid;
                button.onClick.AddListener(() => OnChoiceClicked?.Invoke(guid));
                _choiceButtons.Add(button);
            }
        }

        public void ClearChoiceButtons()
        {
            foreach (var button in _choiceButtons)
            {
                if (button == null)
                    continue;

                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }

            _choiceButtons.Clear();
        }

        public void RequestProceed()
        {
            OnProceedRequested?.Invoke();
        }
    }
}
