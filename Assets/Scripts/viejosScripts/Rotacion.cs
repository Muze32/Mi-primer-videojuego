using UnityEngine;

public class Rotacion : MonoBehaviour
{
    [SerializeField] private float velocidad = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 360 * velocidad * Time.deltaTime);
    }
}
