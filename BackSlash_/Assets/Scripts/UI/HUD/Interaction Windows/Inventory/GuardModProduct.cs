using UnityEngine;

public class GuardModProduct : BasicProduct
{
	private WeaponGuardMod _product;

	protected override void SetProductValues()
	{
		_productName = _product.Name;
		_productDescription = _product.Description;
		_productIcon = _product.Icon;
		_productHave = _product.Have;
		_productPrice = _product.Price;
	}
	public void SetProduct(WeaponGuardMod product) 
	{
		_product = product;
	}
	
	protected override string GenerateStats()
	{
		return string.Format("Guard stats");
	}

	
	protected override void AddItem()
	{
		_playerInventory.AddItem(_product);
		_product.Have = true;
	}
}