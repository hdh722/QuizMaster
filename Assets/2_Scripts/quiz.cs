using TMPro;
using UnityEngine;

public class quiz : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI questionText;
    [SerializeField]Question_SO question;
    [SerializeField] TextMeshProUGUI[] answerTextArr = new TextMeshProUGUI[4];

    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerTextArr.Length; i++)
        {
            answerTextArr[i].text = question.GetAnswer(i);
        }
    }

}
