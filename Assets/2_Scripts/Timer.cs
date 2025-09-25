using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0;
    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;
    [HideInInspector] public bool loadNextQuestion;

    public TextMeshProUGUI timerText; // 인스펙터에서 드래그
    public GameObject loadingCanvas;  // 인스펙터에서 드래그

    void Start()
    {
        time = problemTime;
        loadNextQuestion = true;
        UpdateTimerText();
    }
    void Update()
    {
        if (loadingCanvas != null && loadingCanvas.activeSelf)
        {
            // 로딩 캔버스가 활성화 중이면 타이머 동작 중지
            return;
        }

        TimerCountDown();
        UpdateFillAmount();
        UpdateTimerText();
    }

    private void TimerCountDown()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (isProblemTime)
            {
                isProblemTime = false;
                time = solutionTime;
            }
            else
            {
                isProblemTime = true;
                time = problemTime;
                loadNextQuestion = true;
            }
        }
    }
    private void UpdateFillAmount()
    {
        if (isProblemTime)
            fillAmount = time / problemTime;
        else
            fillAmount = time / solutionTime;
    }
    public void CancelTimer()
    {
        time = 0;
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(time).ToString("0");
        }
    }
}
