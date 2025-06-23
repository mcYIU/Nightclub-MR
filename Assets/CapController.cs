using UnityEngine;

public class CapController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;

    void FixedUpdate()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.up);
    }
}
