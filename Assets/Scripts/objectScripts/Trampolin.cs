using Unity.VisualScripting;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float fuerzaRebote = 15f;
    [SerializeField] private AudioClip bounceSFX;
    [SerializeField] private SoundManager soundManager;
    private int maxRebotes = 2, rebotesActuales = 0;

    private void OnEnable() => GameEvents.OnLaunch += ResetRebotes;
    private void OnDisable() => GameEvents.OnLaunch -= ResetRebotes;

    private void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D rbObj = col.gameObject.GetComponent<Rigidbody2D>();

        if (!rbObj || rebotesActuales >= maxRebotes) return;

        soundManager.PlaySFXOnce(bounceSFX);
        rebotesActuales++;

        Vector2 velocidadEntrada = rbObj.linearVelocity;

        float velocidadHorizontal = Mathf.Abs(velocidadEntrada.x);
        rbObj.linearVelocity = new Vector2(velocidadEntrada.x, fuerzaRebote + velocidadHorizontal * 0.5f);
    }

    private void ResetRebotes(GameObject _)
    {
        rebotesActuales = 0;
    }
}