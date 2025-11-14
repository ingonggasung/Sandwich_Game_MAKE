using UnityEngine;

public class ZPositionController : MonoBehaviour
{
    private static float currentZPosition;

    void Start()
    {
        // 시작 시 Z값을 CheckBox 오브젝트의 위치에서 -0.5f로 설정합니다.
        currentZPosition = transform.position.z - 0.5f;
    }

    /// <summary>
    /// 놓인 오브젝트의 Z값을 설정하고, 다음 Z값을 준비합니다.
    /// </summary>
    /// <param name="placedObject">Z값을 조정할 오브젝트</param>
    public void SetNextZPosition(GameObject placedObject)
    {
        Vector3 finalPosition = placedObject.transform.position;
        finalPosition.z = currentZPosition;
        placedObject.transform.position = finalPosition;

        // 다음 오브젝트를 위해 Z값을 0.5f씩 줄입니다.
        currentZPosition -= 0.5f;
    }
}