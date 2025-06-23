using UnityEngine;

public class Whiskey_Pour : MonoBehaviour
{ 
    [SerializeField] private ParticleSystem fluidVisual;
    [SerializeField] private float pouringAngle;
    [SerializeField] private Interactable interactable;

    private Quaternion initialRotation;
    private bool isBottleHeld = false;
    private bool isPouring = false;

    private void Start()
    {
        // Stores the initial rotation of the bottle
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // Checks if the bottle is held and tilted enough to pour, and plays or stops the fluid visual accordingly
        if (isBottleHeld && interactable.isInteractionEnabled)
        {
            bool pourCheck = CalculatePourAngle() > pouringAngle;
            if (isPouring != pourCheck)
            {
                isPouring = pourCheck;

                if (isPouring)
                {
                    fluidVisual.Play();
                }
                else
                {
                    fluidVisual.Stop();
                }
            }
        }
        else
        {
            fluidVisual.Stop();
        }
    }

    private float CalculatePourAngle()
    {
        // Calculates the angle between the initial and current rotation of the bottle
        return Quaternion.Angle(initialRotation, transform.localRotation);
    }

    public void ToggleBottle()
    {
        // Toggles the held state of the bottle
        isBottleHeld = !isBottleHeld;
    }
}
