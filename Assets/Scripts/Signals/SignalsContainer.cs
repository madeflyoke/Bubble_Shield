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

    public struct LevelResetSignal
    {
        
    }
    
    public struct LevelSelectorCallSignal
    {
        
    }

    public struct RestartLevelCallSignal
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

    public struct LevelCompletedSignal
    {
        public readonly LevelData LevelData;
        public readonly int WrongScoreAnswers;

        public LevelCompletedSignal(LevelData levelData, int wrongScoreAnswers)
        {
            LevelData = levelData;
            WrongScoreAnswers = wrongScoreAnswers;
        }
    }
}
