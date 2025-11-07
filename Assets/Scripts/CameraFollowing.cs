using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform sapo;
    [SerializeField] private float tope;
    private Vector3 posicionInicial;
    void Start()
    {
        posicionInicial = transform.position;
    }

    private void LateUpdate()
    {
        if (sapo.position.x > transform.position.x && sapo.position.x < tope)
        {
            transform.position = new Vector3(sapo.position.x, transform.position.y, transform.position.z);
        }
    }

    public void resetPosition()
    {
        transform.position = posicionInicial;
    }
}