using UnityEngine;

public class rotatoryAxe : MonoBehaviour
{
    [SerializeField] private float velocidadRotacion = 180f; // grados por segundo

    void Update()
    {
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }
}
