using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float distance = 2.2f;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] LayerMask mask = ~0;
    [SerializeField] float longPressSeconds = 0.45f;
    [SerializeField] bool shortPressInteracts = true;
    [SerializeField] bool longPressInteracts = false;

    float pressStartedAt;
    bool isPressingInteract;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void OnEnable()
    {
        var action = interactAction != null ? interactAction.action : null;
        if (action == null)
            return;

        action.started += OnInteractStarted;
        action.canceled += OnInteractCanceled;
        action.Enable();
    }

    void OnDisable()
    {
        var action = interactAction != null ? interactAction.action : null;
        if (action == null)
            return;

        action.started -= OnInteractStarted;
        action.canceled -= OnInteractCanceled;
        action.Disable();
    }

    void OnInteractStarted(InputAction.CallbackContext context)
    {
        isPressingInteract = true;
        pressStartedAt = Time.unscaledTime;
    }

    void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (!isPressingInteract)
            return;

        isPressingInteract = false;
        float pressDuration = Time.unscaledTime - pressStartedAt;

        if (pressDuration >= longPressSeconds)
            HandleLongPress(context, pressDuration);
        else
            HandleShortPress(context, pressDuration);
    }

    void HandleShortPress(InputAction.CallbackContext context, float pressDuration)
    {
        string controlPath = context.control != null ? context.control.path : "unknown";
        Debug.Log($"Interact short press ({pressDuration:F2}s) from {controlPath}");

        if (shortPressInteracts)
            TryInteractRaycast();
    }

    void HandleLongPress(InputAction.CallbackContext context, float pressDuration)
    {
        string controlPath = context.control != null ? context.control.path : "unknown";
        Debug.Log($"Interact long press ({pressDuration:F2}s) from {controlPath}");

        if (longPressInteracts)
            TryInteractRaycast();
    }

    void TryInteractRaycast()
    {
        if (!cam)
            cam = Camera.main;

        if (!cam)
            return;

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out var hit, distance, mask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            var interactable =
                hit.collider.GetComponent<DoorInteractable>() ??
                hit.collider.GetComponentInParent<DoorInteractable>() ??
                hit.collider.GetComponentInChildren<DoorInteractable>();

            Debug.Log($"Interactable: {(interactable != null ? interactable.GetType().Name : "null")}");

            interactable?.Interact();
        }
    }
}
