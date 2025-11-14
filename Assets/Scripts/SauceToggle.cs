using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceToggle : MonoBehaviour
{
    [Header("토글할 오브젝트들")]
    [SerializeField] private GameObject[] targetObjects;

    [Header("애니메이션 설정")]
    [SerializeField] private float slideDuration = 0.7f;
    [SerializeField] private float offScreenX = -50f; // UI 기준. 월드일 땐 적절히 조정

    private Vector3[] homePositions;
    private bool areVisible = false;

    void Awake()
    {
        homePositions = new Vector3[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            homePositions[i] = targetObjects[i].transform.localPosition;
            // 시작 상태는 다 화면 밖+비활성화
            targetObjects[i].transform.localPosition = new Vector3(offScreenX, homePositions[i].y, homePositions[i].z);
            targetObjects[i].SetActive(false);
        }
    }

    // 버튼에서 직접 연결
    public void ToggleObjectsSlide()
    {
        if (!areVisible)
        {
            StartCoroutine(SlideInObjectsAllAtOnce());
        }
        else
        {
            StartCoroutine(SlideOutObjectsAllAtOnce());
        }
        areVisible = !areVisible;
    }

    public void ForceSlideOut()
    {
        if (areVisible)
        {
            StartCoroutine(SlideOutObjectsAllAtOnce());
            areVisible = false;
        }
    }

    IEnumerator SlideInObjectsAllAtOnce()
    {
        // 먼저 모두 활성화
        foreach (var go in targetObjects)
            go.SetActive(true);

        float elapsed = 0f;
        // 초기 위치는 모두 화면 밖
        Vector3[] startPositions = new Vector3[targetObjects.Length];
        Vector3[] endPositions = new Vector3[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            startPositions[i] = new Vector3(offScreenX, homePositions[i].y, homePositions[i].z);
            endPositions[i] = homePositions[i];
            targetObjects[i].transform.localPosition = startPositions[i];
        }

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float p = Mathf.Clamp01(elapsed / slideDuration);
            for (int i = 0; i < targetObjects.Length; i++)
            {
                targetObjects[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], p);
            }
            yield return null;
        }
        // 위치 보정
        for (int i = 0; i < targetObjects.Length; i++)
            targetObjects[i].transform.localPosition = endPositions[i];
    }

    IEnumerator SlideOutObjectsAllAtOnce()
    {
        float elapsed = 0f;
        Vector3[] startPositions = new Vector3[targetObjects.Length];
        Vector3[] endPositions = new Vector3[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            startPositions[i] = homePositions[i];
            endPositions[i] = new Vector3(offScreenX, homePositions[i].y, homePositions[i].z);
        }

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float p = Mathf.Clamp01(elapsed / slideDuration);
            for (int i = 0; i < targetObjects.Length; i++)
            {
                targetObjects[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], p);
            }
            yield return null;
        }
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targetObjects[i].transform.localPosition = endPositions[i];
            targetObjects[i].SetActive(false);
        }
    }
}