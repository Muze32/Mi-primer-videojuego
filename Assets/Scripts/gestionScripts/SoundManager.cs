using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip holdHondaSFX;
    [SerializeField] private AudioClip releaseHondaSFX;
    private AudioSource characterSfx;

    [Header("Musica del nivel")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip nextLevelMusic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ChangeMusic(bgMusic);
    }

    public void PlayHold()
    {
        if (holdHondaSFX) {
            sfxSource.clip = holdHondaSFX;
            sfxSource.Play();
        }
    }

    public void PlayLaunchSound(AudioSource characterSfx)
    {
        this.characterSfx = characterSfx;

        sfxSource.Stop();
        sfxSource.clip = releaseHondaSFX;
        sfxSource.Play();

        Invoke("ReproducirLanzamientoSfx", .5f); 
    }

    private void ReproducirLanzamientoSfx()
    {
        if (characterSfx)
            characterSfx.Play();
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
