using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _xBounds;

    [SerializeField]
    private Vector2 _yBounds;

    [SerializeField]
    private float _speed = 10f;

    private void Update()
    {
        transform.Translate(new(Input.GetAxis("Horizontal") * _speed *
            Time.deltaTime, Input.GetAxis("Vertical") * _speed *
            Time.deltaTime, 0f));

        transform.position = new(Mathf.Clamp(transform.position.x,
            _xBounds.x, _xBounds.y), Mathf.Clamp(transform.position.y,
            _yBounds.x, _yBounds.y), transform.position.z);
    }
}
