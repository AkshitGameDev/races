using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AblityData", menuName = "Scriptable Objects/AblityData")]
public class AblityData : ScriptableObject
{
    public string label;
    [SerializeReference]
    public List<AblityEffect> effect;

    void OnEnable()
    {

        if(string.IsNullOrEmpty(label))
            label = name;
        if (effect == null)
            effect = new List<AblityEffect>();
    }
    
}

[Serializable]
public abstract class AblityEffect
{
    public abstract void Execute(GameObject caster, GameObject target);
}

[Serializable]
public class DamageEffect : AblityEffect
{
    public int damageAmount;

    public override void Execute(GameObject caster, GameObject target)
    {
        // target.GetComponent<Health>().ApplyDamage(damageAmount);
        // Implement damage logic here
        Debug.Log($"{caster.name} deals {damageAmount} damage to {target.name}");
    }
}

[Serializable]
public class HealEffect : AblityEffect
{
    public int healAmount;

    public override void Execute(GameObject caster, GameObject target)
    {
        // target.GetComponent<Health>().Heal(healAmount);
        // Implement healing logic here
        Debug.Log($"{caster.name} heals {target.name} for {healAmount} health");
    }
}

[Serializable]
public class KnockbackEffect : AblityEffect
{
    public float knockbackForce;

    public override void Execute(GameObject caster, GameObject target)
    {
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            Vector3 knockbackDirection = (target.transform.position - caster.transform.position).normalized;
            targetRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            Debug.Log($"{caster.name} knocks back {target.name} with force {knockbackForce}");
        }
    }
}