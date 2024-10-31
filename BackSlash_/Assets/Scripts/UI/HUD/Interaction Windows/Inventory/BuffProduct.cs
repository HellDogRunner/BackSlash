using UnityEngine;

public class BuffProduct : BasicProduct
{
	private TemporaryBuff _product;

	protected override (string, string, int, bool, Sprite) GetValues()
	{
		return (_product.Name, _product.Description, _product.Price, _product.Have, _product.Icon);
	}

	public void SetProduct(TemporaryBuff product) 
	{
		_product = product;
	}
	
	protected override void AddItem()
	{
		_playerInventory.AddItem(_product);
		_product.Have = true;
	}
}