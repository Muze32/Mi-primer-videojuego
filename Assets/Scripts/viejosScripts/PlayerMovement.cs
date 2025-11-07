using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float velocidad = 7f; 
    [SerializeField] private float fuerzaSalto = 10f;
    private float dirX;
    private SpriteRenderer spriteR;
    private enum estadoMovimiento { idle, running, jumping, falling };
    private BoxCollider2D boxColl;
    [SerializeField] private LayerMask elPiso;
    [SerializeField] private AudioSource saltoSfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(dirX * velocidad, rb.linearVelocityY);

        if (Input.GetButtonDown("Jump") && enElPiso()) {
            saltoSfx.Play();
            rb.linearVelocity = new Vector2(rb.linearVelocityX, fuerzaSalto);
        }
        updateMovement();
    }

    private void updateMovement()
    {
        estadoMovimiento estado;
        if (dirX != 0)
        {
            estado = estadoMovimiento.running;
            if (dirX > 0f) //Si se mueve a la derecha
            {
                spriteR.flipX = false;
            }
            else //Si se mueve a la izquierda
            {
                spriteR.flipX = true;
            }
        }
        else //Si esta quieto
        {
            estado = estadoMovimiento.idle;
        }

        if(rb.linearVelocityY > 0.1f) //Si esta saltando
        {
            estado = estadoMovimiento.jumping;
        }
        else if(rb.linearVelocityY < -0.1f) //Si el jugador esta cayendo
        {
            estado = estadoMovimiento.falling;
        }
        animator.SetInteger("estado", (int)estado);
    }

    private bool enElPiso() //Devuelve true si el player esta en el piso
    {
        return Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, 0.1f, elPiso);
    }
}