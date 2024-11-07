using UnityEngine;

[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/BuffItem")]
public class BuffItem : Item
{
    [Header("Values")]
    public int Damage;
    public int Resistance;
    public int AttackSpeed;
}
