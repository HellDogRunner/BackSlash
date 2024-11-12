using Scripts.Combo.Models;
using UnityEngine;

[CreateAssetMenu(fileName = "HiltModItem", menuName = "Scriptable Objects/Items/HiltModItem")]
public class HiltModItem : Item
{
	public string ComboAttack;
	[Space]
	public int Complexity;
	
	public override void GenerateStats()
	{
		Stats = "";
		Stats += $"Improves {ComboAttack}\n";
		Stats += $"Complexity: {Complexity}";
	}
}
