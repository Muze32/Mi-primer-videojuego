using UnityEngine;
using UnityEngine.SceneManagement;

public class LanzarPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float fuerzaLanzamiento = 300f;
    [SerializeField] private float maxDistance;
    private Camera main;
    private FinNivel finNivel;
    private Vector2 startPosition, clampedPosition;

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
        //Se auto-desactiva para que QueueManager lo pueda activar a futuro para usar OnEnable()
        this.enabled = false;
    }
    private void OnMouseDrag()
    {
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
            
            transform.position = clampedPosition;
        }
    }

    private void OnMouseUp()
    {
        //Rb.Dynamic para que el objeto responda a las fisicas de unity
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - clampedPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento);
        //Se comprueba la logica de manejar el final 2 segs despues del lanzamiento
        Invoke("llamarManejarFinal", 2f); //TODO: Llamar la funcion despues de que el personaje se haya detenido (como en el juego original) no despues de x segundos
    }

    private void llamarManejarFinal()
    {
        finNivel.manejarFinal();
    }

    public void actualizarReferencias(Camera cam, FinNivel finalNivel)
    {
        main = cam;
        finNivel = finalNivel;
    }
}
