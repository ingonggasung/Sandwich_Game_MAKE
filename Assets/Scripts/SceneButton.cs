using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    // New Game ��ư
    public void GoToNewGame()
    {
        PlayerPrefs.SetString("GameMode", "NewGame");
        PlayerPrefs.Save();
        SceneManager.LoadScene("GamePlay"); // ���� ���� �� �̸�
    }

    // Continue ��ư
    public void GoToContinue()
    {
        PlayerPrefs.SetString("GameMode", "Continue");
        PlayerPrefs.Save();
        SceneManager.LoadScene("GamePlay"); // ���� ���� �� �̸�
    }
}
