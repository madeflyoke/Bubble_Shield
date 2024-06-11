using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MatchCompleteScoreView : MonoBehaviour
    {
        private const string POSTFIX = "</size>";

        private const string FINAL_SCORE_PREFIX = "FINAL SCORE: <size=120%>";
        private const string RECORD_SCORE_PREFIX = "RECORD : <size=120%>";
        
        [SerializeField] private TMP_Text _finalScoreText;
        [SerializeField] private TMP_Text _recordScoreText;


        public void SetScoreView(int finalScore, int recordScore)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FINAL_SCORE_PREFIX);
            stringBuilder.Append(finalScore.ToString());
            stringBuilder.Append(POSTFIX);
            _finalScoreText.text = stringBuilder.ToString();
            
            stringBuilder.Clear();
            stringBuilder.Append(RECORD_SCORE_PREFIX);
            stringBuilder.Append(recordScore.ToString());
            stringBuilder.Append(POSTFIX);
            _recordScoreText.text = stringBuilder.ToString();
        }
    }
}
