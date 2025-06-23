using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Match_Fracture : MonoBehaviour
{
    [SerializeField] private GameObject fracturedMatch;
    [SerializeField] private HandGrabInteractable[] interactables;
    [SerializeField] Interactable interactable;

    public void Fracture()
    {
        // Checks if all interactors are grabbing, then fractures the match and disables UI
        if (interactable.isInteractionEnabled)
        {
            int grabCount = 0;

            for (int i = 0; i < interactables.Length; i++)
                if (interactables[i].Interactors.Count > 0)
                    grabCount++;

            if (grabCount == interactables.Length)
            {
                interactable.SetUI(false);

                Instantiate(fracturedMatch, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}