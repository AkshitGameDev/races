using UnityEngine;

[CreateAssetMenu(fileName = "AblityData", menuName = "Scriptable Objects/AblityData")]
public class AblityData : ScriptableObject
{
    
}

[Serializable]
abstract class AblityEffect
{
    public abstract void Execute(GameObject caster, GameObject target);
}
