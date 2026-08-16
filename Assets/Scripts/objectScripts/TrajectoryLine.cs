using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("Longitud de la linea de apuntado")]
    [SerializeField] private int segmentCount = 50; 
    [SerializeField] private float espacioEntrePuntos = 0.05f; 
    [SerializeField] private Transform initialLaunchPoint;
    public static Transform launchPoint;

    private LineRenderer lineRenderer;
    private float speed;
    private Vector2 gravityVector;
    private float drag;
    private Rigidbody2D activeRb;
    private LanzarPersonaje activeLanzarPersonaje;
    private bool isDrawing = false;

    private void OnEnable()
    {
        GameEvents.OnHold += StartDrawing;
        GameEvents.OnLaunch += StopDrawing;
        launchPoint = initialLaunchPoint;
    }

    private void OnDisable()
    {
        GameEvents.OnHold -= StartDrawing;
        GameEvents.OnLaunch -= StopDrawing;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;
        lineRenderer.enabled = false; // Oculta al iniciar
    }

    private void StartDrawing(GameObject obj)
    {
        activeRb = obj.GetComponent<Rigidbody2D>();
        activeLanzarPersonaje = obj.GetComponent<LanzarPersonaje>();
        
        if (activeRb != null && activeLanzarPersonaje != null)
        {
            // Lee la gravedad y el arrastre (damping) del rigidbody
            gravityVector = Physics2D.gravity * activeRb.gravityScale;
            drag = activeRb.linearDamping; 
            speed = activeLanzarPersonaje.FuerzaLanzamiento;
            isDrawing = true;
        }
    }

    private void StopDrawing(GameObject obj)
    {
        isDrawing = false;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        activeRb = null;
        activeLanzarPersonaje = null;
    }

    private void Update()
    {
        if (!isDrawing || activeRb == null || activeLanzarPersonaje == null)
        {
            return;
        }

        lineRenderer.enabled = true;
        
        // 1. El personaje inicia su vuelo desde su posición de arrastre actual (clampedPosition)
        Vector2 pos = activeRb.transform.position; 
        
        // 2. La posición central de la honda (startPosition)
        Vector2 initialPos = activeLanzarPersonaje.StartPosition;
        
        // 3. Calculamos la dirección y velocidad inicial idéntica a LanzarPersonaje.cs
        Vector2 direccionLanzamiento = initialPos - pos;
        Vector2 velocity = (direccionLanzamiento * speed) / activeRb.mass;
        
        // Clampa la velocidad máxima para coincidir con la física
        velocity = Vector2.ClampMagnitude(velocity, activeLanzarPersonaje.MaxVelocity);

        // Simula la física de Unity paso a paso (Integración de Euler)
        float dt = espacioEntrePuntos; 
        for (int i = 0; i < segmentCount; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(pos.x, pos.y, 0f));

            // Simula la gravedad
            velocity += gravityVector * dt;
            // Simula la resistencia del aire (drag / damping)
            velocity *= (1f - drag * dt);
            // Simula el desplazamiento de posición
            pos += velocity * dt;
        }
    }
}
