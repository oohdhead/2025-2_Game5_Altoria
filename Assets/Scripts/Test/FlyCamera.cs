using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float fastMultiplier = 3f;      // Shift 누르면 빨라짐
    public float slowMultiplier = 0.25f;   // Ctrl 누르면 느려짐
    public float lookSensitivity = 2f;

    float yaw;
    float pitch;

    void Start()
    {
        var e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        // 오른쪽 마우스 누르고 있을 때만 카메라 회전
        if (!Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mx = Input.GetAxis("Mouse X") * lookSensitivity;
        float my = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw += mx;
        pitch -= my;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void Move()
    {
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= fastMultiplier;
        if (Input.GetKey(KeyCode.LeftControl))
            speed *= slowMultiplier;

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) dir += transform.forward;
        if (Input.GetKey(KeyCode.S)) dir -= transform.forward;
        if (Input.GetKey(KeyCode.A)) dir -= transform.right;
        if (Input.GetKey(KeyCode.D)) dir += transform.right;
        if (Input.GetKey(KeyCode.E)) dir += transform.up;    // 위
        if (Input.GetKey(KeyCode.Q)) dir -= transform.up;    // 아래

        if (dir.sqrMagnitude > 0f)
            transform.position += dir.normalized * speed * Time.unscaledDeltaTime;
    }
}
