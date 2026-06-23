using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform hand;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactDistance = 3f;

    private GameObject heldObject;

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E key pressed");
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    private void TryPickup()
    {
        Debug.Log("Trying to pick up object");
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

        if(heldObject.GetComponent<Wepon>() != null)
        {
            Wepon weapon = heldObject.GetComponent<Wepon>();
            weapon.SetEquipped(true);
        }

        heldObject.transform.SetParent(hand);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
        Debug.Log("Picked up object: " + heldObject.name);
    }

    private void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if(heldObject.GetComponent<Wepon>() != null)
        {
            Wepon weapon = heldObject.GetComponent<Wepon>();
            weapon.SetEquipped(false);
        }


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