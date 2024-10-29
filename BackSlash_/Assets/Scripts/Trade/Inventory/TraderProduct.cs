using UnityEngine;

[CreateAssetMenu(fileName = "TraderProduct", menuName = "Scriptable Objects/TraderProduct")]
public class TraderProduct : ScriptableObject
{
	public string Name;
	public int Price;
	public Sprite Icon;
	public bool Sold;
	[TextArea] public string Description;
	//public GameObject Model;
}