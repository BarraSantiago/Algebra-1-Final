using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 0.25f;

    private Vector3 lastMousePosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            position += transform.forward * (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            position -= transform.forward * (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            position -= transform.right * (speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            position += transform.right * (speed * Time.deltaTime);
        }
        transform.position = position;

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        transform.rotation *= Quaternion.Euler(new Vector3(0, mouseDelta.x * sensitivity, 0));
        lastMousePosition = Input.mousePosition;
    }
}
