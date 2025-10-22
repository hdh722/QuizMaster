using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private quiz quiz;
    [SerializeField] private EndScreen endScreen;
    [SerializeField] private GameObject loadingCanvas;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //ShowQuizScene();
    }

    public void ShowQuizScene()
    {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }
    public void ShowEndScene()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
        loadingCanvas.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QStart()
    {
        //퀴즈맞히러가는
        if (SceneManager.GetActiveScene().name == "Title")
        {
            SceneManager.LoadScene("Game");
        }
    }
    public void Qquit()
    {
        //그만하는
        if (SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("Title");
        }
    }
    public void Logoff()
    {
        //그만하는
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서는 플레이 모드 중지
    #else
        Application.Quit(); // 빌드에서는 앱 종료
    #endif
    }
}
