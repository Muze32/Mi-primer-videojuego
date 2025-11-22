using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Personaje"))
        {
            Destroy(gameObject, 0.5f);
        } 
    }
}