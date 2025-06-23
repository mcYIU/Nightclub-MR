using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Photo_Grip : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private HandGrabInteractable[] handGrabs;
    [SerializeField] private Interactable interactable;

    public void Grip()
    {
        // Triggers the grip animation and sound if both hands are grabbing and interaction is enabled
        if (CheckHandGrabs() && interactable.isInteractionEnabled)
        {
            animator.SetBool("IsGripped", true);
            SoundEffectManager.PlaySFXOnce(SFX);

            interactable.IncreaseInteractionLevel();
        }
    }

    private bool CheckHandGrabs()
    {
        // Returns true if all hand grab interactors are grabbing the object
        int grabCount = 0;

        for (int i = 0; i < handGrabs.Length; i++)
        {
            if (handGrabs[i].Interactors.Count > 0) grabCount++;
        }

        if (grabCount == handGrabs.Length) return true;
        else return false;
    }
}
