using UnityEngine;

namespace RedMoonGames.Database
{
    public interface IDatabaseModelPrimaryKey<TKey>
    {
        TKey PrimaryKey { get; }
    }
}
