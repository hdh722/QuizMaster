using UnityEngine;
using UnityEngine.SceneManagement;

public class Button2 : MonoBehaviour
{
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
