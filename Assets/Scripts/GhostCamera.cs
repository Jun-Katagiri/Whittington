using UnityEngine;
using UnityEngine.InputSystem; // �������d�v

public class GhostCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mouseSensitivity = 0.5f; // ���x����
    public float interactDistance = 3f;
    public LayerMask interactMask = ~0;

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
        // --- �V���� Input System �ł̏����� ---

        // �h�A���J����iE / ���N���b�N�j
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryInteract();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryInteract();
        }

        // �}�E�X���_�ړ�
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            rotationY += mouseDelta.x * mouseSensitivity;
            rotationX -= mouseDelta.y * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        // �L�[�{�[�h�ړ�
        if (Keyboard.current != null)
        {
            float moveX = 0;
            float moveZ = 0;
            float moveY = 0;

            if (Keyboard.current.wKey.isPressed) moveZ = 1;
            if (Keyboard.current.sKey.isPressed) moveZ = -1;
            if (Keyboard.current.aKey.isPressed) moveX = -1;
            if (Keyboard.current.dKey.isPressed) moveX = 1;
            if (Keyboard.current.eKey.isPressed) moveY = 1; // �㏸
            if (Keyboard.current.qKey.isPressed) moveY = -1; // ���~

            Vector3 move = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
            transform.position += move * moveSpeed * Time.deltaTime;
        }
    }

    void TryInteract()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out var hit, interactDistance, interactMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.GetComponentInParent<DoorController>()?.Toggle();
        }
    }
}