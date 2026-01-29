using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource holdHondaSfx;
    [SerializeField] private AudioSource releaseHondaSfx;
    private AudioSource characterSfx;

    [Header("Musica del nivel")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip nextLevelMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // opcional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeMusic(bgMusic);
    }

    public void PlayHold()
    {
        if (holdHondaSfx != null)
        {
            holdHondaSfx.Play();
        } 
    }
    public void PlayLaunchSound(AudioSource characterSfx)
    {
        this.characterSfx = characterSfx;
        //Detener el sonido de "hold" antes de lanzar
        if (holdHondaSfx.isPlaying)
        {
            holdHondaSfx.Stop();
        }

        releaseHondaSfx.Play();
        Invoke("ReproducirLanzamientoSfx", .5f); 
    }

    private void ReproducirLanzamientoSfx()
    {
        if (characterSfx != null)
        {
            characterSfx.Play();
        }
    }

    private void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip != newClip)
        {
            musicSource.clip = newClip;
            musicSource.Play();
        }
    }

    public void PlayNextLevel()
    {
        ChangeMusic(nextLevelMusic);
    }

    public void PlayGameOver()
    {
        ChangeMusic(gameOverMusic);
    }
}
