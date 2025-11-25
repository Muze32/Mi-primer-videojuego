using UnityEngine;

public class CameraMovement : MonoBehaviour
{
        // --- Configuración de Movimiento ---
    [Header("Movimiento")]
    [Tooltip("Velocidad de desplazamiento de la cámara.")]
    public float movementSpeed;

    // --- Configuración de Zoom ---
    [Header("Zoom")]
    [Tooltip("El tamaño de cámara más pequeño (más zoom).")]
    public float minZoomSize; 
    
    [Tooltip("El tamaño de cámara más grande (menos zoom).")]
    public float maxZoomSize; 
    
    [Tooltip("Rapidez con la que cambia el zoom.")]
    public float zoomSpeed; 

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        // Obtiene la entrada de teclado (WASD, flechas)
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Calcula el vector de dirección y aplica el movimiento
        Vector3 direction = new Vector3(inputX, inputY, 0);
        
        // Time.deltaTime garantiza movimiento suave e independiente del framerate
        transform.position += direction * movementSpeed * Time.deltaTime;
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