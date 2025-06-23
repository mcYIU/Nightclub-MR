using UnityEngine;

public class Balloon_Pop : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionVisual;
    [SerializeField] AudioClip SFX;
    [SerializeField] private Interactable interactable;


    public void Poke()
    {
        // Instantiates explosion effect, plays sound, updates interaction, and destroys the balloon
        if (interactable.isInteractionEnabled) 
        {
            Instantiate(explosionVisual, transform.position, transform.rotation);
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();

            Destroy(gameObject);
        }
    }
}
