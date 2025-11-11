using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESC_Manager : MonoBehaviour
{
    public GameObject PausePanel;
    public Button GameOutBTN;
    public Button ResumeBTN;
    void Start()
    {
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // PausePanel이 활성화되어 있지 않으면 (비활성화 상태)
            if (!PausePanel.activeSelf)
            {
                // PausePanel을 활성화합니다.
                PausePanel.SetActive(true);
            }
            // PausePanel이 활성화되어 있으면
            else
            {
                // PausePanel을 비활성화합니다.
                PausePanel.SetActive(false);
            }
        }
    }

    public void GameOut()
    {
        Application.Quit();
        // 게임을 종료합니다.
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
    }
}
