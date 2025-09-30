using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] ScoreKeeper scoreKeeper;

    public void ShowFinalScore()
    {
        Debug.Log($"EndScreen에서 최종 점수: {scoreKeeper.CalculateScore()}");
        finalScoreText.text = "축하합니다!\n\n" + $"총 점수는 {scoreKeeper.CalculateScore()}점 입니다.";
    }
}