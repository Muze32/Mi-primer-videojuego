using System;
using UnityEngine;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPosition, clampedPosition;

    //Parametros de lanzamiento
    [Range(0f, 500f)]
    [SerializeField] private float fuerzaLanzamiento = 10f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float maxVelocity = 50f;

    [Header("Efectos de sonido")]
    //SerializeField] private AudioSource characterSound;
    [SerializeField] private AudioClip launchSound;
    public AudioClip LaunchSound => launchSound; // solo lectura
    private Camera mainCamera;

    //Orden de prioridad: Awake, OnEnable, Start
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (!rb) return;

        //Cuando se activa el script se cambia RB a kinematic (se activa despues de mover visualmente los personajes)
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = transform.position;
        clampedPosition = startPosition; // ✅ inicializa aquí
    }
    void Start()
    {
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

            // Limita la velocidad si supera el limite
            if (speed > maxVelocity)
            {
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.isGameActive) return; // Bloquea la interacción si el nivel ha terminado.
        GameEvents.OnHoldEv();
    }

    private void OnMouseDrag()
    {
        if (!GameManager.isGameActive || !mainCamera) return; // Bloquea la interacción si el nivel ha terminado o no hay camara.
        
        Vector2 dragPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        clampedPosition = dragPosition;

        float dragDistance = Vector2.Distance(startPosition, dragPosition);

        //Limita la distancia de arrastre maxima para los personajes
        if (dragDistance > maxDistance)
        {
            //Suma la pos inicial + un vector con tamaño equivalente a maxDistance
            clampedPosition = startPosition + (dragPosition - startPosition).normalized * maxDistance; 
        }
            
        //Limita el arrastre para que unicamente se pueda disparar hacia delante
        if (dragPosition.x > startPosition.x)
        {
            clampedPosition.x = startPosition.x;
        }

        transform.position = clampedPosition;
    }

    private void OnMouseUp()
    {
        if (!GameManager.isGameActive) return; // Bloquea la interacción si el nivel ha terminado.

        GameEvents.OnLaunchEv(gameObject);
        
        //Rb.Dynamic para que el objeto responda a las fisicas de unity
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);
    }

    public void UpdateCamera(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }
}
