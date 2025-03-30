using System;

namespace Scripts.Entity
{
    [Serializable]  
    public enum EEntity : int
    {
        Physical = 0,
        Burning = 1,
        Bleeding = 2,
        Frost = 3,
        Shock = 4,
        Poison = 5,
    }
}
