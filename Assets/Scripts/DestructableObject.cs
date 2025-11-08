using UnityEngine;


public class DestructableObject : MonoBehaviour
{
    [SerializeField] private float resistance;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > resistance)
        {
            Destroy(gameObject, 0.2f);
        }
        else
        {
            resistance -= collision.relativeVelocity.magnitude;
        }
    }
}
