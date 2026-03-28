using UnityEngine;

public class CBallPower : MonoBehaviour
{
    [SerializeField] private float factorGravedad = 3f; // Multiplicador de la gravedad (ej: 3x más pesado)
    [SerializeField] private float factorMasa = 2f;      // Multiplicador de la masa (ej: 2x más masa)
    private Rigidbody2D rb;
    private CharacterStatus characterStatus;

    private void Start()
    {
        characterStatus = GetComponent<CharacterStatus>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    { 
        if (Input.GetMouseButtonDown(0) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            Debug.Log("click");
            ActivarSuperPeso();
            // Deshabilita el script para evitar que se use de nuevo
            this.enabled = false;
        }
    }

    private void ActivarSuperPeso()
    {
        Debug.Log("Atacando");
        //Cambia el sprite a modo ataque
        characterStatus.ChangeStatus("attack");

        // Aumentar el peso (afecta la caída)
        rb.gravityScale *= factorGravedad;

        // Aumentar la masa (afecta el impacto y la inercia)
        rb.mass *= factorMasa;
    }
}