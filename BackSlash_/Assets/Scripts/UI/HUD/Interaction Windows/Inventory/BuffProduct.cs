using Unity.Collections;
using UnityEngine;

public class BuffProduct : BasicProduct
{
	[Header("Buff Settings")]
	[SerializeField] private int _minDamage;
	[SerializeField] private int _maxDamage;
	[SerializeField] private int _minAttackSpeed;
	[SerializeField] private int _maxAttackSpeed;
	[SerializeField] private int _minResistance;
	[SerializeField] private int _maxResistance;
	
	private TemporaryBuff _product;

	protected override void SetProductValues()
	{
		_productName = _product.Name;
		_productDescription = _product.Description;
		_productIcon = _product.Icon;
		_productHave = _product.Have;
		_productPrice = _product.Price;
	}

	public void SetProduct(TemporaryBuff product) 
	{
		_product = product;
		
		_product.SetValues(_minDamage, _maxDamage, _minAttackSpeed, _maxAttackSpeed, _minResistance, _maxResistance);
	}

	protected override string GenerateStats()
	{
		return string.Format("Increases {0} by {1} during ? time.", _product._buffName, _product.Value);
	}

	protected override void AddItem()
	{
		_playerInventory.AddItem(_product);
		_product.Have = true;
	}
}