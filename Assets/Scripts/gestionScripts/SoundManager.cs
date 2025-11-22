using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource holdHondaSfx;
    [SerializeField] private AudioSource releaseHondaSfx;
    private AudioSource characterSfx;

    public void playHold()
    {
        if (holdHondaSfx != null)
        {
            holdHondaSfx.Play();
        } 
    }
    public void playLaunchSounds(AudioSource characterSfx)
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
}
