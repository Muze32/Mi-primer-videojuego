using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float tope;
    private Vector3 posicionInicial;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

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
    }

    public void ResetPosition()
    {
        transform.position = posicionInicial;
    }

    private void UpdateLimits()
    {
        float s = mainCamera.orthographicSize;

        //Valores obtenidos en base a prueba y error
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