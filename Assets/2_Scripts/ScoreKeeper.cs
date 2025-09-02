using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionSeen = 0;

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

    public int CalculateScore()
    {
        return Mathf .RoundToInt((float)correctAnswers / questionSeen * 100);
    }
}
