using System.Collections;
using UnityEngine;

public class Whiskey_InGlass : MonoBehaviour
{
    [SerializeField] private float fillSpeed;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Interactable interactable;

    private Renderer sphereRenderer;
    private float maxHeight;
    private float currentHeight = 0f;

    private bool isWhiskeyPouring = false;

    private void Start()
    {
        // Initializes the renderer and sets the initial state of the glass
        maxHeight = transform.localScale.y;
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.enabled = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        // Starts filling the glass if the correct particle collides and pouring hasn't finished
        if(other.CompareTag("Whiskey") && !isWhiskeyPouring && currentHeight != maxHeight)
        {
            StartFilling();
        }
    }

    private void StartFilling()
    {
        // Begins the filling coroutine and disables the UI
        isWhiskeyPouring = true;
        interactable.SetUI(!isWhiskeyPouring);

        StartCoroutine(FillWhiskey());
    }

    private void CompleteFilling()
    {
        // Stops all coroutines and updates interaction state after pouring is complete
        isWhiskeyPouring = false;
        StopAllCoroutines();

        interactable.IncreaseInteractionLevel();
    }

    private IEnumerator FillWhiskey()
    {
        // Animates the whiskey fill, plays sound, and completes filling when full
        SoundEffectManager.PlaySFXLoop(SFX);

        while (currentHeight < maxHeight)
        {
            currentHeight += fillSpeed * Time.deltaTime;
            currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);

            sphereRenderer.enabled = true;

            yield return null;
        }

        SoundEffectManager.StopSFXLoop();
        CompleteFilling();
    }
}