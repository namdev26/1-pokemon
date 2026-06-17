using System;
using UnityEngine;

namespace Game.Onet
{
    [Serializable]
    public sealed class OnetDemoPalette
    {
        public Color backgroundColor = new Color(0.16f, 0.11f, 0.06f, 1f);

        public Color defaultTileColor = new Color(0.96f, 0.9f, 0.76f, 1f);

        public Color selectedTileColor = new Color(0.98f, 0.78f, 0.28f, 1f);

        public Color matchedTileColor = new Color(0.45f, 0.86f, 0.48f, 1f);

        public Color hintTileColor = new Color(0.45f, 0.75f, 1f, 1f);

        public Color textColor = new Color(0.2f, 0.16f, 0.1f, 1f);

        public Color infoColor = Color.white;
    }
}
