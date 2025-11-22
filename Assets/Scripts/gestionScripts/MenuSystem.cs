using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void avanzarNivel()
    {
        Debug.Log("Avanzando al siguiente nivel");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void terminarJuego()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
