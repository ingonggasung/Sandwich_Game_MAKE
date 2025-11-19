using UnityEngine;

public class ZPositionController : MonoBehaviour
{
    private static float currentZPosition;

    void Start()
    {
        // 시작 시 Z값 체크박스 위치 기반 초기화
        currentZPosition = transform.position.z - 0.5f;
    }

    public void SetNextZPosition(GameObject placedObject)
    {
        Vector3 finalPosition = placedObject.transform.position;
        finalPosition.z = currentZPosition;
        placedObject.transform.position = finalPosition;

        currentZPosition -= 0.5f;
    }

    // 외부 호출용 - z값 초기화 (예: 오브젝트 파괴 직후 호출)
    public static void ResetZPositionTo15()
    {
        currentZPosition = 14f;
        Debug.Log("[ZPositionController] currentZPosition reset to 13");
    }

    // 현 상태 returned (필요 시)
    public static float GetCurrentZPosition()
    {
        return currentZPosition;
    }
}
