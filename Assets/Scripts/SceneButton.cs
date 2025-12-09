using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    // New Game
    public void GoToNewGame()
    {
        PlayerPrefs.SetString("GameMode", "NewGame");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Sandwich_Make");
    }

    // Continue
    public void GoToContinue()
    {
        PlayerPrefs.SetString("GameMode", "Continue");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Sandwich_Make");
    }
}
