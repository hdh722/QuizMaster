using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionSeen = 0;
    int totalScore = 0; // 실제 점수

    public int GetcorrectAnswers() 
    { 
        return correctAnswers;
    }

    public void IncrementCorrectAnswer()
    {
        correctAnswers++;
    }

    public int GetQuestionSeen() 
    { 
        return questionSeen;
    }

    public void IncrementQuestionSeen()
    {
        questionSeen++;
    }

    public void AddScore(int value)
    {
        totalScore += value;
    }

    public int CalculateScore()
    {
        // 실제 점수 반환
        return totalScore;
    }
}
