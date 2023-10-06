using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform enemyPosition)
    {
        Debug.Log("Effect executed!");
    }
}
