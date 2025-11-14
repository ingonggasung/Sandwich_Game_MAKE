using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public ReceipeManager recipeManager;

    public TMP_Text dialogueTMPText;

    private Dictionary<string, List<string>> orderDialogues = new Dictionary<string, List<string>>();
    private bool isOrderActive = false;
    private string currentOrderDialogue = "";

    void Start()
    {
        InitializeDialogues();
        if (dialogueTMPText != null)
            dialogueTMPText.text = "";
    }

    private void InitializeDialogues()
    {
        // 대사 초기화 (생략)
    }

    public void DisplayDialogue(string dialogue)
    {
        Debug.Log($"[NPC 대사] {dialogue}");

        if (dialogueTMPText != null)
        {
            dialogueTMPText.text = dialogue;
        }
    }

    public string GenerateRandomOrder()
    {
        if (isOrderActive)
        {
            Debug.Log("[주문 제한] 아직 샌드위치를 제공하지 않았습니다.");
            DisplayDialogue(currentOrderDialogue);
            return currentOrderDialogue;
        }

        if (recipeManager == null)
        {
            Debug.LogError("Recipe Manager가 연결되지 않았습니다.");
            return "";
        }

        var unlockedRecipes = recipeManager.GetUnlockedRecipes();

        if (unlockedRecipes.Count == 0)
        {
            return "현재 해금된 레시피가 없어 주문을 받을 수 없습니다.";
        }

        var selectedReceipe = unlockedRecipes[Random.Range(0, unlockedRecipes.Count)];

        string selectedType = selectedReceipe.stepDescription.Trim();
        int lastSpaceIndex = selectedType.LastIndexOf(' ');

        if (lastSpaceIndex > 0)
            selectedType = selectedType.Substring(0, lastSpaceIndex).Trim();

        if (orderDialogues.ContainsKey(selectedType))
        {
            var dialogues = orderDialogues[selectedType];
            currentOrderDialogue = dialogues[Random.Range(0, dialogues.Count)];
        }
        else
        {
            currentOrderDialogue = $"'{selectedType}' 샌드위치 주문이 들어왔어요.";
        }

        DisplayDialogue(currentOrderDialogue);
        isOrderActive = true;

        return currentOrderDialogue;
    }

    public void CompleteOrder()
    {
        isOrderActive = false;
        currentOrderDialogue = "";
        if (dialogueTMPText != null)
            dialogueTMPText.text = "";
        Debug.Log("[주문 완료] 새로운 주문을 받을 준비가 되었습니다.");
    }
}
