using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq; // Necesario para usar el método OrderBy

public class CharacterQueueManager : MonoBehaviour
{
    [SerializeField] private float separationDistance = 1f; 
    private Queue<GameObject> characterQueue;
    [SerializeField] private float alturaMaximaArco = 1.5f; // Altura del salto en el punto medio
    [SerializeField] private float duracionMovimiento = 0.5f; // Tiempo que tarda en llegar a la honda
    
    // Asigna el punto de la honda (destino) desde el Inspector.
    [SerializeField] private Transform puntoDeLanzamiento;
    void Start()
    {
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Personaje");
        List<GameObject> orderedCharactersList = allCharacters.OrderBy(c => c.transform.position.x).ToList();
        characterQueue = new Queue<GameObject>(orderedCharactersList);
        sortQueue();
    }
    private void sortQueue()
    {
        if (characterQueue.Count > 0)
        {
            GameObject firstCharacter = characterQueue.Dequeue();
            MoverPersonajeALaHonda(firstCharacter);
        }
    }
    

    public void MoverPersonajeALaHonda(GameObject personaje)
    {
        // Detiene cualquier movimiento anterior (opcional)
        StopAllCoroutines(); 
        
        // Inicia el movimiento en arco
        StartCoroutine(MoverEnArco(personaje, personaje.transform.position, puntoDeLanzamiento.position));
    }

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
    }
}