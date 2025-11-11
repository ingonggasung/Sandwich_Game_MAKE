using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))] // 클릭 받으려면 Collider 필요(BoxCollider 등)
public class CubeMover : MonoBehaviour
{
    private float _targetX = 4.5f;
    private float _speed = 3f;
    private bool _finished = false;
    private CubeSpawner _spawner;

    [Header("UI Label")]
    [SerializeField] private TextMeshPro label; // 큐브 위에 뜰 라벨

    public void Init(float targetX, float speed, CubeSpawner spawner)
    {
        _targetX = targetX;
        _speed = speed;
        _spawner = spawner;
    }

    // 스폰한 쪽에서 문구를 전달
    public void SetLabel(string text)
    {
        if (label != null)
        {
            label.text = text;
        }
    }

    private void Update()
    {
        if (_finished) return;

        transform.position += Vector3.right * _speed * Time.deltaTime;

        // (선택) 라벨이 카메라를 보도록
        if (label != null && Camera.main != null)
            label.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        if (transform.position.x >= _targetX)
        {
            End(false);
        }
    }

    private void OnMouseDown()
    {
        if (_finished) return;
        End(true);
    }

    private void End(bool success)
    {
        _finished = true;

        if (success) GameManager.Instance?.OnCubeSuccess();
        else GameManager.Instance?.OnCubeFail();

        _spawner?.NotifyCubeFinished();
        Destroy(gameObject);
    }
}