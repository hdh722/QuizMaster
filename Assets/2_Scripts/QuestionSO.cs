using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] private string question = "여기에 질문을 적어주세요";
    [SerializeField] private string[] answers = new string[4];
    [SerializeField] private int correctAnswerIndex;
    [SerializeField] private string hint = "";

    public string GetQuestion()
    {
        return question; 
    }
    public string GetHint()
    {
        return hint; 
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

    public void SetData(string q, string[] a, int correctIndex, string h)
    {
        SetData(q, a, correctIndex);
        hint = h;
    }
    public void SetData(string q, string[] a, int correctIndex)
    {
        question = q;
        answers = a;
        correctAnswerIndex = correctIndex;
    }


}

