using UnityEngine;

public class ZPositionController : MonoBehaviour
{
    private static float currentZPosition;

    void Start()
    {
        // ���� �� Z�� üũ�ڽ� ��ġ ��� �ʱ�ȭ
        currentZPosition = transform.position.z - 0.5f;
    }

    public void SetNextZPosition(GameObject placedObject)
    {
        Vector3 finalPosition = placedObject.transform.position;
        finalPosition.z = currentZPosition;
        placedObject.transform.position = finalPosition;

        currentZPosition -= 0.5f;
    }

    // �ܺ� ȣ��� - z�� �ʱ�ȭ (��: ������Ʈ �ı� ���� ȣ��)
    public static void ResetZPositionTo15()
    {
        currentZPosition = 13f;
        Debug.Log("[ZPositionController] currentZPosition reset to 13");
    }

    // �� ���� returned (�ʿ� ��)
    public static float GetCurrentZPosition()
    {
        return currentZPosition;
    }
}
