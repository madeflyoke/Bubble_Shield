using Match;

namespace Signals
{
    public struct ResetMatchSignal
    {
        
    }
    
    public struct StartMatchCallSignal
    {
        
    }

    public struct MatchStartedSignal
    {
        public readonly MatchData MatchData;

        public MatchStartedSignal(MatchData matchData)
        {
            MatchData =  matchData;
        }
    }

    public struct MatchCompletedSignal
    {
        public readonly int FinalScore;

        public MatchCompletedSignal(int finalScore)
        {
            FinalScore = finalScore;
        }
    }

    public struct FinishZoneHealthEmptySignal
    {
    }
}
