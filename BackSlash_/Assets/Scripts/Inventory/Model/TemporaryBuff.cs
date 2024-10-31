using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryBuff", menuName = "Scriptable Objects/Items/TemporaryBuff")]
public class TemporaryBuff : TradeItem
{
	[Header("Buff Stats")]
	public bool Damage;
	public bool Resistance;
	public float Buff;
	public float Duration;
}
