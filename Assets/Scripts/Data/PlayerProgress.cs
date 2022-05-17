using UnityEngine;

namespace Scripts.Data
{
    public class PlayerProgress
    {
        private static WorldData _worldData;
        private const string DATA_KEY_PARAMETER = "Data";

        public static WorldData GetData()
        {
            if (_worldData != null)
            {
                return _worldData;
            }

            _worldData = Load() ?? new WorldData();

            return _worldData;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Data/Remove data")]
        public static void RemoveAccount()
        {
            PlayerPrefs.DeleteKey(DATA_KEY_PARAMETER);
            PlayerPrefs.Save();
            Debug.Log("Success remove account data");
        }

        [UnityEditor.MenuItem("Data/Previous Level")]
        public static void PreviousLevel()
        {
            GetData().Level--;
            _worldData.Level = Mathf.Clamp(_worldData.Level, 0, int.MaxValue);
            Save();
        }

        [UnityEditor.MenuItem("Data/Next Level")]
        public static void NextLevel()
        {
            GetData().Level++;
            _worldData.Level = Mathf.Clamp(_worldData.Level, 0, int.MaxValue);
            Save();
        }
#endif

        public static void Save()
        {
            var json = GetData().ToJson();
            PlayerPrefs.SetString(DATA_KEY_PARAMETER, json);
            PlayerPrefs.Save();
        }

        private static WorldData Load()
        {
            var json = PlayerPrefs.GetString(DATA_KEY_PARAMETER);
            return json.ToDeserialized<WorldData>();
        }
    }
}