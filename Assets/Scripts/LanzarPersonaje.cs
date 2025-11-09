using UnityEngine;
using UnityEngine.SceneManagement;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float fuerzaLanzamiento = 300f;
    [SerializeField] private float maxDistance;
    [SerializeField] private Camera main;
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraFollowing cameraFollowing;
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
        //Se auto-desactiva para que QueueManager lo pueda activar a futuro para usar OnEnable()
        this.enabled = false;
    }
    private void OnEnable()
    {
        //Cuando se activa el script se cambia RB a kinematic (se activa despues de mover visualmente los personajes)
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
        //Rb.Dynamic para que el objeto responda a las fisicas de unity
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento);

        //Si no hay mas personajes reinicia el nivel
        if (personajesRestantes <= 0)
        {
            Invoke("resetPosition", 3.5f);
        }

        //Destruye el personaje despues de cuatro segundos y avanza la cola
        Invoke("handleNextCharacter", 4f);
    }

    private void handleNextCharacter()
    {
        Destroy(gameObject, .5f);
        cameraFollowing.resetPosition();
        queueManager.sortQueue();
    }

    private void resetPosition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
