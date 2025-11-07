using UnityEngine;

public class LanzarSapo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private float fuerzaLanzamiento = 300f;
    [SerializeField] private Camera main;
    private Vector2 startPosition, dragPosition;
    private float startRotation;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = transform.position;
        startRotation = rb.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDrag()
    {
        if (main != null)
        {
            dragPosition = main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = dragPosition;
        }
    }

    private void OnMouseUp()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 direccionLanzamiento = startPosition - dragPosition;
        rb.AddForce(direccionLanzamiento * fuerzaLanzamiento);
        Invoke("resetPosition", 5f);
    }

    private void resetPosition()
    {
        transform.position = startPosition;
        rb.rotation = startRotation;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        main.GetComponent<CameraFollowing>().resetPosition();
    }
}