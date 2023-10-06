using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, enemyPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, 1);
    }
}
