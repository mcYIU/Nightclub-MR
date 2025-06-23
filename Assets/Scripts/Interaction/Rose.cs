using UnityEngine;

public class Rose : MonoBehaviour
{
    [SerializeField] private Rose_Pluck[] rosePieces;
    [SerializeField] private Interactable interactable;

    private int availablePluck;
    private MeshRenderer mesh;

    private void Start()
    {
        // Initializes the rose mesh and sets all rose pieces inactive
        mesh = GetComponent<MeshRenderer>();
        availablePluck = rosePieces.Length;

        SetRosePiece(false);
    }

    public void Pluck()
    {
        // Decreases the available pluck count and hides the rose when all pieces are plucked
        availablePluck--;

        if (availablePluck == 0)
        {
            if (mesh != null) mesh.enabled = false;

            interactable.IncreaseInteractionLevel();
        }
    }

    public void SetRoseInteraction(bool isActive)
    {
        // Activates or deactivates all rose pieces if interaction is enabled
        if (interactable.isInteractionEnabled)
        {
            SetRosePiece(isActive);
        }
    }

    private void SetRosePiece(bool isActive)
    {
        // Sets the active state of all rose pieces
        foreach (var _piece in rosePieces)
        {
            _piece.gameObject.SetActive(isActive);
        }
    }
}
