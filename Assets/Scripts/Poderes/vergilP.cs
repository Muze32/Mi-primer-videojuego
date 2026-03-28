using UnityEngine;
using System.Collections;

public class VergilP : MonoBehaviour
{
    [Header("Configuración")]
    public float slashRadius = 1.5f;
    public float slashDamage = 50f;
    public float knockbackForce = 10f;
    public float attackDelay = 0.2f;

    private CharacterStatus characterStatus;
    private Rigidbody2D rb;

    private void Awake()
    {
        characterStatus = GetComponent<CharacterStatus>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            Debug.Log("Power activado");
            ActivatePower();
        }
    }

    private void ActivatePower()
    {
        characterStatus.ChangeStatus("attack");
        StartCoroutine(SlashRoutine());
        this.enabled = false;
    }

    private IEnumerator SlashRoutine()
    {
        Vector2 direction = rb.linearVelocity.normalized;

        yield return new WaitForSeconds(attackDelay);

        Vector2 slashOrigin = (Vector2)transform.position + direction * (slashRadius * 0.5f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(slashOrigin, slashRadius);

        Debug.Log($"Objetos golpeados: {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                hitRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rb == null) return;
        Vector2 direction = rb.linearVelocity.normalized;
        Vector2 slashOrigin = (Vector2)transform.position + direction * (slashRadius * 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(slashOrigin, slashRadius);
    }
}
