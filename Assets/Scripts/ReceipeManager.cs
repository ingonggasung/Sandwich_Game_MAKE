using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReceipeManager : MonoBehaviour
{
    public static ReceipeManager Instance { get; private set; }

    public Receipe receipeData;

    private List<ReceipeSet> unlockedRecipes = new List<ReceipeSet>();
    private int currentRecipeIndex = -1;

    private string saveKey = "UnlockedRecipes";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        LoadUnlockedRecipes();

        if (unlockedRecipes.Count == 0)
        {
            UnlockNextTwoRecipes();
            SaveUnlockedRecipes();
        }
    }

    public void UnlockNextTwoRecipes()
    {
        for (int i = 0; i < 2; i++)
        {
            currentRecipeIndex++;
            if (currentRecipeIndex < receipeData.SandwichReceipe.Length)
            {
                UnlockRecipe(currentRecipeIndex);
            }
            else
            {
                Debug.Log("��� �����ǰ� �̹� �رݵǾ����ϴ�.");
                break;
            }
        }
        SaveUnlockedRecipes();
    }

    public void UnlockRecipe(int recipeIndex)
    {
        if (recipeIndex >= 0 && recipeIndex < receipeData.SandwichReceipe.Length)
        {
            ReceipeSet recipeToUnlock = receipeData.SandwichReceipe[recipeIndex];
            if (!unlockedRecipes.Contains(recipeToUnlock))
            {
                unlockedRecipes.Add(recipeToUnlock);
                Debug.Log($"������ �ر�: {recipeToUnlock.stepDescription}");
            }
        }
    }

    public List<ReceipeSet> GetUnlockedRecipes()
    {
        return unlockedRecipes;
    }

    public void SaveUnlockedRecipes()
    {
        List<string> recipeNames = new List<string>();
        foreach (var r in unlockedRecipes)
        {
            recipeNames.Add(r.stepDescription);
        }
        string json = JsonUtility.ToJson(new Serialization<string>(recipeNames));
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public void ResetRecipesToDefault()
    {
        unlockedRecipes.Clear();
        currentRecipeIndex = -1;
        // "햄 샌드위치"가 들어가는 모든 레시피 잠금 해제
        var hamRecipes = receipeData.SandwichReceipe.Where(r => r.stepDescription.Contains("햄 샌드위치"));
        foreach (var hamRecipe in hamRecipes)
        {
            unlockedRecipes.Add(hamRecipe);
        }
        // 저장 및 디버그 출력
        SaveUnlockedRecipes();
        Debug.Log("레시피가 기본값(햄 샌드위치 1/2)으로 초기화되었습니다.");
    }

    public void UnlockRecipesByDay(int day)
    {
        var sandwichList = receipeData.SandwichReceipe.ToList();

        // 종류별 해금 순서 (1일차=0번째)
        List<string> unlockTypes = new List<string> {
        "햄 샌드위치",     // 1일차
        "치즈 샌드위치",   // 2일차
        "참치 샌드위치",   // 3일차
        "치킨 샌드위치",   // 4일차
        "땅콩 샌드위치"    // 5일차
         };
        if (day < 1 || day > unlockTypes.Count)
            day = Mathf.Clamp(day, 1, unlockTypes.Count);

        unlockedRecipes.Clear();

        for (int t = 0; t < day; t++)
        {
            string typeName = unlockTypes[t];
            var foundRecipes = sandwichList.Where(r => r.stepDescription.Contains(typeName));
            foreach (var recipe in foundRecipes)
            {
                if (!unlockedRecipes.Contains(recipe))
                    unlockedRecipes.Add(recipe);
            }
        }

        currentRecipeIndex = unlockedRecipes.Count - 1;

        SaveUnlockedRecipes();
        Debug.Log($"{day}일차 해금: {string.Join(", ", unlockedRecipes.Select(r => r.stepDescription))}");
    }


    public void LoadUnlockedRecipes()
    {
        unlockedRecipes.Clear();
        string json = PlayerPrefs.GetString(saveKey, "");
        if (string.IsNullOrEmpty(json)) return;

        var recipeNamesWrapper = JsonUtility.FromJson<Serialization<string>>(json);
        if (recipeNamesWrapper != null && recipeNamesWrapper.items != null)
        {
            foreach (var name in recipeNamesWrapper.items)
            {
                var r = receipeData.SandwichReceipe.FirstOrDefault(x => x.stepDescription == name);
                if (r != null) unlockedRecipes.Add(r);
            }
        }
    }

    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> items;
        public Serialization(List<T> items) { this.items = items; }
    }
}
