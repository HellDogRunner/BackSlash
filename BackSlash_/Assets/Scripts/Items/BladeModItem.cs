using UnityEngine;

[CreateAssetMenu(fileName = "BladeModItem", menuName = "Scriptable Objects/Items/BladeModItem")]
public class BladeModItem : Item
{
	[Header("Parameters")]
	public int Damage;
	public int StabilityDamage;
	public float AttackSpeed;
	public float AttackRange;
	[Space]
	public int Complexity;

	public override void SetValues()
	{
		Stats = "";
		Stats += $"Damage: {Damage}\n";
		Stats += $"Stability Damage: {StabilityDamage}\n";
		Stats += $"Attack Speed: {AttackSpeed}\n";
		Stats += $"Attack Range: {AttackRange}\n";
		Stats += $"Complexity: {Complexity}";
	}
}