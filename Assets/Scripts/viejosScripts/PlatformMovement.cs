using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] puntosDeReferencia;
    [SerializeField] private float velocidad = 2f;
    private int puntoActual = 0;


    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(puntosDeReferencia[puntoActual].transform.position, transform.position) < 0.1f)
        {
            puntoActual++;
            if (puntoActual == puntosDeReferencia.Length)
            {
                puntoActual = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, puntosDeReferencia[puntoActual].transform.position, velocidad * Time.deltaTime);
    }
}