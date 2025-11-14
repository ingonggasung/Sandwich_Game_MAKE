using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ReceipeCheck : MonoBehaviour
{
    public Receipe receipe;
    public ReceipeManager receipeManager;

    private const string ingredientTag = "Ingredient";
    private float zCheckPoint;

    void Start()
    {
        zCheckPoint = transform.position.z;

        if (receipe == null)
            Debug.LogError("receipe is null. Inspector에서 할당 필요");
        if (receipeManager == null)
            Debug.LogError("receipeManager is null. Inspector에서 할당 필요");
    }

    public void CheckRecipe()
    {
        GameObject[] allIngredients = GameObject.FindGameObjectsWithTag(ingredientTag);

        if (allIngredients.Length == 0)
        {
            Debug.Log("오브젝트가 놓여있지 않습니다.");
            return;
        }

        var onTopIngredients = allIngredients
            .Where(obj => obj.transform.position.z < zCheckPoint)
            .ToList();

        onTopIngredients.Sort((a, b) =>
            Mathf.Abs(a.transform.position.z - zCheckPoint).CompareTo(
            Mathf.Abs(b.transform.position.z - zCheckPoint)));

        string[] placedIngredientNames = onTopIngredients
            .Select(obj => obj.name.Replace("(Clone)", "").Trim())
            .ToArray();

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
        if (receipeManager == null)
        {
            Debug.LogError("receipeManager가 null입니다.");
            return;
        }
        if (receipe == null)
        {
            Debug.LogError("receipe가 null입니다.");
            return;
        }

        foreach (var unlockedRecipe in receipeManager.GetUnlockedRecipes())
        {
            string[] unlockedRecipeIngredientNames = unlockedRecipe.ingredients
                .Select(prefab => prefab.name.Replace("(Clone)", "").Trim())
                .ToArray();

            if (placedIngredients.Length == unlockedRecipeIngredientNames.Length &&
                placedIngredients.SequenceEqual(unlockedRecipeIngredientNames))
            {
                string sandwichName = RemoveTrailingNumber(unlockedRecipe.stepDescription);
                List<string> successLines = new List<string>
                {
                    $"{sandwichName}! 완벽해요!",
                    $"바로 이 맛이에요, {sandwichName}!",
                    $"{sandwichName}, 정말 맛있게 보이네요!",
                    $"이거예요! 제가 주문했던 {sandwichName} 맞아요!"
                };

                orderManager.DisplayDialogue(successLines[Random.Range(0, successLines.Count)]);
                if (GameManager.Instance != null)
                    GameManager.Instance.MakeSuccess();
                else
                    Debug.LogError("GameManager.Instance가 null입니다.");
                return;
            }
        }

        foreach (var allRecipe in receipe.SandwichReceipe)
        {
            string[] allRecipeIngredientNames = allRecipe.ingredients
                .Select(prefab => prefab.name.Replace("(Clone)", "").Trim())
                .ToArray();

            if (placedIngredients.Length == allRecipeIngredientNames.Length &&
                placedIngredients.SequenceEqual(allRecipeIngredientNames))
            {
                string sandwichName = RemoveTrailingNumber(allRecipe.stepDescription);
                List<string> lockedLines = new List<string>
                {
                    $"{sandwichName}? 이건 아직 메뉴에 없을 텐데요.",
                    $"음... {sandwichName}? 제가 주문한 건 아닌데요.",
                    $"{sandwichName}, 생소하네요. 신메뉴인가요?"
                };

                orderManager.DisplayDialogue(lockedLines[Random.Range(0, lockedLines.Count)]);
                if (GameManager.Instance != null)
                    GameManager.Instance.MakeFail();
                else
                    Debug.LogError("GameManager.Instance가 null입니다.");
                return;
            }
        }

        List<string> failLines = new List<string>
        {
            "음... 이건 제가 주문한 게 아니에요.",
            "이 조합은 조금 이상한데요?",
            "제가 부탁드린 샌드위치가 아닌 것 같아요!"
        };

        orderManager.DisplayDialogue(failLines[Random.Range(0, failLines.Count)]);
        if (GameManager.Instance != null)
            GameManager.Instance.MakeFail();
        else
            Debug.LogError("GameManager.Instance가 null입니다.");
    }

    private string RemoveTrailingNumber(string input)
    {
        // 이름 뒤 숫자와 공백 제거 (예: "햄 샌드위치 1" → "햄 샌드위치")
        return Regex.Replace(input, @"\s\d+$", "");
    }
}
