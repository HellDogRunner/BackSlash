using RedMoonGames.Database;
using System;

[Serializable]
public class InventoryModel : IDatabaseModelPrimaryKey<EItemType>
{
    public EItemType Type;
    public Item Item;

    public EItemType PrimaryKey => Type;
}

