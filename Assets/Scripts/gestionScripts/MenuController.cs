using UnityEngine;
using UnityEngine.UI; // Necesario para usar componentes UI antiguos

public class MenuController : MonoBehaviour
{
    [Header("Botones de la UI")]
    [SerializeField] private Button botonPausa;
    [SerializeField] private Button botonUnPause;
    [SerializeField] private Button botonReiniciar;
    [SerializeField] private Button botonSiguienteNivel;
    [SerializeField] private Button botonMute;

    [SerializeField] private Button[] botonesHome;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        // Asignamos las funciones por código al iniciar el nivel
        if (botonPausa != null)
            botonPausa.onClick.AddListener(() => gameManager.TogglePause());

        if (botonUnPause != null)
            botonUnPause.onClick.AddListener(() => gameManager.TogglePause());

        if (botonReiniciar != null)
            botonReiniciar.onClick.AddListener(() => gameManager.ResetLevel());

        if (botonSiguienteNivel != null)
            botonSiguienteNivel.onClick.AddListener(() => gameManager.NextLevel());

        if (botonMute != null)
            botonMute.onClick.AddListener(() => gameManager.ToggleMute());

        if (botonesHome != null)
        {
            foreach (Button btn in botonesHome)
            {
                if (btn != null)
                {
                    btn.onClick.AddListener(() => gameManager.GoHome());
                }
            }
        }
        
    }
}