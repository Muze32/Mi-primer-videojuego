using System;
using System.Collections;
using UnityEngine;

public class FinNivel : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    private GameObject personajeActual;
    private Coroutine victoriaCoroutine;

    private void OnEnable() => GameEvents.OnLaunch += OnLaunch;
    private void OnDisable() => GameEvents.OnLaunch -= OnLaunch;

    private void OnLaunch(GameObject _)
    {
        DetenerCheckeo();
        StartCoroutine(ManejarFinal());
    }

    private void DetenerCheckeo()
    {
        if (victoriaCoroutine == null) return;

        StopCoroutine(victoriaCoroutine);
        victoriaCoroutine = null;
    }
    private IEnumerator ManejarFinal()
    {
        Rigidbody2D rb = personajeActual.GetComponent<Rigidbody2D>();

        //Espera 1 segundo para que no tome en cuenta el momento del lanzamiento
        yield return new WaitForSeconds(1f);

        //Espera hasta que el personaje este quieto
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.01f);

        //Si el personaje esta quieto comprueba los posibles escenarios

        if (NoHayEnemigos())
        {
            AvanzarNivel();
            yield break;
        }

        int personajesRestantes = GameObject.FindGameObjectsWithTag("Personaje").Length - 1;

        if (personajesRestantes <= 0)
        {
            yield return new WaitForSeconds(3f);
            ManejarTurnoFinal();
        }

        else
        {
            Destroy(personajeActual);
            AvanzarTurno();
            victoriaCoroutine = StartCoroutine(CheckearVictoriaCoroutine());
        }
    }

    private void ManejarTurnoFinal()
    {
        Destroy(personajeActual);

        if (NoHayEnemigos())
            AvanzarNivel();
        else
            EjecutarGameOver();
    }

    private IEnumerator CheckearVictoriaCoroutine()
    {
        while (GameObject.FindGameObjectsWithTag("Enemigo").Length > 0)
            yield return new WaitForSeconds(1f);

        AvanzarNivel();
    }

    private void AvanzarTurno()
    {
        cameraMovement.ResetPosition();
        queueManager.ExecuteNextTurn();
    }

    private void AvanzarNivel()
    {
        soundManager.PlayNextLevel();
        gameManager.showNextLevelScreen();
    }

    private void EjecutarGameOver()
    {
        soundManager.PlayGameOver();
        gameManager.showGameOverScreen();
    }

    private bool NoHayEnemigos()
    {
        return GameObject.FindGameObjectsWithTag("Enemigo").Length == 0;
    }

    public void ActualizarPersonaje(GameObject personaje)
    {
        personajeActual = personaje;
    }
}
