using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform jugador;
    [SerializeField] private float tope;
    private Vector3 posicionInicial;
    void Start()
    {
        posicionInicial = transform.position;
    }

    private void LateUpdate()
    {
        if (jugador.position.x > transform.position.x && jugador.position.x < tope)
        {
            transform.position = new Vector3(jugador.position.x, transform.position.y, transform.position.z);
        }
    }

    public void resetPosition()
    {
        transform.position = posicionInicial;
    }
}