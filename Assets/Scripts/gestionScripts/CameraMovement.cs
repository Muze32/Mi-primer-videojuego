using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
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
    private Vector3 posicionInicial;
    private float minX, maxX, minY, maxY;
    private bool shouldFollow = false, isResetting = false;
    private Transform charTransform;
    [SerializeField] LanzarPersonaje lanzarPersonaje;

    [Header("Límites Dinámicos")]
    [Tooltip("Arrastra aquí el BoxCollider2D que creaste para delimitar el mapa.")]
    [SerializeField] private Collider2D mapBounds;

    private void OnEnable()
    {
        GameEvents.OnNextTurn += ResetPosition;
        GameEvents.OnLaunch += StartFollow;
    }

    private void OnDisable()
    {
        GameEvents.OnNextTurn -= ResetPosition;
        GameEvents.OnLaunch -= StartFollow;
    }

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        posicionInicial = transform.position;
    }

    //Maneja los movimientos principales de la camara
    private void Update()
    {
        UpdateLimits();
        HandleMovement();
        HandleZoom();
        ClampCameraPosition();
    }

    //Sigue al personaje durante el vuelo
    private void LateUpdate()
    {
        if (!shouldFollow || isResetting || !charTransform) return;

        if (charTransform.position.x > transform.position.x && transform.position.x < maxX)
        {
            transform.position = new Vector3(charTransform.position.x, transform.position.y, transform.position.z);
        }
        ClampCameraPosition();
    }

    public void StartFollow(GameObject obj)
    {
        this.charTransform = obj.transform;
        shouldFollow = true;
    }

    //Actualiza los limites de la camara en base al zoom de esta
    private void UpdateLimits()
    {
        if (mapBounds == null) return;

        float s = mainCamera.orthographicSize;
        float halfWidth = s * mainCamera.aspect;

        Bounds bounds = mapBounds.bounds;

        // Calculamos los bordes internos basados en el tamaño de la pantalla
        minX = bounds.min.x + halfWidth;
        maxX = bounds.max.x - halfWidth;
        minY = bounds.min.y + s;
        maxY = bounds.max.y - s;

        // Si la pantalla es más grande que el mapa, nos centramos en él
        if (minX > maxX) minX = maxX = bounds.center.x;
        if (minY > maxY) minY = maxY = bounds.center.y;
    }

    //Maneja el movimiento de la camara segun el teclado
    private void HandleMovement()
    {
        if (shouldFollow || isResetting) return;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(inputX, inputY, 0);
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    private void ClampCameraPosition()
    {
        if (mapBounds == null) return;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    //Realiza el zoom de la camara mediante la rueda del mouse
    private void HandleZoom()
    {
        // Obtiene la entrada de la rueda del ratón (+ para adelante, - para atrás)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput == 0f) return;

        // Modifica el tamaño ortográfico (menos Size = más Zoom)
        mainCamera.orthographicSize -= scrollInput * zoomSpeed;

        // Limita el valor del Size entre el mínimo y el máximo configurado
        mainCamera.orthographicSize = Mathf.Clamp(
            mainCamera.orthographicSize,
            minZoomSize,
            maxZoomSize
        );
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        shouldFollow = false;
        StartCoroutine(MoverSuave(posicionInicial, 0.5f));
    }

    private IEnumerator MoverSuave(Vector3 destino, float duracion)
    {
        isResetting = true;
        Vector3 inicio = transform.position;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            transform.position = Vector3.Lerp(inicio, destino, t);
            yield return null;
        }

        transform.position = destino;
        isResetting = false;
    }

}