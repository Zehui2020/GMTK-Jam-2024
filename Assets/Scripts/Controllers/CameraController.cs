using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    private void Update()
    {
        transform.Translate(new(Input.GetAxis("Horizontal") * _speed *
            Time.deltaTime, Input.GetAxis("Vertical") * _speed *
            Time.deltaTime, 0f));
    }
}
