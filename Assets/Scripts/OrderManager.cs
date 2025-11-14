using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스 추가
using System.Linq;

public class OrderManager : MonoBehaviour
{
    public ReciepeManager recipeManager;

    public TMP_Text dialogueTMPText;  // TextMeshPro 텍스트 컴포넌트 연결용

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
        // 기존 대사 초기화 코드 동일
        orderDialogues.Add("햄 샌드위치", new List<string>
        {
            "햄 샌드위치 하나 부탁해요!",
            "간단하게 햄 샌드위치로 할게요.",
            "신선한 햄 샌드위치 주세요.",
            "햄 샌드위치 급하게 필요해요!"
        });
        // 나머지 대사들도 동일하게...
    }

    public void DisplayDialogue(string dialogue)
    {
        Debug.Log($"[NPC 대사] {dialogue}");

        if (dialogueTMPText != null)
        {
            dialogueTMPText.text = dialogue; // TextMeshPro 텍스트에 표시
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
            return "오류: 레시피 관리자 없음";
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
