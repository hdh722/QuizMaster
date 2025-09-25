using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper;

    public void ShowFinalScore()
    {
        finalScoreText.text = "축하인지는 모르겠지만\n\n" + $"당신의 점수는 {scoreKeeper.CalculateScore()}% 입니다.";
    }

    
}
