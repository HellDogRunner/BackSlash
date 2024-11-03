using RedMoonGames.Database;
using System;

[Serializable]
public class InventoryModel : IDatabaseModelPrimaryKey<EItemType>
{
    public EItemType ItemType;
    public Item Item;

    public EItemType PrimaryKey => ItemType;
}

