using UnityEngine;

[CreateAssetMenu(fileName = "WeaponGuardMod", menuName = "Scriptable Objects/Items/WeaponGuardMod")]
public class WeaponGuardMod : TradeItem
{
	[Header("Attack Resistance")]
	public bool CommonAttack;
	public bool StabbingAttack;
	public bool KickAttack;
	public bool HeadAttack;
	
	[Header("Negative Effects Resistance")]
	public bool Burning;
	public bool Frost;
	public bool Bleed;
	public bool Shock;
	public bool Poison;
}
