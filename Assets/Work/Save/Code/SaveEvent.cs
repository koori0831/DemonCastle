using Work.Utils.EventBus;

namespace Work.Save.Code
{
    public struct SaveCharacterDataEvent : IEvent
    {
        public CharacterData SaveData { get; private set; }
        public SaveCharacterDataEvent(CharacterData data)
        {
            SaveData = data;
        }
    }

    public struct GetCharacterDataByIDEvent : IEvent
    {
        public int CharacterID { get; private set; }
        // 인자로 CharacterData를 받는 콜백 함수
        public System.Action<CharacterData> Callback { get; private set; }
        public GetCharacterDataByIDEvent(int characterID, System.Action<CharacterData> callback)
        {
            CharacterID = characterID;
            Callback = callback;
        }
    }
}
