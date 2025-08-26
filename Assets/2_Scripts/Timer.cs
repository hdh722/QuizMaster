using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float problemTime = 10f;
    [SerializeField] float solutionTime = 3f;
    float time = 0;
    [HideInInspector] public bool isProblemTime = true;
    [HideInInspector] public float fillAmount;

    void Start()
    {
        time = problemTime;
    }
    void Update()
    {
        TimerCountDown();
        UpdateFillAmount();

    }

    private void UpdateFillAmount()
    {
        if (isProblemTime)
            fillAmount = time / problemTime;
        else
            fillAmount = time / solutionTime;
    }

    private void TimerCountDown()
    {
        Debug.Log("남은시간: " + time);
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
            }
        }
    }
}
