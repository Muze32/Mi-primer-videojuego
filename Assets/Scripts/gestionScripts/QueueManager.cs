using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq; // Necesario para usar el método OrderBy

public class QueueManager : MonoBehaviour
{

    [Header("Dependencias del sistema (Inyección)")]
    [SerializeField] private Transform puntoDeLanzamiento;
    [SerializeField] private CameraFollowing cameraFollowing;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private FinNivel finNivel;

    //Variables referentes a la animacion de movimiento
    private float separationDistance = 5f;
    private float alturaMaximaArco = 1.5f; // Altura del salto en el punto medio
    private float duracionMovimiento = 0.5f; // Tiempo que tarda en llegar a la honda
    private float duracionMovimientoFila = 0.3f;

    private Queue<GameObject> characterQueue;
    void Start()
    {
        createQueue();
        ExecuteNextTurn();
    }

    private void createQueue()
    {
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Personaje");
        //Ordena la lista en base a posicion x de los personajes
        List<GameObject> orderedCharactersList = allCharacters.OrderByDescending(c => c.transform.position.x).ToList();
        characterQueue = new Queue<GameObject>(orderedCharactersList);
    }

    public void ExecuteNextTurn()
    {
        //Maneja el primer elemento de la cola (moviendolo a la honda)
        if (characterQueue.Count > 0)
        {
            GameObject firstCharacter = characterQueue.Dequeue();
            //Agrega elementos necesarios para el personaje
            InjectDependencies(firstCharacter);
            MoverPersonajeALaHonda(firstCharacter);
            ActualizarReferencias(firstCharacter);
        }
        //Maneja el resto de elementos de la cola
        if (characterQueue.Count > 0)
        {
            AvanzarFilaVisualmente();
        }
    }

    //Actualiza la referencia para la camara y el script FinNivel
    private void ActualizarReferencias(GameObject character)
    {
        cameraFollowing.actualizarPersonaje(character);
        finNivel.ActualizarPersonaje(character);
    }

    //Asigna dependencias necesarias para el correcto funcionamiento de LanzarPersonaje
    private void InjectDependencies(GameObject character)
    {
        LanzarPersonaje lanzarScript = character.GetComponent<LanzarPersonaje>();

        if (lanzarScript != null)
        {
            // Asignación de referencias centrales
            lanzarScript.ActualizarReferencias(mainCamera, finNivel);
        }
    }

    private void MoverPersonajeALaHonda(GameObject personaje)
    {
        // Inicia el movimiento en arco
        StartCoroutine(MoverEnArco(personaje, personaje.transform.position, puntoDeLanzamiento.position));
    }

    //Realiza la animacion del movimiento en arco hacia la honda
    private IEnumerator MoverEnArco(GameObject personaje, Vector3 start, Vector3 end)
    {
        float tiempo = duracionMovimiento;
        float altura = alturaMaximaArco;
        float tiempoTranscurrido = 0.0f;

        while (tiempoTranscurrido < tiempo)
        {
            // 1. Calcular el progreso (t) de 0 a 1
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / tiempo;

            // 2. Movimiento Horizontal/Recto (Lineal)
            Vector3 posicionRecta = Vector3.Lerp(start, end, t);

            // 3. Cálculo del Arco (Parábola)
            // La función t * (1 - t) garantiza un arco que empieza en 0, llega a un máximo en 0.5, y vuelve a 0.
            float arco = t * (1.0f - t) * altura;

            // 4. Aplicar la posición Y al arco
            personaje.transform.position = new Vector3(
                posicionRecta.x,
                posicionRecta.y + arco,
                posicionRecta.z
            );

            yield return null; // Espera al siguiente frame
        }

        // Asegura que aterrice exactamente en el punto final
        personaje.transform.position = end;

        LanzarPersonaje lanzarScript = personaje.GetComponent<LanzarPersonaje>();
        if (lanzarScript != null)
        {
            // Esto le da el control al jugador, permitiendo OnMouseDrag y OnMouseUp.
            lanzarScript.enabled = true;
        }
    }

    //Desplaza los personajes de la cola
    private void AvanzarFilaVisualmente()
    {
        // Mueve los personajes restantes de la cola a su nueva posición
        foreach (GameObject character in characterQueue)
        {
            // 1. Calcula la posición de destino: Mueve el personaje 
            //    hacia la derecha por la distancia de separación.
            Vector3 targetPosition = character.transform.position + Vector3.right * separationDistance;

            // 2. Inicia la Corrutina: Mueve suavemente al personaje a la nueva posición.
            StartCoroutine(MoverPersonaje(character, character.transform, targetPosition, duracionMovimientoFila));
        }
    }

    //Desplaza un personaje hacia la derecha
    private IEnumerator MoverPersonaje(GameObject character, Transform characterTransform, Vector3 end, float duration)
    {
        CharacterStatus characterStatus = character.GetComponent<CharacterStatus>();
        characterStatus.ChangeStatus("walking");

        Vector3 start = characterTransform.position;
        float tiempoTranscurrido = 0.0f;

        while (tiempoTranscurrido < duration)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duration;

            // Usa Vector3.Lerp para mover linealmente de forma suave
            characterTransform.position = Vector3.Lerp(start, end, t);

            yield return null; // Espera al siguiente frame
        }
        //TODO: cambiar animacion a idle
        characterStatus.ChangeStatus("idle");
        // Asegura que aterrice exactamente en el punto final
        characterTransform.position = end;
    }
}
