using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform hand;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactDistance = 3f;

    [Header("Weapon Controller")]
    [SerializeField] private Wepon weponController;

    private GameObject heldObject;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    private void TryPickup()
    {
        if (!Physics.Raycast(
                playerCamera.transform.position,
                playerCamera.transform.forward,
                out RaycastHit hit,
                interactDistance,
                interactableLayer))
        {
            return;
        }

        heldObject = hit.collider.gameObject;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        heldObject.transform.SetParent(hand);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        Gun gun = heldObject.GetComponent<Gun>();

        if (gun != null && weponController != null)
        {
            weponController.SetCurrentGun(gun);
        }

        Debug.Log("Picked up object: " + heldObject.name);
    }

    private void DropObject()
    {
        Gun gun = heldObject.GetComponent<Gun>();

        if (gun != null && weponController != null)
        {
            weponController.SetCurrentGun(null);
        }

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        heldObject.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        heldObject = null;

        Debug.Log("Dropped object");
    }
}