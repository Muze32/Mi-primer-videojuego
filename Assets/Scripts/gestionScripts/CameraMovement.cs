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
    }

    //Sigue al personaje durante el vuelo
    private void LateUpdate()
    {
        if (!shouldFollow || isResetting || charTransform == null)
            return;

        if (charTransform.position.x > transform.position.x && transform.position.x < maxX)
            transform.position = new Vector3(charTransform.position.x, transform.position.y, transform.position.z);
    }

    public void StartFollow(Transform charTransform)
    {
        this.charTransform = charTransform;
        shouldFollow = true;
    }

    //Actualiza los limites de la camara en base al zoom de esta
    private void UpdateLimits()
    {
        float s = mainCamera.orthographicSize;

        //Valores obtenidos en base a prueba y error
        minX = 1.4f * s - 26f;
        maxX = -1.4f * s + 69f;

        minY = 1.2f * s - 22f;
        maxY = -1.2f * s + 58f;
    }

    //Maneja el movimiento de la camara segun el teclado
    private void HandleMovement()
    {
        if (shouldFollow || isResetting)
            return;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(inputX, inputY, 0);
        Vector3 nextPosition = transform.position + direction * movementSpeed * Time.deltaTime;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);

        transform.position = nextPosition;
    }

    //Realiza el zoom de la camara mediante la rueda del mouse
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