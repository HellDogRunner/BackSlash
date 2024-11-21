using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI.PlayerMenu
{
	[Serializable]
	public class TabTypeModel : IDatabaseModelPrimaryKey<string>
	{
		public GameObject Prefab;

		public string PrimaryKey => Prefab.name;
	}
}
