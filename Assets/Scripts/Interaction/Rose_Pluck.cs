using UnityEngine;

public class Rose_Pluck : MonoBehaviour
{
    [SerializeField] Transform detechPoint;
    [SerializeField] float detachDistance;
    [SerializeField] AudioClip SFX;
    [SerializeField] Rose rose;
    [SerializeField] Interactable interactable;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Pluck()
    {
        interactable.SetUI(false);

        // Checks if the piece is plucked and interaction is enabled, then detaches the piece
        if (IsPlucked() && interactable.isInteractionEnabled)
        {
            PickOutFromParent();
        }
    }

    private bool IsPlucked()
    {
        // Returns true if the piece is within the detach distance from the detect point
        return Vector3.Distance(transform.position, detechPoint.position) < detachDistance;
    }

    private void PickOutFromParent()
    {
        // Plays sound, detaches the piece, enables physics, and notifies the rose
        SoundEffectManager.PlaySFXOnce(SFX);

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        rose.Pluck();
    }
}
