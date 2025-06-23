using UnityEngine;

public class Ashtray_Put : MonoBehaviour
{
    [SerializeField] private AudioClip SFX;
    [SerializeField] Interactable interactable;

    private void OnCollisionEnter(Collision collision)
    {
        // Plays sound and updates interaction state if a fractured object (match) is placed in the ashtray
        if (collision.collider.gameObject.CompareTag("Fracture") && 
            interactable.isInteractionEnabled)
        {
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();
        }
    }
}
