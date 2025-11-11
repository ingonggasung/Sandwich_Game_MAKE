using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnYRange = 2.5f;

    [Header("Timing")]
    [SerializeField] private float firstDelay = 0.6f;
    [SerializeField] private float delayBetweenCubes = 0.6f;

    [Header("Move Settings")]
    [SerializeField] private float minSpeed = 2.2f;
    [SerializeField] private float maxSpeed = 3.8f;
    [SerializeField] private float targetX = 4.5f;

    [Header("Random Labels")]
    [TextArea]
    [SerializeField]
    private string[] labels = new string[]
    {
        "ham",
        "cheeze"
        
    };

    private bool _hasAlive;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            if (!_hasAlive)
            {
                SpawnOne();
                _hasAlive = true;
            }

            yield return new WaitUntil(() => _hasAlive == false);
            yield return new WaitForSeconds(delayBetweenCubes);
        }
    }

    private void SpawnOne()
    {
        if (!cubePrefab || !spawnPoint) return;

        Vector3 pos = spawnPoint.position;
        pos.y += Random.Range(-spawnYRange, spawnYRange);

        GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity);

        var mover = cube.GetComponent<CubeMover>();
        if (mover != null)
        {
            float dayMul = 1f + (GameManager.Instance?.Day ?? 1 - 1) * 0.05f;
            float speed = Random.Range(minSpeed, maxSpeed) * dayMul;
            mover.Init(targetX, speed, this);

            // 라벨 무작위 지정
            if (labels != null && labels.Length > 0)
            {
                int idx = Random.Range(0, labels.Length);
                mover.SetLabel(labels[idx]);
            }
        }
    }

    public void NotifyCubeFinished()
    {
        _hasAlive = false;
    }
}
