using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private Sprite resistanceMedium;
    [SerializeField] private Sprite resistanceLow;
    [SerializeField] private float resistance;
    private float resistanceIni;
    private SpriteRenderer spriteR;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        resistanceIni = resistance;
        spriteR = GetComponent<SpriteRenderer>();
        animator.enabled = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float velColision = collision.relativeVelocity.magnitude;

        //Destruye el objeto si la velocidad es mayor a su resistencia
        if (velColision > resistance)
        {
            animator.enabled = true;
            animator.SetTrigger("dead");
            Destroy(gameObject, .2f);
        }
        else
        {
            resistance -= velColision;
            //Cambia los sprites de los objetos segun la resistencia
            if(resistance <= resistanceIni / 3)
            {
                spriteR.sprite = resistanceLow;
            } else if(resistance > resistanceIni / 3 && resistance <= resistanceIni * 2/3)
            {
                spriteR.sprite = resistanceMedium;
            }
        }
    }
}
