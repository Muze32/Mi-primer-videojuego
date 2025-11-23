using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip nextLevelMusic;

    private void Start()
    {
        ChangeMusic(bgMusic);
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
