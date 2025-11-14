using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReceipeCheck : MonoBehaviour
{
    // Inspector에서 Receipe 스크립트가 있는 오브젝트를 연결
    public Receipe receipe;

    // 해금된 레시피 목록을 가져오기 위해 RecipeManager 스크립트를 연결합니다.
    public ReciepeManager recipeManager;

    // 검사할 오브젝트들의 태그
    private const string ingredientTag = "Ingredient";

    // 이 스크립트가 있는 오브젝트의 Z값. 이 값을 기준으로 재료와의 거리를 계산합니다.
    private float zCheckPoint;

    void Start()
    {
        zCheckPoint = transform.position.z;
    }

    public void CheckRecipe()
    {
        // 1. "Ingredient" 태그를 가진 모든 게임 오브젝트를 찾습니다.
        GameObject[] allIngredients = GameObject.FindGameObjectsWithTag(ingredientTag);

        if (allIngredients.Length == 0)
        {
            Debug.Log("오브젝트가 놓여있지 않습니다.");
            return;
        }

        // 2. 이 스크립트가 있는 오브젝트보다 Z값이 작은(앞에 있는) 오브젝트들만 필터링합니다.
        var onTopIngredients = allIngredients
            .Where(obj => obj.transform.position.z < zCheckPoint)
            .ToList();

        // 3. 재료들을 스크립트가 있는 오브젝트에 Z값이 가장 가까운 순서대로 정렬합니다.
        onTopIngredients.Sort((a, b) => Mathf.Abs(a.transform.position.z - zCheckPoint).CompareTo(Mathf.Abs(b.transform.position.z - zCheckPoint)));

        // 4. 정렬된 오브젝트들의 이름(프리팹 이름)으로 새로운 문자열 배열을 만듭니다.
        string[] placedIngredientNames = onTopIngredients
            .Select(obj => obj.name.Replace("(Clone)", "").Trim())
            .ToArray();

        // 5. 정렬된 재료 배열을 해금된 레시피 목록과 비교합니다.
        CheckMatch(placedIngredientNames);
    }

    private void CheckMatch(string[] placedIngredients)
    {
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        if (orderManager == null)
        {
            Debug.LogError("OrderManager를 찾을 수 없습니다.");
            return;
        }

        // 1단계: 정답
        foreach (var unlockedRecipe in recipeManager.GetUnlockedRecipes())
        {
            string[] unlockedRecipeIngredientNames = unlockedRecipe.ingredients
                .Select(prefab => prefab.name.Replace("(Clone)", "").Trim())
                .ToArray();

            if (placedIngredients.Length == unlockedRecipeIngredientNames.Length &&
                placedIngredients.SequenceEqual(unlockedRecipeIngredientNames))
            {
                string sandwichName = unlockedRecipe.stepDescription.Trim();
                List<string> successLines = new List<string>
            {
                $"{sandwichName}! 완벽해요!",
                $"바로 이 맛이에요, {sandwichName}!",
                $"{sandwichName}, 정말 맛있게 보이네요!",
                $"이거예요! 제가 주문했던 {sandwichName} 맞아요!"
            };

                orderManager.DisplayDialogue(successLines[Random.Range(0, successLines.Count)]);
                return;
            }
        }

        // 2단계: 잠겨있는 레시피
        foreach (var allRecipe in receipe.SandwichReceipe)
        {
            string[] allRecipeIngredientNames = allRecipe.ingredients
                .Select(prefab => prefab.name.Replace("(Clone)", "").Trim())
                .ToArray();

            if (placedIngredients.Length == allRecipeIngredientNames.Length &&
                placedIngredients.SequenceEqual(allRecipeIngredientNames))
            {
                string sandwichName = allRecipe.stepDescription.Trim();
                List<string> lockedLines = new List<string>
            {
                $"{sandwichName}? 이건 아직 메뉴에 없을 텐데요.",
                $"음... {sandwichName}? 제가 주문한 건 아닌데요.",
                $"{sandwichName}, 생소하네요. 신메뉴인가요?"
            };

                orderManager.DisplayDialogue(lockedLines[Random.Range(0, lockedLines.Count)]);
                return;
            }
        }

        // 3단계: 완전히 불일치
        List<string> failLines = new List<string>
    {
        "음... 이건 제가 주문한 게 아니에요.",
        "이 조합은 조금 이상한데요?",
        "제가 부탁드린 샌드위치가 아닌 것 같아요!"
    };

        orderManager.DisplayDialogue(failLines[Random.Range(0, failLines.Count)]);
    }
}
