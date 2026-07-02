using System.Collections;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    [Header("Ability Base")]
    [SerializeField] protected string abilityName;
    [SerializeField] protected float cooldown = 5f;

    protected bool isOnCooldown;
    protected AbilityController abilityController;

    public bool IsOnCooldown => isOnCooldown;
    public string AbilityName => abilityName;

    public virtual void Initialize(AbilityController controller)
    {
        abilityController = controller;
    }

    public abstract void Activate();

    protected IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
