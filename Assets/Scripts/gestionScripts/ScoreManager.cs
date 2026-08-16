using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 1. INSTANCIA ESTATICA
    // Esto hace que esta instancia de ScoreManager sea accesible globalmente (ej: ScoreManager.instance.AddScore(500);)
    public static ScoreManager instance;
    private int score = 0;
    private float timer;

    // Referencia al componente de texto de la UI para mostrar la puntuaci�n
    [Header("Descuento por Tiempo")]
    [SerializeField] private int timePenalty;
    [SerializeField] private float penaltyInterval;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private TextMeshProUGUI currentScoreText;

    private void OnEnable()
    {
        GameEvents.OnPause += StopTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnPause -= StopTimer;
    }

    private void Start()
    {
        instance = this;
        timer = penaltyInterval;
        currentScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!GameManager.isGameActive) return;

        // 1. Descuenta el tiempo transcurrido
        timer -= Time.deltaTime;

        // 2. Verifica si el intervalo de penalizacion ha terminado
        if (timer <= 0)
        {
            ApplyTimePenalty();
            // 3. Reinicia el temporizador
            timer = penaltyInterval;
        }
        UpdateScoreDisplay();
    }

    private void StopTimer()
    {
        string formattedScore = score.ToString("N0"); // Formato con separadores de miles (ej. 100,000)
        finalScoreText.text = "Final score\n\n" + formattedScore;
    }

    private void ApplyTimePenalty()
    {
        score -= timePenalty;
        score = Mathf.Max(0, score);
    }

    public void AddScore(int points)
    {
        score += points;
    }

    private void UpdateScoreDisplay()
    {
        currentScoreText.text = "Score: " + score.ToString();
    }
}