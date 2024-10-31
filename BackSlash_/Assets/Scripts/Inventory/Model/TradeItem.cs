using UnityEngine;

public class TradeItem : ScriptableObject
{
	[Header("Item Infoarmation")]
	public string Name;
	public int Price;
	public Sprite Icon;
	public bool Have;
	[TextArea] public string Description;
}