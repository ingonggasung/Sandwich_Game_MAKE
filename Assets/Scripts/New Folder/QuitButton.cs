using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        // 종료 직전 현재 진행 상황 저장 (Day, Money, SuccessStreak)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveData();
        }

        Application.Quit();

#if UNITY_EDITOR
        // 에디터에서 실행 중일 때는 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
