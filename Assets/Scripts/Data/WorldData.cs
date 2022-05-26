using System;

namespace Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public int Level = 0;
        public bool IsSwipeTutorialComplete = false;

        public WorldData()
        {
            Level = 0;
            IsSwipeTutorialComplete = false;
        }

        public void CompleteLevel()
        {
            Level++;
            PlayerProgress.Save();
        }

        public void SwipeTutorialCompleted()
        {
            IsSwipeTutorialComplete = true;
            PlayerProgress.Save();
        }
    }
}