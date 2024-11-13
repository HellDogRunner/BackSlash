using System.Collections.Generic;
using UnityEngine;
using static BuffItem;

public class BuffConfigurator : ScriptableObject
{
	[SerializeField] private float _basicPrice;
	[SerializeField] private int _valueMulti;

	[SerializeField]
	private List<string> _names = new List<string>()
	{
		"Damage Buff",
		"Stability Damage Buff",
		"Attack Speed Buff",
		"Mobility Name Buff",
		"Health Buff",
		"Resistance Buff",
	};
	[SerializeField] private List<int> _resistsMulti = new List<int>();
	[SerializeField] private List<Sprite> _sprites = new List<Sprite>();

	[HideInInspector] public EBuff Buff;
	[HideInInspector] public EResist Resist = EResist.None;
	[HideInInspector] public Sprite Icon;
	[HideInInspector] public string Name;
	[HideInInspector] public string Stats;
	[HideInInspector] public int Price;
	[HideInInspector] public int Value;

	public void Start()
	{
		RandomizeBuff();
		SetValues();
		SetStats();
	}

	private void RandomizeBuff()
	{
		Buff = (EBuff)Random.Range(0, System.Enum.GetValues(typeof(EBuff)).Length);
		if (Buff == EBuff.Resistance)
		{
			var Resist = (EResist)Random.Range(1, System.Enum.GetValues(typeof(EResist)).Length);
		}
		Value = Random.Range(5, 15);
	}

	private void SetValues()
	{
		int multi = Value * _valueMulti;

		int i = 0;
		for (EBuff buff = 0; (int)buff < System.Enum.GetValues(typeof(EBuff)).Length; buff++)
		{
			if (Buff == buff)
			{
				multi += _resistsMulti[i];
				Name = _names[i];
				Icon = _sprites[i];
				break;
			}
			i++;
		}

		Price = (int)(_basicPrice * (multi + 100) / 100);
	}
	
	private void SetStats()
	{
		Stats = "";
		Stats += $"Buff: {Buff}\n";
		Stats += Resist != EResist.None ? $"Resistance: {Resist}\n" : "";
		Stats += $"Value: {Value}";
	}
}