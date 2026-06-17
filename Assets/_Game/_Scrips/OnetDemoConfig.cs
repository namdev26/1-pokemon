using System;
using UnityEngine;

namespace Game.Onet
{
    [Serializable]
    public sealed class OnetDemoConfig
    {
        [Min(2)]
        public int playableRowCount = 8;

        [Min(2)]
        public int playableColumnCount = 10;

        [Min(2)]
        public int distinctIconCount = 10;

        [Min(0.1f)]
        public float tileWorldWidth = 1.2f;

        [Min(0.1f)]
        public float tileWorldHeight = 1.2f;

        [Min(0f)]
        public float tileSpacing = 0.15f;

        public Vector3 boardOrigin = Vector3.zero;

        public Vector3 pathOffset = new Vector3(0f, 0f, -0.1f);

        [Min(0.01f)]
        public float pathThickness = 0.12f;

        [Min(1)]
        public int tileSortingOrder = 0;

        [Min(1)]
        public int indicatorSortingOrder = 1;
    }
}
