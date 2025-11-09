using UnityEngine;
using UnityEngine.SceneManagement;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float fuerzaLanzamiento = 300f;
    [SerializeField] private float maxDistance;
    [SerializeField] private Camera main;
    private Vector2 startPosition, clampedPosition;
    private float startRotation;
    private static int personajesRestantes;

    private void Awake()
    {
        // Solo inicializar si es la primera vez que se accede a esta clase en la escena
        if (personajesRestantes == 0)
        {
            personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length;
        }
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        startPosition = transform.position;
        startRotation = rb.rotation;
        this.enabled = false;    
    }
    private void OnEnable()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            startPosition = transform.position;

        }    
    }
    private void OnMouseDrag()
    {
        if (main != null)
        {
            Vector2 dragPosition = main.ScreenToWorldPoint(Input.mousePosition);
            clampedPosition = dragPosition;

            float dragDistance = Vector2.Distance(startPosition, dragPosition);

            //Limita la distancia de arrastre maxima para los personajes
            if (dragDistance > maxDistance)
            {
                clampedPosition = startPosition + (dragPosition - startPosition).normalized * maxDistance; //Suma la pos inicial + un vector con tamaño equivalente a maxDistance
            }

            transform.position = clampedPosition;
        }
    }

    private void OnMouseUp()
    {
        personajesRestantes -= 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento);

        //Destruye el personaje despues de dos segundos
        Destroy(gameObject, 4f);

        if (personajesRestantes <= 0)
        {
            Invoke("resetPosition", 3.5f);
        }
    }

    private void resetPosition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}