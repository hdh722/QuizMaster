using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class Question_SO : ScriptableObject
{
    [TextArea(2,6)]
    public string question = "여기에 질문을 적어주세요";
}

