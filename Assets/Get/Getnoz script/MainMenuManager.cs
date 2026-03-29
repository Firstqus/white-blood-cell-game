using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // ลาก BtnStartGame ใส่ใน Inspector แล้ว
    // ผูก OnClick → MainMenuManager → OnStartGame()

    public void OnStartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OnSettingsPressed()
    {
        SceneManager.LoadScene("Settings");
    }
    public void QuitGame()
    {
        PlayerPrefs.Save();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
}
}