using System;

namespace Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public int Level = 0;

        public WorldData()
        {
            Level = 0;
        }

        public void CompleteLevel()
        {
            Level++;
            PlayerProgress.Save();
        }
    }
}