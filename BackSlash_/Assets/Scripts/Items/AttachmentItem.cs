using UnityEngine;

[CreateAssetMenu(fileName = "AttachmentItem", menuName = "Scriptable Objects/AttachmentItem")]
public class AttachmentItem : Item
{
    [Header("Values")]
    public int Damage;
    public int AttackSpeed;
    public int ElementalDamage;
}
