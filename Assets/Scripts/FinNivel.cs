using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinNivel : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraFollowing cameraFollowing;
    private int personajesRestantes;
    private int enemigosRestantes;
    private GameObject personajeActual;

    public void ActualizarPersonaje(GameObject personaje)
    {
        personajeActual = personaje;
    }

    public void ManejarFinal()
    {
        //FIXME: Por alguna extraña razon, despues de agregar este script hay un ligero trabon cuando el primer personaje sube a la honda
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;
        personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length - 1;

        if (enemigosRestantes == 0)
        {
            Debug.Log("Felicidades. Nivel completado");
            Invoke("avanzarNivel", .5f);
        }
        else if (personajesRestantes <= 0)
        {
            Destroy(personajeActual);
            Invoke("esperarTurnoFinal", 5f);
        }
        else
        {
            Destroy(personajeActual);
            Invoke("avanzarTurno", .5f);
            InvokeRepeating("CheckearVictoria", 0.5f, 2f);
        }
    }

    private void esperarTurnoFinal()
    {
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (enemigosRestantes == 0)
        {
            Debug.Log("Felicidades. Nivel completado");
            Invoke("avanzarNivel", .5f);
        }

        Debug.Log("No quedan mas personajes. Reiniciando nivel");
        Invoke("resetPosition", .5f);
    }

    private void CheckearVictoria()
    {
        // Lógica de verificación de victoria (copiada de tu Update())
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (enemigosRestantes == 0)
        {
            Debug.Log("Felicidades. Nivel completado");

            // CRÍTICO: DETENER la repetición del chequeo
            CancelInvoke("CheckearVictoria");

            // Iniciar el avance de nivel
            Invoke("avanzarNivel", 1f);
            this.enabled = false; // Desactiva el script
        }
    }

    private void avanzarTurno()
    {
        cameraFollowing.resetPosition();
        queueManager.ExecuteNextTurn();
    }
    
    private void resetPosition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void avanzarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
