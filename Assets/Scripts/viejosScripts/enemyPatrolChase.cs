using UnityEngine;

public class enemyPatrolChase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform player;
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float velocidadPatrullaje = 1f;
    [SerializeField] private float velocidadPersecucion = 2f;
    [SerializeField] private float rangoDeteccion = 6f;
    [SerializeField] private float rangoAtaque = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 puntoDestino;
    private bool persiguiendo = false;
    private bool atacando = false;
    void Start()
    {
        puntoDestino = puntoB.position;
        persiguiendo = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaAlJugador <= rangoAtaque)
        {
            //Ataca al player
            persiguiendo = false;
            atacando = true;
        }

        else if (distanciaAlJugador <= rangoDeteccion)
        {
            //Persigue al player
            persiguiendo = true;
            atacando = false;
        }

        else
        {
            persiguiendo = false;
            atacando = false;
        }

        if (atacando)
        {
            atacar();
        } 
        else if (persiguiendo)
        {
            perseguir();
        }
        else
        {
            patrullar();
        }

        animator.SetBool("patrullando", !persiguiendo && !atacando);
        animator.SetBool("persiguiendo", persiguiendo);
        animator.SetBool("atacando", atacando);
    }

    private void perseguir()
    {
        //Si no se usa rigidBody2D en el enemigo
        //Vector2 playerPositionx = new Vector2(player.position.y, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, player.position, velocidadPersecucion * Time.deltaTime);

        if(transform.position.x < player.position.x && spriteRenderer.flipX || transform.position.x > player.position.x && !spriteRenderer.flipX)
        {
            voltear();
        }

    }

    private void patrullar()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoDestino, velocidadPatrullaje * Time.deltaTime);
        if (Vector2.Distance(transform.position, puntoDestino) < 0.1f)
        {
            if (puntoDestino == puntoA.position)
            {
                puntoDestino = puntoB.position;
            }
            else
            {
                puntoDestino = puntoA.position;
            }
        }

        if (transform.position.x < puntoDestino.x && spriteRenderer.flipX || transform.position.x > puntoDestino.x && !spriteRenderer.flipX)
        {
            voltear();
        }

    }

    private void atacar()
    {
        Debug.Log("Atacando, disparando, etc etc");
    }

    private void OnDrawGizmosSelected()
    {
        //Dibuja rando de deteccion
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        //Dibuja rango de ataque
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);

    }

    private void voltear()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

}