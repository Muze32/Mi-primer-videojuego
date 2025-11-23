using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private float resistance;
    private float resistanceIni;
    private Animator animator;
    private enum estadoMovimiento { highRes, midRes, lowRes, destroyed }; //highRes = 0, midRes = 1, lowRes = 2, destroyed = 3
    [SerializeField] private AudioSource soundBreak;

    [Header("Puntuación")]
    [SerializeField] private int scoreValue = 100;

    private void Start()
    {
        animator = GetComponent<Animator>();
        resistanceIni = resistance;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        estadoMovimiento estado = estadoMovimiento.highRes;
        float velColision = collision.relativeVelocity.magnitude;

        //Destruye el objeto si la velocidad es mayor a su resistencia
        if (velColision > resistance)
        {

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddScore(scoreValue);
            }

            if (soundBreak)
            {
                soundBreak.Play();
            }
            estado = estadoMovimiento.destroyed;
            Destroy(gameObject, .5f);
            animator.SetInteger("estado", (int)estado);
        }
        else
        {
            resistance -= velColision;
            //Cambia los sprites de los objetos segun la resistencia
            if(resistance <= resistanceIni / 3)
            {
                estado = estadoMovimiento.midRes;
            }
            else if(resistance > resistanceIni / 3 && resistance <= resistanceIni * 2/3)
            {
                estado = estadoMovimiento.lowRes;
            }
            animator.SetInteger("estado", (int)estado);
        }
    }
}
