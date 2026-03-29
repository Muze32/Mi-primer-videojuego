using System;
using UnityEngine;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPosition, clampedPosition;
    private CharacterStatus characterStatus;

    //Parametros de lanzamiento
    [SerializeField] private float fuerzaLanzamiento = 300f;
    [SerializeField] private float maxDistance;
    [SerializeField] private float maxVelocity = 50f;
    public event Action OnLaunch;

    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource characterSound;

    private Camera mainCamera;
    private FinNivel finNivel;
    private SoundManager soundManager;
    private CameraMovement cameraMovement;

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

            // Limita la velocidad si supera el limite
            if (speed > maxVelocity)
            {
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.isGameActive)
        {
            return; // Bloquea la interacción si el nivel ha terminado.
        }

        soundManager.PlayHold();
    }

    private void OnMouseDrag()
    {
        if (!GameManager.isGameActive)
        {
            return; // Bloquea la interacción si el nivel ha terminado.
        }

        if (mainCamera != null)
        {
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

        OnLaunch?.Invoke();

        cameraMovement.StartFollow(transform);
        soundManager.PlayLaunchSound(characterSound);
        finNivel.DetenerCheckeo();
        characterStatus.ChangeStatus("air");
        
        //Rb.Dynamic para que el objeto responda a las fisicas de unity
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento, ForceMode2D.Impulse);

        //Se comprueba la logica para manejar el final de cada turno
        StartCoroutine(finNivel.ManejarFinal());
    }

    public void ActualizarReferencias(Camera mainCamera, FinNivel finNivel, CameraMovement cameraMovement)
    {
        soundManager = SoundManager.Instance;
        this.mainCamera = mainCamera;
        this.finNivel = finNivel;
        this.cameraMovement = cameraMovement;
    }
}
