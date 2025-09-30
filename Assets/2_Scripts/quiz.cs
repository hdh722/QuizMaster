using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class quiz : MonoBehaviour
{
    [Header("����")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("����")]
    [SerializeField] GameObject[] answerButtons;
    
    [Header("��ư ��")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Ÿ�̸�")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("����")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progressbar")]
    [SerializeField] Slider progressBar;
    //public bool isComplete;

    [Header("ChatGPT clietnt")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int questionCount = 9;
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] TextMeshProUGUI HintText;

    [SerializeField] GameObject HintPopup; // �ν����Ϳ��� �巡��
    [SerializeField] Button TimePlus;      // �ν����Ϳ��� �巡��
    [SerializeField] GameObject WinCanvas; // �ν����Ϳ��� �巡��
    [SerializeField] GameObject QuizCanvas;

    bool isGeneratingQuestions = false;


    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        chatGPTClient.quizGenerateHandler += QuizGeneratedHandler;

        if (questions.Count ==0)
        {
            GenerateQuestionsIfNeeded();
        }
        else
        {
            InitailizeProgressBar();
        }

        if (HintPopup != null)
        {
            var button = HintPopup.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(OnHintPopupClicked);
        }
        
        //GetNextQuestion();
    }

    private void GenerateQuestionsIfNeeded()
    {
        if (isGeneratingQuestions) return;

        isGeneratingQuestions = true;
        GameManager.Instance.ShowLoadingScreen();
        string topicToUse = GetTrandTopic();
        chatGPTClient.GenerateQuizQuestions(questionCount, topicToUse);
        Debug.Log($"GenerateQuestionsIfneeded {topicToUse}");
    }

    private string GetTrandTopic()
    {
        string[] topics = new string[] { "����", "����", "������", "����", "��ȭ" };
        int randomIndex = UnityEngine.Random. Range(0, topics.Length);
        return topics[randomIndex];
    }

    void QuizGeneratedHandler(List<QuestionSO> GeneratedQuestions)
    {
        Debug.Log($"QuizGeneratedHandler {GeneratedQuestions.Count}");
        isGeneratingQuestions = false;

        if(GeneratedQuestions == null || GeneratedQuestions.Count ==0)
        {
            Debug.LogError("������������");
            LoadingText.text = "���� ������ �����߽��ϴ�.";
            return;
        }

        Debug.Log("���������Ϸ� ===> " + GeneratedQuestions[0].GetQuestion());
        questions.AddRange(GeneratedQuestions);
        //progressBar.maxValue = GeneratedQuestions.Count;

        GetNextQuestion();

    }


    private void InitailizeProgressBar()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void Update()
    {
        if(timer.isProblemTime)
            timerImage.sprite = problemTimerSprite;
        else
            timerImage.sprite = solutionTimerSprite;
        
        timerImage.fillAmount = timer.fillAmount;

        // solution Ÿ�ӿ��� TimePlus, HintPopup ��Ȱ��ȭ
        if (!timer.isProblemTime)
        {
            if (TimePlus != null)
                TimePlus.interactable = false;
            if (HintPopup != null)
                HintPopup.SetActive(false);
        }

        // �����̴��� 9�� �����ϸ� WinCanvas Ȱ��ȭ
        if (WinCanvas != null && progressBar.value >= 9)
        {
            WinCanvas.SetActive(true);
            QuizCanvas.SetActive(false);
            // 엔드씬으로 전환
            GameManager.Instance.ShowEndScene();
        }

        if (timer.loadNextQuestion)
        {
            if (questions.Count == 0)
            {
                GenerateQuestionsIfNeeded();
                //GameManager.Instance.ShowEndScene();
            }
            else
            {
                //timer.loadNextQuestion = false;
                GetNextQuestion();
            }
        }

        if (timer.isProblemTime ==false && chooseAnswer == false)
        {
            Displaysolution(-1);
        }
    }

    private void GetNextQuestion()
    {
        if(questions.Count <= 0)
        {
            Debug.Log("������������");
            return;
        }

        timer.loadNextQuestion = false;

        GameManager.Instance.ShowQuizScene();
        chooseAnswer = false;
        SetButtonState(true);
        SetDefaultButtonSprite();
        GetRandomQuestion();
        OnDisplayQuestion();
        scoreKeeper.IncrementQuestionSeen();
        progressBar.value++;

        // ���� ���� �� HintPopup Ȱ��ȭ
        if (HintPopup != null)
            HintPopup.SetActive(true);

        // ���� ���� �� TimePlus ��ư Ȱ��ȭ
        if (TimePlus != null)
            TimePlus.interactable = true;
    }

    private void GetRandomQuestion()
    {
        int randomIndex = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        
        questions.RemoveAt(randomIndex);
    }

    private void OnDisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();
        hintText.text = currentQuestion.GetHint();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswer(i);
        }
    }

    public void OnAnswerButtonClicked(int index)
    {
        chooseAnswer = true;
        Displaysolution(index);
        timer.CancelTimer();
        scoreText.text = $"���� : {scoreKeeper.CalculateScore()}��";
    }

    private void Displaysolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "�����Դϴ�!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswer();

            // ���� �ð��� ���� ���� ���� �ο�
            int bonusScore = 1;
            if (timer.isProblemTime && timer != null)
            {
                var timeField = typeof(Timer).GetField("time", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                float remainTime = timeField != null ? (float)timeField.GetValue(timer) : 0f;

                if (remainTime >= 11f && remainTime <= 20f)
                    bonusScore = 3;
                else if (remainTime >= 5f && remainTime <= 10f)
                    bonusScore = 2;
                else if (remainTime >= 0f && remainTime < 5f)
                    bonusScore = 1;
            }

            scoreKeeper.AddScore(bonusScore); // ���� ������ ���ʽ� �ݿ�
            scoreText.text = $"���� : {scoreKeeper.CalculateScore()}��";
        }
        else
        {
            questionText.text = "Ʋ�Ƚ��ϴ� " + currentQuestion.GetcorrectAnswer() + "�� �����Դϴ�.";
            scoreText.text = $"���� : {scoreKeeper.CalculateScore()}��";
        }

        SetButtonState(false);
    }

    private void SetDefaultButtonSprite()
    {
        foreach (GameObject obj in answerButtons)
        {
            obj.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }

    private void SetButtonState(bool state)
    {
        foreach (GameObject odj in answerButtons)
        {
            odj.GetComponent<Button>().interactable = state;
        }
        
    }

    public void OnHintPopupClicked()
    {
        if (HintPopup != null)
            HintPopup.SetActive(false);
    }

    public void TimePlusClicked()
    {
        // Ÿ�̸ӿ� 30�� �߰�
        if (timer != null)
        {
            var timeField = typeof(Timer).GetField("time", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (timeField != null)
            {
                float currentTime = (float)timeField.GetValue(timer);
                timeField.SetValue(timer, currentTime + 20f);
            }
        }

        // ���� 2�� ����
        if (scoreKeeper != null)
        {
            scoreKeeper.AddScore(-2);
            scoreText.text = $"���� : {scoreKeeper.CalculateScore()}��";
        }

        // ��ư ��Ȱ��ȭ
        if (TimePlus != null)
            TimePlus.interactable = false;
    }
}
