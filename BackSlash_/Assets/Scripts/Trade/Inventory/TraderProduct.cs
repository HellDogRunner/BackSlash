using UnityEngine;

[CreateAssetMenu(fileName = "TraderProduct", menuName = "Scriptable Objects/TraderProduct")]
public class TraderProduct : ScriptableObject
{
	public string Name;
	public int Price;
	public GameObject Icon;
	public GameObject Prefab;
}