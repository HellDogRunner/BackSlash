using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.Inventory
{
	[Serializable]
	public class ModsTypeModel : IDatabaseModelPrimaryKey<string>
	{
		public string Name;
		public int Prace;
		public Sprite Icon;
		public string PrimaryKey => Name;
	}
}