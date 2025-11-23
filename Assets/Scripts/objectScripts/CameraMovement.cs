using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // ... (Variables de Movimiento y Zoom) ...

    [Header("Movimiento")]
    [SerializeField] private float movementSpeed = 5f;

    [Header("Límites del Mapa")]
    // Define las coordenadas máximas y mínimas del mundo de juego
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    [Header("Zoom")]
    [SerializeField] private float minZoomSize = 2f;
    // Asegúrate de que este valor sea mayor que el Size inicial de tu cámara (ej. 35f si es 30)
    [SerializeField] private float maxZoomSize = 35f;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        // Opcional: Si quieres que la cámara se centre en el área al inicio, puedes forzar el clamp aquí.
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        // 1. Mover la Cámara
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(inputX, inputY, 0);
        transform.position += direction * movementSpeed * Time.deltaTime;

        // 2. APLICAR LÍMITES (CLAMPING)

        // El tamaño del borde visible de la cámara en unidades de mundo
        float cameraHalfHeight = mainCamera.orthographicSize;
        float cameraHalfWidth = mainCamera.aspect * cameraHalfHeight; // mainCamera.aspect = ancho/alto

        // Limita la posición X, asegurando que el borde de la cámara no pase del límite del mapa
        float clampedX = Mathf.Clamp(
            transform.position.x,
            minX + cameraHalfWidth, // Detiene el centro de la cámara para que el borde izquierdo quede en minX
            maxX - cameraHalfWidth  // Detiene el centro de la cámara para que el borde derecho quede en maxX
        );

        // Limita la posición Y
        float clampedY = Mathf.Clamp(
            transform.position.y,
            minY + cameraHalfHeight, // Detiene el centro de la cámara para que el borde inferior quede en minY
            maxY - cameraHalfHeight  // Detiene el centro de la cámara para que el borde superior quede en maxY
        );

        // Aplica la posición limitada
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            mainCamera.orthographicSize -= scrollInput * zoomSpeed;

            // Limita el valor del Size entre el mínimo y el máximo configurado
            mainCamera.orthographicSize = Mathf.Clamp(
                mainCamera.orthographicSize,
                minZoomSize,
                maxZoomSize
            );

            HandleMovement();
        }
    }
}