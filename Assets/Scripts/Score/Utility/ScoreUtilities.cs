using UnityEngine;

namespace Score.Utility
{
    public static class ScoreUtilities
    {
        public static int GetStarsCountByScore(int targetScore, int wrongAnswersScore, int maxStars, float enemiesPerAllyRatio = 1.2f)
        {
            int stars = maxStars;
            
            for (int i = 0; i < maxStars; i++)
            {
                int step =  Mathf.CeilToInt(targetScore * enemiesPerAllyRatio) / (maxStars-1) * i;
                if (wrongAnswersScore<=step)
                {
                    return stars;
                }
                
                stars = Mathf.Clamp(--stars, 1, maxStars);
            }

            return stars;
        }
        
    }
}
