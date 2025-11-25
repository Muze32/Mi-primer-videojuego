using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private float tope;
    private Transform personaje;
    private Vector3 posicionInicial;
    private bool shouldFollow = true;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    // --- Configuración de Movimiento ---
    [Header("Movimiento")]
    [Tooltip("Velocidad de desplazamiento de la cámara.")]
    [SerializeField] private float movementSpeed;

    // --- Configuración de Zoom ---
    [Header("Zoom")]
    [Tooltip("El tamaño de cámara más pequeño (más zoom).")]
    [SerializeField] private float minZoomSize; 
    
    [Tooltip("El tamaño de cámara más grande (menos zoom).")]
    [SerializeField] private float maxZoomSize; 
    
    [Tooltip("Rapidez con la que cambia el zoom.")]
    [SerializeField] private float zoomSpeed; 

    private Camera mainCamera;


    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        posicionInicial = transform.position;
    }

    private void Update()
    {
        UpdateLimits();
        HandleMovement();
        HandleZoom();  

        if(shouldFollow && personaje != null)
        {
            if (personaje.position.x > transform.position.x && personaje.position.x < tope)
            {
                //transform.position = new Vector3(personaje.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    public void resetPosition()
    {
        transform.position = posicionInicial;
        shouldFollow = false;
    }
    public void actualizarPersonaje(GameObject nvoPersonaje)
    {
        personaje = nvoPersonaje.transform;
        shouldFollow = true;
    }

    private void UpdateLimits()
    {
        float s = mainCamera.orthographicSize;

        minX = 1.4f * s - 26f;
        maxX = -1.4f * s + 69f;

        minY = 1.2f * s - 22f;
        maxY = -1.2f * s + 58f;
    }

    private void HandleMovement()
    {
        // Obtiene la entrada de teclado (WASD, flechas)
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        float valorX = transform.position.x;
        float valorY = transform.position.y;
        // Bloquear eje X si intenta salir
        if (inputX > 0 && valorX >= maxX)
            inputX = 0;
        else if (inputX < 0 && valorX <= minX)
            inputX = 0;

        // Bloquear eje Y si intenta salir
        if (inputY > 0 && valorY >= maxY)
            inputY = 0;
        else if (inputY < 0 && valorY <= minY)
            inputY = 0;

        Vector3 direction = new Vector3(inputX, inputY, 0);

        // Time.deltaTime garantiza movimiento suave e independiente del framerate
        transform.position += direction * movementSpeed * Time.deltaTime;
    }
 
    private Vector3 GetClampedPosition(Vector3 targetPos)
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        float clampedX = Mathf.Clamp(targetPos.x, minX + halfWidth, maxX - halfWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minY + halfHeight, maxY - halfHeight);

        return new Vector3(clampedX, clampedY, targetPos.z);
    }
    private void HandleZoom()
    {
        // Obtiene la entrada de la rueda del ratón (+ para adelante, - para atrás)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            // Modifica el tamaño ortográfico (menos Size = más Zoom)
            mainCamera.orthographicSize -= scrollInput * zoomSpeed;

            // Limita el valor del Size entre el mínimo y el máximo configurado
            mainCamera.orthographicSize = Mathf.Clamp(
                mainCamera.orthographicSize, 
                minZoomSize, 
                maxZoomSize
            );
        }
    }
}