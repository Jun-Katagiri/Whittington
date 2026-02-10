using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float distance = 2.2f;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] LayerMask mask = ~0;
    [SerializeField] float longPressSeconds = 0.45f;
    // [SerializeField] bool shortPressInteracts = true;
    // [SerializeField] bool longPressInteracts = false;

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
            TryInteract(true);
        else
            TryInteract(false);
    }



    void TryInteract(bool isLongPress)
    {
        if (!cam)
            cam = Camera.main;

        if (!cam)
            return;

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out var hit, distance, mask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (!isLongPress)
            {
                var interactableShort =
                    hit.collider.GetComponent<Interactable>();
                Debug.Log($"InteractableShort: {(interactableShort != null ? interactableShort.GetType().Name : "null")}");
                interactableShort.ExecuteCommand(interactableShort.GetPrimaryCommand());
            }


        }
    }
}
