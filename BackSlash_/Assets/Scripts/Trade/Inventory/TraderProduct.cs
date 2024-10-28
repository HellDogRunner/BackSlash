using UnityEngine;

[CreateAssetMenu(fileName = "TraderProduct", menuName = "Scriptable Objects/TraderProduct")]
public class TraderProduct : ScriptableObject
{
	public string Name;
	public bool Sold;
	public int Price;
	public GameObject Product;
	[TextArea] public string Description;
	public GameObject Model;
}