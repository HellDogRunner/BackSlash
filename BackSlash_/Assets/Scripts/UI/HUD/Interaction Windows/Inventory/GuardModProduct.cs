using UnityEngine;

public class GuardModProduct : BasicProduct
{
	private WeaponGuardMod _product;

	protected override (string, string, int, bool, Sprite) GetValues()
	{
		return (_product.Name, _product.Description, _product.Price, _product.Have, _product.Icon);
	}

	public void SetProduct(WeaponGuardMod product) 
	{
		_product = product;
	}
	
	protected override void AddItem()
	{
		_playerInventory.AddItem(_product);
		_product.Have = true;
	}
}