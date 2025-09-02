using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    public string question = "여기에 질문을 적어주세요";
    public string[] answers = new string[4];
    public int correctAnswerIndex;

    public string GetQuestion()
    {
        return question; 
    }
    public string GetAnswer(int index)
    {
        return answers[index];
    }
    public string GetcorrectAnswer()
    {
        return answers[correctAnswerIndex];
    }
    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }



}

