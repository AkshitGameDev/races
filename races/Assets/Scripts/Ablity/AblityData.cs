using UnityEngine;

[CreateAssetMenu(fileName = "AblityData", menuName = "Scriptable Objects/AblityData")]
public class AblityData : ScriptableObject
{
    public string label;
    [ScerializeReference] public List<AblityEffect> effect;

    void OnEnable()
    {

        if(string.IsNullOrEmpty(label))
            label = name;
        if (effect == null)
            effect = new List<AblityEffect>();
    }
    
}

[Serializable]
abstract class AblityEffect
{
    public abstract void Execute(GameObject caster, GameObject target);
}

[Serializable]
class DamageEffect : AblityEffect
{
    public float damageAmount;

    public override void Execute(GameObject caster, GameObject target)
    {
        target.GetComponent<Health>().ApplyDamage(damageAmount);
        // Implement damage logic here
        Debug.Log($"{caster.name} deals {damageAmount} damage to {target.name}");
    }
}
