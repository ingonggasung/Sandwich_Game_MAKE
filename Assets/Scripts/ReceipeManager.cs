using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    // Inspector에서 기존의 Receipe 스크립트가 붙은 오브젝트를 연결해주세요.
    public Receipe receipeData;

    // 해금된 레시피를 담을 리스트
    private List<ReceipeSet> unlockedRecipes = new List<ReceipeSet>();

    // 현재 해금된 레시피의 인덱스를 추적합니다.
    private int currentRecipeIndex = -1;

    void Start()
    {
        // 게임 시작 시, 첫 두 개의 레시피를 해금합니다.
        UnlockNextTwoRecipes();
    }

    /// <summary>
    /// 버튼 클릭 시 호출하여 다음 두 개의 레시피를 해금합니다.
    /// </summary>
    public void UnlockNextTwoRecipes()
    {
        // 두 개의 레시피를 순차적으로 해금합니다.
        for (int i = 0; i < 2; i++)
        {
            // 다음 인덱스로 이동
            currentRecipeIndex++;

            // 레시피 목록의 범위를 벗어나지 않는지 확인합니다.
            if (currentRecipeIndex < receipeData.SandwichReceipe.Length)
            {
                // 현재 인덱스의 레시피를 해금합니다.
                UnlockRecipe(currentRecipeIndex);
            }
            else
            {
                Debug.Log("모든 레시피가 이미 해금되었습니다.");
                break; // 더 이상 해금할 레시피가 없으면 반복문을 종료합니다.
            }
        }
    }


    // 특정 인덱스의 레시피를 해금하고 해금된 목록에 추가합니다.    
    // <param name="recipeIndex">해금할 레시피의 인덱스</param>
    public void UnlockRecipe(int recipeIndex)
    {
        if (recipeIndex >= 0 && recipeIndex < receipeData.SandwichReceipe.Length)
        {
            ReceipeSet recipeToUnlock = receipeData.SandwichReceipe[recipeIndex];
            if (!unlockedRecipes.Contains(recipeToUnlock))
            {
                unlockedRecipes.Add(recipeToUnlock);
                Debug.Log($"레시피 해금: {recipeToUnlock.stepDescription}");
                // 해금된 레시피 목록을 UI에 표시하는 등 추가 로직을 여기에 구현할 수 있습니다.
            }
        }
    }


    // 해금된 모든 레시피 목록을 반환합니다.    
    public List<ReceipeSet> GetUnlockedRecipes()
    {
        return unlockedRecipes;
    }
}