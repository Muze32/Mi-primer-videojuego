using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    private int personajesRestantes;
    private int enemigosRestantes;
    private GameObject personajeActual;
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraFollowing cameraFollowing;
    public void actualizarPersonaje(GameObject personaje)
    {
        personajeActual = personaje;
    }
    public void manejarFinal()
    {
        //FIXME: Por alguna extraña razon, despues de agregar este script hay un ligero trabon cuando el primer personaje sube a la honda
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;
        personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length - 1;

        if (enemigosRestantes == 0)
        {
            Debug.Log("Felicidades. Nivel completado");
            Invoke("resetPosition", 2f);
        }
        else if (personajesRestantes <= 0)
        {
            Debug.Log("No quedan mas personajes. Reiniciando nivel");
            Invoke("resetPosition", 2f);
        }
        else
        {
            Destroy(personajeActual);
            Invoke("avanzarTurno", 1f);
        }
    }

    private void avanzarTurno()
    {
        cameraFollowing.resetPosition();
        queueManager.sortQueue();
    }
    private void resetPosition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
