using UnityEngine;
using TMPro; // TextMeshPro를 사용할 경우 필요합니다.

public class UIManager : MonoBehaviour
{
    // Inspector에서 연결
    public TextMeshProUGUI orderText;
    public OrderManager orderManager;
    public Transform cameraPoint;

    // UI 활성화/비활성화 기준값
    private const float ACTIVATION_HEIGHT = 10f; // 텍스트를 활성화할 Y 좌표 기준 (10 이상)
    private const float DEACTIVATION_HEIGHT = 8f;  // 텍스트를 비활성화할 Y 좌표 기준 (8 이하)

    void Update()
    {
        // Y 좌표 기반 UI 활성화/비활성화 로직
        if (cameraPoint == null || orderText == null) return;

        GameObject textObject = orderText.gameObject;
        float currentY = cameraPoint.position.y;

        // Y >= 10: 활성화
        if (currentY >= ACTIVATION_HEIGHT && !textObject.activeSelf)
        {
            textObject.SetActive(true);
            // Debug.Log($"주문 텍스트 활성화됨. Y: {currentY}");
        }
        // Y <= 8: 비활성화
        else if (currentY <= DEACTIVATION_HEIGHT && textObject.activeSelf)
        {
            textObject.SetActive(false);
            // Debug.Log($"주문 텍스트 비활성화됨. Y: {currentY}");
        }
    }

    /// <summary>
    /// 버튼 클릭 시 호출될 메서드: 주문을 생성하고 UI에 표시합니다.
    /// </summary>
    public void GenerateAndDisplayOrder()
    {
        if (orderManager == null || orderText == null)
        {
            Debug.LogError("Order Manager 또는 Order Text가 연결되지 않았습니다.");
            return;
        }

        // 1. OrderManager에서 랜덤 주문 대사를 가져옵니다.
        string newOrderDialogue = orderManager.GenerateRandomOrder();

        // 2. 가져온 주문 대사를 Text 컴포넌트에 할당합니다.
        orderText.text = newOrderDialogue;

        // 3. 주문이 들어왔으니 텍스트를 즉시 활성화합니다. (Update에서 Y 좌표에 따라 다시 꺼질 수 있음)
        orderText.gameObject.SetActive(true);

        Debug.Log("버튼 클릭으로 새로운 주문 생성 완료.");
    }
}