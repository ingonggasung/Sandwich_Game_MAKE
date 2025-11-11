using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpDown : MonoBehaviour
{
    // 현재 실행 중인 코루틴을 추적합니다.
    private Coroutine movementCoroutine;

    // 이동 속도를 Inspector에서 조절할 수 있습니다.
    [SerializeField] private float moveSpeed = 10f;

    /// <summary>
    /// 오브젝트의 y좌표를 0에서 10으로, 또는 10에서 0으로 부드럽게 전환합니다.
    /// </summary>
    public void ToggleMove()
    {
        // 이미 이동 중이라면 기존 코루틴을 중지하여 새로운 이동을 시작합니다.
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        // 현재 위치를 기준으로 목표 위치를 설정합니다.
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition;

        // 현재 y좌표가 5 이상이면 아래로 (목표 0), 5 미만이면 위로 (목표 10) 이동합니다.
        // 이렇게 하면 중간 지점인 5에서 멈췄을 때도 올바른 방향으로 움직입니다.
        if (currentPosition.y >= 5f)
        {
            targetPosition = new Vector3(currentPosition.x, 0f, currentPosition.z);
        }
        else
        {
            targetPosition = new Vector3(currentPosition.x, 10f, currentPosition.z);
        }

        // 새로운 이동 코루틴을 시작합니다.
        movementCoroutine = StartCoroutine(MoveSmoothly(targetPosition));
    }

    /// <summary>
    /// 지정된 목표 위치로 오브젝트를 부드럽게 이동시키는 코루틴입니다.
    /// </summary>
    /// <param name="targetPos">오브젝트가 도달할 목표 위치</param>
    private IEnumerator MoveSmoothly(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPos, targetPos);

        while (transform.position != targetPos)
        {
            // 시간과 속도를 이용해 이동한 거리를 계산합니다.
            float distCovered = (Time.time - startTime) * moveSpeed;

            // 전체 여정에서 얼마나 이동했는지 비율(0~1)을 계산합니다.
            float fractionOfJourney = distCovered / journeyLength;

            // Vector3.Lerp를 사용하여 시작 위치와 목표 위치 사이를 부드럽게 보간합니다.
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);

            yield return null; // 다음 프레임까지 대기합니다.
        }

        // 이동이 완전히 끝나면 코루틴 참조를 초기화합니다.
        movementCoroutine = null;
    }
}