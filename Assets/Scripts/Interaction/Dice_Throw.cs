using UnityEngine;

public class Dice_Throw : MonoBehaviour
{
    public static bool isGrounded = true;
    [SerializeField] private Interactable interactable;
    [SerializeField] private AudioClip[] SFX;
    [SerializeField] private float throwForce;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checks if the dice collides with the environment and triggers the throw logic
        if (collision.collider.gameObject.CompareTag("Environment") && !isGrounded)
        {
            ThrowOnTable();
        }
    }

    private void ThrowOnTable()
    {
        // Plays a random sound and updates interaction when the dice lands on the table
        int soundIndex = Random.Range(0, SFX.Length);
        SoundEffectManager.PlaySFXOnce(SFX[soundIndex]);

        interactable.IncreaseInteractionLevel();
    }

    public void GrabAllDices(Transform parentDice)
    {
        // Prepares the dice for grabbing, disables physics, and attaches it to the parent
        if (interactable.isInteractionEnabled)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;

            transform.SetPositionAndRotation(parentDice.position, parentDice.rotation);
            transform.SetParent(parentDice.transform);
            transform.localScale = Vector3.one;

            isGrounded = false;

            SetUI(isGrounded);
        }
    }

    public void ReleaseAllDices()
    {
        // Releases the dice, enables physics, and applies a throw force
        if (interactable.isInteractionEnabled)
        {
            transform.SetParent(null);
            transform.localScale = Vector3.one;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }
    }

    private void SetUI(bool isGrabbed)
    {
        // Updates the UI based on whether the dice is grabbed
        interactable.SetUI(isGrabbed);
    }
}
