using System.Collections;
using UnityEngine;

public class FinNivel : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraFollowing cameraFollowing;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MusicManager musicManager;
    private int personajesRestantes;
    private int enemigosRestantes;
    private GameObject personajeActual;
    public Rigidbody2D rb;

    public void ActualizarPersonaje(GameObject personaje)
    {
        personajeActual = personaje;
    }

    public IEnumerator ManejarFinal()
    {
        rb = personajeActual.GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(1f);
        //Cada 1 segundo comprueba si velocidad != 0
        while (rb.linearVelocity.magnitude > 0.01f)
            yield return new WaitForSeconds(1f);

        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemigo").Length;
        personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length - 1;

        //Si el objeto esta quieto sale de la rutina

        if (enemigosRestantes == 0)
        {
            musicManager.PlayNextLevel();
            gameManager.showNextLevelScreen();
            yield break;
        }

        Destroy(personajeActual);

        if (personajesRestantes <= 0)
        {
            yield return new WaitForSeconds(3f);
            manejarTurnoFinal();
        }

        else
        {
            avanzarTurno();
            yield return new WaitForSeconds(0.5f);
            InvokeRepeating("CheckearVictoria", 0.5f, 2f);
            //StartCoroutine(CheckearVictoriaCoroutine());
        }
    }

    private void manejarTurnoFinal()
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
    private IEnumerator CheckearVictoriaCoroutine()
    {
        while (GameObject.FindGameObjectsWithTag("Enemigo").Length > 0)
            yield return new WaitForSeconds(2f);

        musicManager.PlayNextLevel();
        gameManager.showNextLevelScreen();
    }

    public void detenerCheckeo()
    {
        StopCoroutine(CheckearVictoriaCoroutine());
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
        }
    }

    private void avanzarTurno()
    {
        cameraFollowing.resetPosition();
        queueManager.ExecuteNextTurn();
    }
}
