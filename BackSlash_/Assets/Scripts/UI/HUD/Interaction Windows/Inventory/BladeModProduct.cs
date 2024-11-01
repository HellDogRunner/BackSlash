using UnityEngine;

public class BladeModProduct : BasicProduct
{
	private WeaponBladeMod _product;

	protected override void SetProductValues()
	{
		_productName = _product.Name;
		_productDescription = _product.Description;
		_productIcon = _product.Icon;
		_productHave = _product.Have;
		_productPrice = _product.Price;
	}

	public void SetProduct(WeaponBladeMod product) 
	{
		_product = product;
	}
	
	protected override string GenerateStats()
	{
		return string.Format("Damage: {0}\nAttack speed: {1}\nPoise damage: {2}\nRange: {3}", _product.Damage, _product.AttackSpeed, _product.PoiseDamage, _product.Range);
	}

	
	protected override void AddItem()
	{
		_playerInventory.AddItem(_product);
		_product.Have = true;
	}
}