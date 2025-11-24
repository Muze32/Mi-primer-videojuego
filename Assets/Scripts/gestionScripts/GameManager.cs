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
    [SerializeField] private ScoreManager scoreManager;

    private bool juegoPausado = false;
    private bool juegoMuteado = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        //TODO: cambiar sprite de boton mute al clickear
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

    public void showGameOverScreen()
    {
        Debug.Log("Nivel Fallido.");
        showMenu(menuGameOver);
    }

    public void showNextLevelScreen()
    {
        Debug.Log("Felicidades. Nivel completado");
        showMenu(menuNextLevel);
    }

    private void showMenu(GameObject menu)
    {
        btnPausa.SetActive(false);
        menu.SetActive(true);
        scoreText.enabled = false;
        scoreManager.stopTimer();
    }
    public void nextLevel()
    {
        Debug.Log("Avanzando al siguiente nivel...");
        ScoreManager.isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void resetLevel()
    {
        Debug.Log("Reiniciando nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void endGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
