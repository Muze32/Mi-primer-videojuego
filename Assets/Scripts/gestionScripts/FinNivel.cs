using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraFollowing cameraFollowing;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MusicManager musicManager;
    private int personajesRestantes;
    private int enemigosRestantes;
    private GameObject personajeActual;

    public void ActualizarPersonaje(GameObject personaje)
    {
        personajeActual = personaje;
    }

    public void ManejarFinal()
    {
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;
        personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length - 1;

        if (enemigosRestantes == 0)
        {
            musicManager.PlayNextLevel();
            gameManager.showNextLevelScreen();
        }
        else if (personajesRestantes <= 0)
        {
            Destroy(personajeActual);
            Invoke("esperarTurnoFinal", 3f);
        }
        else
        {
            Destroy(personajeActual);
            Invoke("avanzarTurno", 0.5f);
            InvokeRepeating("CheckearVictoria", 0.5f, 2f);
        }
    }

    private void esperarTurnoFinal()
    {
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (enemigosRestantes == 0)
        {
            musicManager.PlayNextLevel();
            gameManager.showNextLevelScreen();
        }
        else
        {
            musicManager.PlayGameOver();
            gameManager.showGameOverScreen();
        }
    }

    private void CheckearVictoria()
    {
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (enemigosRestantes == 0)
        {
            //Detiene los checkeos
            CancelInvoke("CheckearVictoria");
            // Iniciar el avance de nivel
            musicManager.PlayNextLevel();
            gameManager.showNextLevelScreen();

            //TODO: comprobar si se puede eliminar la linea de abajo
            this.enabled = false; // Desactiva el script
        }
    }

    private void avanzarTurno()
    {
        cameraFollowing.resetPosition();
        queueManager.ExecuteNextTurn();
    }
}
