using UnityEngine;

[CreateAssetMenu(fileName = "GuardModItem", menuName = "Scriptable Objects/Items/GuardModItem")]
public class GuardModItem : Item
{
	[Header("Resists")]
	public int Physical;
	public int Burning;
	public int Bleeding;
	public int Frost;
	public int Shock;
	public int Poison;
	[Space]
	public int Complexity;

	public override void GenerateStats()
	{
		Stats = "";
		Stats += Physical != 0 ? $"Physical resistance: {Physical}\n" : "";
		Stats += Burning != 0 ? $"Burning resistance: {Burning}\n" : "";
		Stats += Bleeding != 0 ? "Bleeding resistance: {Bleeding}\n" : "";
		Stats += Frost != 0 ? $"Frost resistance: {Frost}\n" : "";
		Stats += Shock != 0 ? $"Shock resistance: {Shock}\n" : "";
		Stats += Poison != 0 ? $"Poison resistance: {Poison}\n" : "";
		Stats += $"Complexity: {Complexity}";
	}
}