using UnityEngine;
using System.Linq;

public class SauceManager : MonoBehaviour
{
    public GameObject startSaucePrefab;
    public GameObject finalSaucePrefab;
    public GameObject targetPuzzlePiece;
    public float snapDistanceThreshold = 1.5f;
    public float scaleFactor = 0.8f;
    public float sauceRotationZ = 135f;

    public ZPositionController zPositionController;

    private GameObject currentDraggedObject;
    private Vector3 originalScale;
    private Vector3 mouseOffset;

    void OnMouseDown()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        currentDraggedObject = Instantiate(startSaucePrefab, mouseWorldPosition, Quaternion.identity);

        // 잡는 동안 Z값을 -9.9로 고정합니다.
        currentDraggedObject.transform.position = new Vector3(currentDraggedObject.transform.position.x, currentDraggedObject.transform.position.y, -9f);

        currentDraggedObject.transform.rotation = Quaternion.Euler(0, 0, sauceRotationZ);

        originalScale = currentDraggedObject.transform.localScale;
        currentDraggedObject.transform.localScale = originalScale * scaleFactor;

        mouseOffset = currentDraggedObject.transform.position - mouseWorldPosition;
    }

    void OnMouseDrag()
    {
        if (currentDraggedObject != null)
        {
            Vector3 newPosition = GetMouseWorldPosition() + mouseOffset;

            // 드래그 중에도 Z값을 -9.9로 유지합니다.
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
        Destroy(currentDraggedObject);
        GameObject finalSauceObject = Instantiate(finalSaucePrefab);

        finalSauceObject.transform.position = targetPuzzlePiece.transform.position;

        zPositionController.SetNextZPosition(finalSauceObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}