using UnityEngine;
using UnityEngine.SceneManagement;

public class finDeNivel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private AudioSource finNivelSFX;
    private bool nivelFinalizado = false;
    void Start()
    {
        //finNivelSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Jugador" && !nivelFinalizado)
        {
            nivelFinalizado = true;
            //finNivelSFX.Play();
            Invoke("cambiarNivel", 2f);
        }
    }

    private void cambiarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}