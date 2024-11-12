using UnityEngine;

public class Item : ScriptableObject
{
	[Header("Item Infoarmation")]
	public string Name;
	public int Price;
	public Sprite Icon;
	[TextArea] public string Description;
	[HideInInspector] public string Stats;
	
	public virtual void GenerateStats() {}
}