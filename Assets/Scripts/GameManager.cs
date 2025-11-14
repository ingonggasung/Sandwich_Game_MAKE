using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string saveFilePath;

    // RecipeManager 인스펙터에서 연결 또는 코드에서 할당
    public ReciepeManager recipeManager;

    [System.Serializable]
    public class RecipeListWrapper
    {
        public List<ReceipeSet> unlockedRecipes;
    }

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "unlockedRecipes.json");

        // RecipeManager가 씬에 있고 자동으로 할당하고 싶을 때
        if (recipeManager == null)
            recipeManager = FindObjectOfType<ReciepeManager>();

        // 현재 해금된 레시피 목록 자동 출력
        var recipes = GetUnlockedRecipes();
        foreach (var recipe in recipes)
        {
            Debug.Log("해금된 레시피: " + recipe.stepDescription);
        }
    }

    // RecipeManager에서 현재 해금된 레시피 리스트 얻어서 반환
    public List<ReceipeSet> GetUnlockedRecipes()
    {
        if (recipeManager != null)
            return recipeManager.GetUnlockedRecipes();
        else
            return new List<ReceipeSet>();
    }

    public void SaveRecipes(List<ReceipeSet> recipes)
    {
        RecipeListWrapper wrapper = new RecipeListWrapper { unlockedRecipes = recipes };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("레시피가 JSON 파일로 저장되었습니다: " + saveFilePath);

        // 디버그용: 현재 저장된 해금 레시피 목록 출력
        foreach (var recipe in recipes)
        {
            Debug.Log("해금된 레시피: " + recipe.stepDescription);
        }
    }

    public List<ReceipeSet> LoadRecipes()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            RecipeListWrapper wrapper = JsonUtility.FromJson<RecipeListWrapper>(json);
            Debug.Log("레시피가 JSON 파일에서 불러와졌습니다.");
            return wrapper.unlockedRecipes;
        }
        else
        {
            Debug.LogWarning("저장된 JSON 파일을 찾을 수 없습니다. 빈 리스트를 반환합니다.");
            return new List<ReceipeSet>();
        }
    }
}
