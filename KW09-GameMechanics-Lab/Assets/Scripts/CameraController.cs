using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float speedScale = 5;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        var cameraTransform = _camera!.transform;
        cameraTransform.transform.position = CreateFromXY(player.position, cameraTransform.position.z);
    }

    private void Update()
    {
        var cameraPosition = _camera.transform.position;
        var targetPosition = CreateFromXY(player.position, cameraPosition.z);
        var distanceToTarget = Vector3.Distance(cameraPosition, targetPosition);
        if (distanceToTarget < 0.05)
        {
            return;
        }
        _camera.transform.position =
            Vector3.Lerp(cameraPosition, targetPosition, Time.deltaTime * speedScale * Mathf.Max(distanceToTarget, 0.1f));
    }

    private Vector3 CreateFromXY(Vector3 originVector, float zComponent = 0)
    {
        return new Vector3(originVector.x, originVector.y, zComponent);
    }
}
