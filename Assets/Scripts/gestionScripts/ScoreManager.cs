using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 1. INSTANCIA ESTÁTICA
    // Esto hace que esta instancia de ScoreManager sea accesible globalmente (ej: ScoreManager.instance.AddScore(500);)
    public static ScoreManager instance;
    private int score = 0;
    private float timer;
    public static bool isGameActive = true;

    // Referencia al componente de texto de la UI para mostrar la puntuación
    [SerializeField] private TextMeshProUGUI scoreText;
    [Header("Descuento por Tiempo")]
    [SerializeField] private int timePenalty;
    [SerializeField] private float penaltyInterval;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private void Start()
    {
        UpdateScoreDisplay();
        instance = this;
        timer = penaltyInterval;
    }

    private void Update()
    {
        if(!isGameActive)
        {
            return;
        }

        // 1. Descuenta el tiempo transcurrido
        timer -= Time.deltaTime;

        // 2. Verifica si el intervalo de penalización ha terminado
        if (timer <= 0)
        {
            ApplyTimePenalty();

            // 3. Reinicia el temporizador
            timer = penaltyInterval;
        }
    }

    public void stopTimer()
    {
        isGameActive = false;
        string formattedScore = score.ToString("N0"); // Formato con separadores de miles (ej. 100,000)
        finalScoreText.text = "Final score\n\n" + formattedScore;
    }

    private void ApplyTimePenalty()
    {
        score -= timePenalty;
        score = Mathf.Max(0, score);

        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}