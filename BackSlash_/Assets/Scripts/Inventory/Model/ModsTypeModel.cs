using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Inventory
{
	[Serializable]
	public class ModsTypeModel : IDatabaseModelPrimaryKey<string>
	{
		public string Name;
		public int Price;
		public Sprite Icon;
		public string PrimaryKey => Name;
	}
}