using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReciepeManager : MonoBehaviour
{
    public Receipe receipeData;
    private List<ReceipeSet> unlockedRecipes = new List<ReceipeSet>();
    private int currentRecipeIndex = -1;

    public GameManager gameManager; // 연결해줘야 함

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        // 저장된 해금 레시피 리스트 불러와서 복원
        var loaded = gameManager.LoadRecipes();
        if (loaded != null && loaded.Count > 0)
        {
            unlockedRecipes = loaded;
            currentRecipeIndex = unlockedRecipes.Count - 1;
            Debug.Log($"저장된 레시피 개수: {unlockedRecipes.Count}");

            // 저장된 레시피들의 이름 출력
            foreach (var recipe in unlockedRecipes)
            {
                Debug.Log($"저장된 해금 레시피: {recipe.stepDescription}");
            }
        }
        else
        {
            // 저장된 게 없으면 최초 두 개 해금
            UnlockNextTwoRecipes();
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
}
