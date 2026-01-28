using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float distance = 2.2f;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] LayerMask mask = ~0; // ïKóvÇ»ÇÁ Door ÉåÉCÉÑÇæÇØÇ…çiÇÈ

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void OnEnable()
    {
        var action = interactAction != null ? interactAction.action : null;
        if (action == null)
            return;

        action.performed += OnInteract;
        action.Enable();
    }

    void OnDisable()
    {
        var action = interactAction != null ? interactAction.action : null;
        if (action == null)
            return;

        action.performed -= OnInteract;
        action.Disable();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (!cam)
            cam = Camera.main;

        if (!cam)
            return;

        //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, distance, mask))
        //{
        //    var it = hit.collider.GetComponentInParent<DoorInteractable>();
        //    if (it)
        //    {
        //        it.Interact();
        //    }
        //}

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out var hit2, 3f))
        {
            Debug.Log("Hit: " + hit2.collider.name);
            hit2.transform.GetComponentInParent<DoorInteractable>()?.Interact();
        }

    }
}





