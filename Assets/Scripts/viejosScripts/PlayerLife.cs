using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource deathSfx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pinchos"))
        {
            deathSfx.Play();
            morir();
        }
    }

    private void morir()
    {
        animator.SetTrigger("dead");
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}