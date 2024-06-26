using RedMoonGames.Database;
using System;
using UnityEngine;

namespace Scripts.UI
{
    [Serializable]
    public class WindowTypeModel : IDatabaseModelPrimaryKey<EWindowType>
    {
        public EWindowType WindowType;

        [Header("Canvas")]
        public Canvas WindowCanvas;

        public EWindowType PrimaryKey => WindowType;
    }
}