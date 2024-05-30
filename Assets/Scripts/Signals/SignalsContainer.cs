using Levels;

namespace Signals
{
    public struct LevelSelectedSignal
    {
        public readonly int LevelId;

        public LevelSelectedSignal(int levelId)
        {
            LevelId = levelId;
        }
    }

    public struct LevelSelectorCallSignal
    {
        
    }

    public struct LevelStartedSignal
    {
        public readonly LevelData LevelData;

        public LevelStartedSignal(LevelData levelData)
        {
            LevelData = levelData;
        }
    }
}
