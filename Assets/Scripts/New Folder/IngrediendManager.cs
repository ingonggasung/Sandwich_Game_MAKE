using UnityEngine;
using System.Linq;

public class IngredientManager : MonoBehaviour
{
    public GameObject ingredientPrefab; // 복제할 재료 프리팹
    public GameObject targetPuzzlePiece; // 맞춰야 할 퍼즐 조각
    public float snapDistanceThreshold = 1.5f; // 퍼즐이 맞춰지는 거리 기준

    public float scaleFactor = 0.8f; // 드래그 중 크기 비율

    private GameObject currentDraggedObject;
    private Vector3 originalScale;
    private Vector3 mouseOffset;

    // Z값 관리를 위해 ZPositionController를 연결합니다.
    public ZPositionController zPositionController;

    void OnMouseDown()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        currentDraggedObject = Instantiate(ingredientPrefab, mouseWorldPosition, Quaternion.identity);

        // 잡는 동안 Z값을 -9.9로 고정합니다.
        currentDraggedObject.transform.position = new Vector3(currentDraggedObject.transform.position.x, currentDraggedObject.transform.position.y, -9f);

        originalScale = currentDraggedObject.transform.localScale;
        currentDraggedObject.transform.localScale = originalScale * scaleFactor;

        mouseOffset = currentDraggedObject.transform.position - mouseWorldPosition;
    }

    void OnMouseDrag()
    {
        if (currentDraggedObject != null)
        {
            Vector3 newPosition = GetMouseWorldPosition() + mouseOffset;

            // 드래그 중에도 Z값을 -9로 유지합니다.
            currentDraggedObject.transform.position = new Vector3(newPosition.x, newPosition.y, -9f);
        }
    }

    void OnMouseUp()
    {
        if (currentDraggedObject != null)
        {
            float distance = Vector2.Distance(currentDraggedObject.transform.position, targetPuzzlePiece.transform.position);

            if (distance <= snapDistanceThreshold)
            {
                SnapObjectInPlace();
            }
            else
            {
                Destroy(currentDraggedObject);
            }
        }
    }

    private void SnapObjectInPlace()
    {
        currentDraggedObject.transform.position = targetPuzzlePiece.transform.position;
        currentDraggedObject.transform.localScale = originalScale;

        // Z값 조정은 ZPositionController에 맡깁니다.
        zPositionController.SetNextZPosition(currentDraggedObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}