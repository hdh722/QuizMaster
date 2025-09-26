using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class quiz : MonoBehaviour
{
    [Header("질문")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("보기")]
    [SerializeField] GameObject[] answerButtons;
    
    [Header("버튼 색")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("타이머")]
    [SerializeField] Image timerImage;
    [SerializeField] Sprite problemTimerSprite;
    [SerializeField] Sprite solutionTimerSprite;
    Timer timer;
    bool chooseAnswer = false;

    [Header("점수")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progressbar")]
    [SerializeField] Slider progressBar;
    //public bool isComplete;

    [Header("ChatGPT clietnt")]
    [SerializeField] ChatGPTClient chatGPTClient;
    [SerializeField] int questionCount = 3;
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] TextMeshProUGUI HintText;

    [SerializeField] GameObject HintPopup; // 인스펙터에서 드래그

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
        string[] topics = new string[] { "예술", "음악", "스포츠", "동물", "문화" };
        int randomIndex = UnityEngine.Random. Range(0, topics.Length);
        return topics[randomIndex];
    }

    void QuizGeneratedHandler(List<QuestionSO> GeneratedQuestions)
    {
        Debug.Log($"QuizGeneratedHandler {GeneratedQuestions.Count}");
        isGeneratingQuestions = false;

        if(GeneratedQuestions == null || GeneratedQuestions.Count ==0)
        {
            Debug.LogError("질문생성실패");
            LoadingText.text = "문제 생성에 실패했습니다.";
            return;
        }

        Debug.Log("질문생성완료 ===> " + GeneratedQuestions[0].GetQuestion());
        questions.AddRange(GeneratedQuestions);
        progressBar.maxValue = GeneratedQuestions.Count;

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
            Debug.Log("남은문제없음");
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

        // 문제 시작 시 HintPopup 활성화
        if (HintPopup != null)
            HintPopup.SetActive(true);
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
        scoreText.text = $"점수 : {scoreKeeper.CalculateScore()}점";

        //if (progressBar.value == progressBar.maxValue)
        //{
        //    isComplete = true;
        //}
    }

    private void Displaysolution(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "정답입니다!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswer();

            // 남은 시간에 따라 점수 차등 부여
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

            scoreKeeper.AddScore(bonusScore); // 실제 점수에 보너스 반영
            scoreText.text = $"점수 : {scoreKeeper.CalculateScore()}점";
        }
        else
        {
            questionText.text = "틀렸습니다 " + currentQuestion.GetcorrectAnswer() + "이 정답입니다.";
            scoreText.text = $"점수 : {scoreKeeper.CalculateScore()}점";
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
}
