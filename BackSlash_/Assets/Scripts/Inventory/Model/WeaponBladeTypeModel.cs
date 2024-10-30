using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Inventory
{
	[Serializable]
	public class WeaponBladeTypeModel : IDatabaseModelPrimaryKey<string>
	{
		public string Name;
		public int Price;
		public Sprite Icon;
		public bool Have;
		[TextArea] public string Description;
		public int Damage;
		public int PoiseDamage;
		public int AttackSpeed;
		
		public string PrimaryKey => Name;
	}
}