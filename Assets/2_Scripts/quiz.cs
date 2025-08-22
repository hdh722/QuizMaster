using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Question_SO question;
    //[SerializeField] TextMeshProUGUI[] answerTextArr = new TextMeshProUGUI[4];
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] Sprite defaultAmswerSprite;
    [SerializeField] Sprite correctAmswerSprite;

    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            //answerText[i].text = question.GetAnswer(i);
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtons[index].GetComponent<Image>().sprite = correctAmswerSprite;
        }
    }
}
