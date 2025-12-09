using UnityEngine;
using TMPro;
using System.Text;

public class RecipePanelToggle : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject recipePanel;          // 레시피 패널 오브젝트
    [SerializeField] private TextMeshProUGUI recipeText;      // 패널 안의 텍스트

    private bool isOpen = false;

    private void Start()
    {
        if (recipePanel != null)
            recipePanel.SetActive(false);
    }

    // 레시피 버튼에서 OnClick으로 연결
    public void ToggleRecipePanel()
    {
        if (recipePanel == null || recipeText == null)
        {
            Debug.LogWarning("RecipePanel 또는 RecipeText가 Inspector에 연결되지 않았습니다.");
            return;
        }

        isOpen = !isOpen;
        recipePanel.SetActive(isOpen);

        if (isOpen)
        {
            UpdateRecipeText();
        }
    }

    private void UpdateRecipeText()
    {
        // ReceipeManager가 있는지 확인
        if (ReceipeManager.Instance == null)
        {
            recipeText.text = "레시피 데이터를 찾을 수 없습니다.";
            return;
        }

        var unlockedList = ReceipeManager.Instance.GetUnlockedRecipes();
        if (unlockedList == null || unlockedList.Count == 0)
        {
            recipeText.text = "해금된 레시피가 없습니다.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("해금된 샌드위치 레시피\n");

        foreach (var recipe in unlockedList)
        {
            // 예: recipe.stepDescription = "햄 샌드위치 1"
            sb.AppendLine($"● {recipe.stepDescription}");

            // 재료 목록이 있다면 간단히 텍스트로 나열 (필요 없으면 이 부분 삭제)
            if (recipe.ingredients != null && recipe.ingredients.Length > 0)
            {
                sb.Append("   재료: ");
                for (int i = 0; i < recipe.ingredients.Length; i++)
                {
                    var ing = recipe.ingredients[i];
                    if (ing != null)
                        sb.Append(ing.name.Replace("(Clone)", "").Trim());

                    if (i < recipe.ingredients.Length - 1)
                        sb.Append(", ");
                }
                sb.AppendLine();
            }

            sb.AppendLine();
        }

        recipeText.text = sb.ToString();
    }
}
