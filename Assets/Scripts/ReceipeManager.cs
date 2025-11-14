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
                Debug.Log("모든 레시피가 이미 해금되었습니다.");
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
                Debug.Log($"레시피 해금: {recipeToUnlock.stepDescription}");
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
