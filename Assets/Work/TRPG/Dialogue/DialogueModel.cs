using System.Collections.Generic;
using Core;

namespace Work.TRPG.Dialogue
{
    public class ChoiceButtonModel
    {
        public string ChoiceGuid { get; }
        public string Text { get; }

        public ChoiceButtonModel(string choiceGuid, string text)
        {
            ChoiceGuid = choiceGuid;
            Text = text;
        }
    }

    public class DialogueModel
    {
        public ReactiveProperty<string> SpeakerName { get; } = new(string.Empty);
        public ReactiveProperty<string> BodyText { get; } = new(string.Empty);
        public ReactiveProperty<IReadOnlyList<ChoiceButtonModel>> Choices { get; } = new(new List<ChoiceButtonModel>());

        public void Clear()
        {
            SpeakerName.Value = string.Empty;
            BodyText.Value = string.Empty;
            Choices.Value = new List<ChoiceButtonModel>();
        }

        public void SetChoices(IReadOnlyList<ChoiceButtonModel> choices)
        {
            Choices.Value = choices ?? new List<ChoiceButtonModel>();
        }
    }
}
