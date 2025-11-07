using UnityEngine;

public class PermanecerEnPlataforma : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.transform.SetParent(null);
    }
}