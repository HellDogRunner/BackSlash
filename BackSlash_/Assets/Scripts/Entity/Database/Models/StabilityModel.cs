using RedMoonGames.Database;
using System;

namespace Scripts.Entity
{
    [Serializable]
    public class StabilityModel : IDatabaseModelPrimaryKey<string>
    {
        public int Max;
        public int Current;
        public float TimeToRecovery;
        public float RecoveryInterval;
        public float StaggerTime;
        public float StunTime;
        public string PrimaryKey => "";
    }
}
