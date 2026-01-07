using UnityEngine;
using Work.Utils.EventBus;

namespace Work.Save.Code
{
	public class SaveManager : MonoBehaviour
	{
		public readonly static string SaveFolderName = "Saves";

        public CharacterSaveData characterSaveData { get; private set; }

		private void Awake()
		{
			characterSaveData = Load<CharacterSaveData>();
            Bus<SaveCharacterDataEvent>.Events += OnSaveCharacterData;
			Bus<GetCharacterDataByIDEvent>.Events += OnRequestedGetGetCharacterData;
        }

        private void OnDestroy()
		{
			Bus<SaveCharacterDataEvent>.Events -= OnSaveCharacterData;
			Bus<GetCharacterDataByIDEvent>.Events -= OnRequestedGetGetCharacterData;
        }
        private void OnRequestedGetGetCharacterData(GetCharacterDataByIDEvent @event)
        {
			CharacterData data = characterSaveData.GetCharacterDataByID(@event.CharacterID);
			@event.Callback?.Invoke(data);
        }

        private void OnSaveCharacterData(SaveCharacterDataEvent @event)
        {
			SaveCharacterData(@event.SaveData);
        }

        public void SaveCharacterData(CharacterData data = null)
		{
			if (data != null)
				characterSaveData += data;
			Save<CharacterSaveData>(characterSaveData);
        }

		private void Save<T>(T data)
		{
            // 모든 저장 데이터는 Saves 폴더에 T.json 형식으로 저장.
			string folderPath = System.IO.Path.Combine(Application.persistentDataPath, SaveFolderName);
			if (!System.IO.Directory.Exists(folderPath))
				System.IO.Directory.CreateDirectory(folderPath);
			string filePath = System.IO.Path.Combine(folderPath, typeof(T).Name + ".json");
			string json = JsonUtility.ToJson(data, true);
			System.IO.File.WriteAllText(filePath, json);
        }
        private T Load<T>()
        {
			string folderPath = System.IO.Path.Combine(Application.persistentDataPath, SaveFolderName);
			string filePath = System.IO.Path.Combine(folderPath, typeof(T).Name + ".json");
			if (System.IO.File.Exists(filePath))
			{
				string json = System.IO.File.ReadAllText(filePath);
				T data = JsonUtility.FromJson<T>(json);
				return data;
			}
			return default;
        }
    }
}