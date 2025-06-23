using UnityEngine;

public class Candle_Blow : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer face;
    [SerializeField] private int[] blendshapeIndex;
    [SerializeField] private float triggerFloat;
    [SerializeField] private AudioClip SFX;
    [SerializeField] Interactable interactable;

    private void OnTriggerStay(Collider other)
    {
        // Checks if the correct face is in the trigger and blendshape weight is sufficient to blow out the candle
        if (interactable.isInteractionEnabled && other.GetComponent<SkinnedMeshRenderer>() == face)
        {
            if (CheckBlendshapeWeight())
            {
                Blow();
            }
        }
    }

    private void Blow()
    {
        // Plays sound, updates interaction, and destroys the candle object
        SoundEffectManager.PlaySFXOnce(SFX);

        interactable.IncreaseInteractionLevel();

        Destroy(gameObject);
    }

    private float GetBlendshapeWeight(int index)
    {
        // Returns the blendshape weight for the given index
        return face.GetBlendShapeWeight(index);
    }

    private bool CheckBlendshapeWeight()
    {
        // Returns true if any blendshape weight exceeds the trigger threshold
        int triggeredIndex = 0;

        foreach (var _index in blendshapeIndex)
        {
            if (GetBlendshapeWeight(_index) > triggerFloat)
                triggeredIndex++;
        }

        return triggeredIndex > 0;
    }
}
