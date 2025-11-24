using UnityEngine;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float fuerzaLanzamiento = 300f;
    [SerializeField] private float maxDistance;
    private Camera main;
    private FinNivel finNivel;
    private Vector2 startPosition, clampedPosition;
    private CharacterStatus characterStatus;
    [SerializeField] private float maxVelocity = 50f;

    [Header("Efectos de sonido")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioSource characterSound;

    //Orden de prioridad: Awake, OnEnable, Start
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    void Start()
    {
        characterStatus = GetComponent<CharacterStatus>();
        //Se auto-desactiva para que QueueManager lo pueda activar a futuro para usar OnEnable()
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        // Solo aplicar el límite si el personaje ya fue lanzado (y no es kinematic)
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            // Obtener la magnitud (velocidad escalar) actual
            float speed = rb.linearVelocity.magnitude;

            // Si la velocidad supera el límite
            if (speed > maxVelocity)
            {
                // Limitar la velocidad
                rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.isGameActive)
        {
            return; // Bloquea la interacción si el nivel ha terminado.
        }

        soundManager.playHold();
    }

    private void OnMouseDrag()
    {
        if (!GameManager.isGameActive)
        {
            return; // Bloquea la interacción si el nivel ha terminado.
        }

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
            
            //Limita el arrastre para que unicamente se pueda disparar hacia delante
            if(dragPosition.x > startPosition.x)
            {
                clampedPosition.x = startPosition.x;
            }

            transform.position = clampedPosition;
        }
    }

    private void OnMouseUp()
    {
        if (!GameManager.isGameActive)
        {
            return; // Bloquea la interacción si el nivel ha terminado.
        }

        soundManager.playLaunchSounds(characterSound);
        finNivel.CancelInvoke("CheckearVictoria");
        characterStatus.ChangeStatus("air");
        //Rb.Dynamic para que el objeto responda a las fisicas de unity
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento);
        //Se comprueba la logica de manejar el final 3 segs despues del lanzamiento
        Invoke("llamarManejarFinal", 3.5f);
    }

    public void llamarManejarFinal()
    {        
        finNivel.ManejarFinal();
    }

    public void actualizarReferencias(Camera cam, FinNivel finalNivel)
    {
        main = cam;
        finNivel = finalNivel;
    }
}
