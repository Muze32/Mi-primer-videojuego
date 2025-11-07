using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherryCont = 0;
    [SerializeField] private Text txtCherry;
    [SerializeField] private AudioSource cherrySfx;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Cherries")) //Si el objeto con el que collisiona tiene un tag "cherries"
        {
            cherrySfx.Play();
            cherryCont++;
            Destroy(collision.gameObject);
            txtCherry.text = "Cherries: " + cherryCont;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
