using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip holdHondaSFX;
    [SerializeField] private AudioClip releaseHondaSFX;
    private AudioClip characterSfx;

    [Header("Musica del nivel")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip nextLevelMusic;

    private void Start()
    {
        ChangeMusic(bgMusic);
    }

    public void PlayHold()
    {
        if (!holdHondaSFX) return;

        sfxSource.clip = holdHondaSFX;
        sfxSource.Play();
    }

    private void OnEnable() { 
        GameEvents.OnLaunch += PlayLaunchSound;
        GameEvents.OnHold += PlayHold;
    }

    private void OnDisable()
    {
        GameEvents.OnLaunch -= PlayLaunchSound;
        GameEvents.OnHold -= PlayHold;
    }

    private void PlayLaunchSound(GameObject obj)
    {
        this.characterSfx = obj.GetComponent<LanzarPersonaje>().LaunchSound;

        sfxSource.Stop();
        sfxSource.clip = releaseHondaSFX;
        sfxSource.Play();
        PlaySFXOnce(characterSfx);
    }

    public void PlaySFXOnce(AudioClip audioSFX)
    {
        sfxSource.PlayOneShot(audioSFX);
    }


    private void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip) return;

        musicSource.clip = newClip;
        musicSource.Play();
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
