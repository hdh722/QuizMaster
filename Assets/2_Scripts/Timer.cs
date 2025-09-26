using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Image timerImage;          // 인스펙터에서 드래그

    Color defaultColor = Color.white;

    void Start()
    {
        time = problemTime;
        loadNextQuestion = true;
        UpdateTimerText();

        if (timerImage != null)
            defaultColor = timerImage.color;
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
        UpdateTimerImageColor();
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

    private void UpdateTimerImageColor()
    {
        if (timerImage != null && timerText != null)
        {
            if (isProblemTime && time <= 9f)
            {
                timerImage.color = Color.red;
                timerText.color = Color.yellow;
            }
            else
            {
                timerImage.color = defaultColor;
                timerText.color = Color.white;
            }
        }
    }
}
