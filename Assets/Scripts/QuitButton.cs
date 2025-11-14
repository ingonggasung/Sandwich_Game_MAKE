using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        // 씬에서 GameManager 오브젝트 찾기 (또는 싱글톤 사용 시 GameManager.Instance 등)
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            // 레시피 저장 메서드 호출
            // 여기서 RecipeManager나 GameManager에 저장할 레시피 리스트를 전달해야 함
            // 예를 들어 gameManager.GetUnlockedRecipes() 로 리스트 가져와 저장
            var unlockedRecipes = gameManager.GetUnlockedRecipes();
            gameManager.SaveRecipes(unlockedRecipes);
            Debug.Log("게임 종료 전 레시피 저장 완료");
        }
        else
        {
            Debug.LogWarning("GameManager를 찾을 수 없습니다. 레시피 저장 실패");
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
