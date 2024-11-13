using UnityEngine;

[CreateAssetMenu(fileName = "BuffItem", menuName = "Scriptable Objects/Items/BuffItem")]
public class BuffItem : Item
{
	[SerializeField] private BuffConfigurator _config;
	[HideInInspector] public enum EBuff : int
	{
		Damage,
		StabilityDamage,
		AttackSpeed,
		Mobility,
		Health,
		Resistance
	}

	[HideInInspector] public enum EResist : int
	{
		None,
		Physical,
		Burning,
		Bleeding,
		Frost,
		Shock,
		Poison,
	}
	
	[Header("Parameters will configurate in play time")]
	public EBuff Buff;
	public EResist Resist = 0;
	public int Value;
	
	public override void SetValues()
	{
		_config.Start();
		
		Buff = _config.Buff;
		Resist = _config.Resist;
		Icon = _config.Icon;
		Name = _config.Name;
		Stats = _config.Stats;
		Price = _config.Price;
		Value = _config.Value;
	}
}