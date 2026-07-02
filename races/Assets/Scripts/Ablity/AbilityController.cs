using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityController : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField] private AbilityBase tacticalAbility;
    [SerializeField] private AbilityBase ultimateAbility;

    private void Start()
    {
        if (tacticalAbility != null)
            tacticalAbility.Initialize(this);

        if (ultimateAbility != null)
            ultimateAbility.Initialize(this);
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            UseTacticalAbility();
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            UseUltimateAbility();
        }
    }

    private void UseTacticalAbility()
    {
        if (tacticalAbility == null) return;
        if (tacticalAbility.IsOnCooldown) return;

        tacticalAbility.Activate();
    }

    private void UseUltimateAbility()
    {
        if (ultimateAbility == null) return;
        if (ultimateAbility.IsOnCooldown) return;

        ultimateAbility.Activate();
    }
}