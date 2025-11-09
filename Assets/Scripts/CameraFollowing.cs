using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform personaje;
    [SerializeField] private float tope;
    private Vector3 posicionInicial;
    private bool shouldFollow = true;
    void Start()
    {
        posicionInicial = transform.position;
    }
    private void LateUpdate()
    {
        if(shouldFollow && personaje != null)
        {
            if (personaje.position.x > transform.position.x && personaje.position.x < tope)
            {
                transform.position = new Vector3(personaje.position.x, transform.position.y, transform.position.z);
            }
        }
    }

    public void resetPosition()
    {
        Debug.Log("reiniciando camara");
        transform.position = posicionInicial;
        shouldFollow = false;
    }
    public void actualizarPersonaje(GameObject nvoPersonaje)
    {
        personaje = nvoPersonaje.transform;
        shouldFollow = true;
    }
}