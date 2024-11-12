using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Items/BuffItem")]
public class BuffItem : Item
{
	[Header("Parametrs")]
	public int Damage;
	public int Resistance;
	public int AttackSpeed;

	public override void GenerateStats()
	{
		Stats = "";
		Stats += Damage != 0 ? $"Damage: {Damage}\n" : "";
		Stats += Resistance != 0 ? $"Resistance: {Resistance}\n" : "";
		Stats += AttackSpeed != 0 ? $"Attack Speed: {AttackSpeed}" : "";
	}
	
	public void RandomizeBuff()
	{
		// 
	}
}