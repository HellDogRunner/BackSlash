using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBladeMod", menuName = "Scriptable Objects/Items/WeaponBladeMod")]
public class WeaponBladeMod : TradeItem
{
	[Header("Blade Stats")]
	public int Damage;
	public int PoiseDamage;
	public int AttackSpeed;
}
