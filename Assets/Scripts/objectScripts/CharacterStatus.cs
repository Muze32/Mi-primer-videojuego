using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    private enum estadoMovimiento { idle, walking, air, attack }; //idle = 0, walking = 1, air = 2, attack = 3
    private estadoMovimiento actualState;
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() => GameEvents.OnLaunch += OnLaunch;
    private void OnDisable() => GameEvents.OnLaunch -= OnLaunch;

    private void OnLaunch(GameObject obj)
    {
        if (obj != gameObject) return;
        ChangeStatus("air");
    }

    public void ChangeStatus(string newState)
    {
        if (System.Enum.TryParse(newState, true, out estadoMovimiento parsedState))
        {
            actualState = parsedState;
            animator.SetInteger("estado", (int)actualState);
        }
        else
        {
            Debug.LogWarning($"Estado '{newState}' no existe en el enum.");
        }
    }
}
