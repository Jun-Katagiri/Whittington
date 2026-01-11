using UnityEngine;
using UnityEngine.InputSystem; // ここが重要

public class GhostCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mouseSensitivity = 0.5f; // 感度調整

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;
    }

    void Update()
    {
        // --- 新しい Input System での書き方 ---

        // マウス視点移動
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            rotationY += mouseDelta.x * mouseSensitivity;
            rotationX -= mouseDelta.y * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        // キーボード移動
        if (Keyboard.current != null)
        {
            float moveX = 0;
            float moveZ = 0;
            float moveY = 0;

            if (Keyboard.current.wKey.isPressed) moveZ = 1;
            if (Keyboard.current.sKey.isPressed) moveZ = -1;
            if (Keyboard.current.aKey.isPressed) moveX = -1;
            if (Keyboard.current.dKey.isPressed) moveX = 1;
            if (Keyboard.current.eKey.isPressed) moveY = 1; // 上昇
            if (Keyboard.current.qKey.isPressed) moveY = -1; // 下降

            Vector3 move = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
            transform.position += move * moveSpeed * Time.deltaTime;
        }
    }
}