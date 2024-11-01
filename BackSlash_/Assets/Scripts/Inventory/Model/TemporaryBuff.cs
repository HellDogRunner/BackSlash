using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryBuff", menuName = "Scriptable Objects/Items/TemporaryBuff")]
public class TemporaryBuff : TradeItem
{
	[Header("Attack")]
	public bool Damage;
	public bool AttackSpeed;

	[Header("Defense")]
	public bool Resistance;
	[Space]
	public bool Burning;
	public bool Frost;
	public bool Bleed;
	public bool Shock;
	public bool Poison;
	[Space]
	public int Value;
	public bool setted;

	[HideInInspector] public string _buffName;
	private int _minDamage;
	private int _maxDamage;
	private int _minAS;
	private int _maxAS;
	private int _minResist;
	private int _maxResist;
	
	
	public void SetValues(int minDmg, int maxDmg, int minAS, int maxAS, int minRes, int maxRes)
	{
		if (setted) return;
		setted = true;
		
		_minDamage = minDmg;
		_maxDamage = maxDmg;
		_minAS = minAS;
		_maxAS = maxAS;
		_minResist = minRes;
		_maxResist = maxRes;
		
		RandomizeBuff();
	}
	
	private void RandomizeBuff()
	{
		switch (Random.Range(0, 3))
		{
			case 0:
				Damage = true;
				Value = GetDamageBuff();
				Name = "Damage buff";
				_buffName = "damage";
				break;

			case 1:
				AttackSpeed = true;
				Value = GetAttackSPeedBuff();
				Name = "Attack Speed Buff";
				_buffName = "attack speed";
				break;

			case 2:
				Resistance = true;
				Value = GetResistanceBuff();
				Name = "Resistance Buff";
				break;
		}
	}

	private int GetDamageBuff()
	{
		return Random.Range(_minDamage, _maxDamage);
	}

	private int GetAttackSPeedBuff()
	{
		return Random.Range(_minAS, _maxAS);
	}

	private int GetResistanceBuff()
	{
		switch (Random.Range(0, 5))
		{
			case 0:
				Burning = true;
				_buffName = "burning resistanse";
				break;

			case 1:
				Frost = true;
				_buffName = "frost resistanse";
				break;

			case 2:
				Bleed = true;
				_buffName = "bleed resistanse";
				break;

			case 3:
				Shock = true;
				_buffName = "shock resistanse";
				break;

			case 4:
				Poison = true;
				_buffName = "poison resistanse";
				break;
		}

		return Random.Range(_minResist, _maxResist);
	}
}