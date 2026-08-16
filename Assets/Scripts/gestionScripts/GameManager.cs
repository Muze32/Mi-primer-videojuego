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
    public static bool isGameActive = true;

    private void OnEnable()
    {
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

        if (juegoPausado)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        menuPausa.SetActive(juegoPausado);
        btnPausa.SetActive(!juegoPausado);
    }
    public void ToggleMute()
    {
        juegoMuteado = !juegoMuteado;

        if (juegoMuteado)
        {
            AudioListener.volume = 0f;
            Debug.Log("Juego Muteado: Volumen = 0");
        }
        else
        {
            AudioListener.volume = 1.0f;
            Debug.Log("Juego Desmuteado: Volumen = 1.0");
        }
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
        isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetLevel()
    {
        Debug.Log("Reiniciando nivel...");
        isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
