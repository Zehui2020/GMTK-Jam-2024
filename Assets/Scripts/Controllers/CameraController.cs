using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _xBounds;

    [SerializeField]
    private Vector2 _yBounds;

    [SerializeField]
    private float _minZoom = 3f;

    [SerializeField]
    private float _maxZoom = 5f;

    [SerializeField]
    private float _movementSpeed = 10f;

    [SerializeField]
    private float _zoomSpeed = 1f;

    private CinemachineVirtualCamera _virtualCamera;

    private void Update()
    {
        transform.Translate(new(Input.GetAxis("Horizontal") *
            _movementSpeed * Time.deltaTime, Input.GetAxis("Vertical") *
            _movementSpeed * Time.deltaTime, 0f));

        transform.position = new(Mathf.Clamp(transform.position.x,
            _xBounds.x, _xBounds.y), Mathf.Clamp(transform.position.y,
            _yBounds.x, _yBounds.y), transform.position.z);

        _virtualCamera.m_Lens.OrthographicSize =
            Mathf.Clamp(_virtualCamera.m_Lens.OrthographicSize +
            (Input.mouseScrollDelta.y * _zoomSpeed * 0.1f), _minZoom,
            _maxZoom);
    }

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}
