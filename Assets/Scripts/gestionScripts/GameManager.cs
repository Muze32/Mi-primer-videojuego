using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject btnPausa;
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuGameOver;    
    [SerializeField] private GameObject menuNextLevel;
    [SerializeField] private TextMeshProUGUI scoreText;

    private bool juegoPausado = false;
    private bool juegoMuteado = false;
    public static bool isGameActive;

    private void OnEnable()
    {
        isGameActive = true;
        Time.timeScale = 1;
        GameEvents.OnGameOver += ShowGameOverScreen;
        GameEvents.OnNextLevel += ShowNextLevelScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverScreen;
        GameEvents.OnNextLevel -= ShowNextLevelScreen;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if (juegoPausado)
            {
                juegoPausado = false;
                Time.timeScale = 1;
            }
            ResetLevel();
        }
    }

    public void TogglePause()
    {
        juegoPausado = !juegoPausado; 
        Time.timeScale = juegoPausado ? 0 : 1;

        menuPausa.SetActive(juegoPausado);
        btnPausa.SetActive(!juegoPausado);
    }
    public void ToggleMute()
    {
        juegoMuteado = !juegoMuteado;
        AudioListener.volume = juegoMuteado ? 0 : 1;
    }

    public void ShowGameOverScreen()
    {
        Debug.Log("Nivel Fallido.");
        ShowMenu(menuGameOver);
    }

    public void ShowNextLevelScreen()
    {
        Debug.Log("Felicidades. Nivel completado");
        ShowMenu(menuNextLevel);
    }

    private void ShowMenu(GameObject menu)
    {
        GameEvents.OnPauseEv();
        btnPausa.SetActive(false);
        menu.SetActive(true);
        scoreText.enabled = false;
        isGameActive = false;
    }
    public void NextLevel()
    {
        Debug.Log("Avanzando al siguiente nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetLevel()
    {
        Debug.Log("Reiniciando nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Debug.Log("Volviendo al selector de niveles...");
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
