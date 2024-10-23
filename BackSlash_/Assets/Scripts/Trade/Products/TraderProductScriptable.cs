using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TraderProduct", menuName = "Scriptable Objects/Product")]
public class TraderProductScriptable : ScriptableObject
{
	public string Name;
	public int Price;
	public Sprite Icon;
}