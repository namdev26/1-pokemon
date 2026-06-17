using UnityEngine;

namespace Game.Onet
{
    public sealed class OnetWorldCoordMapper
    {
        private readonly OnetDemoConfig demoConfig;

        public OnetWorldCoordMapper(OnetDemoConfig demoConfig)
        {
            this.demoConfig = demoConfig;
        }

        public Vector3 GetTilePosition(BoardCoord coord)
        {
            float stepX = demoConfig.tileWorldWidth + demoConfig.tileSpacing;
            float stepY = demoConfig.tileWorldHeight + demoConfig.tileSpacing;
            float boardWidth = (demoConfig.playableColumnCount - 1) * stepX;
            float boardHeight = (demoConfig.playableRowCount - 1) * stepY;
            float x = demoConfig.boardOrigin.x - boardWidth * 0.5f + (coord.Column - 1) * stepX;
            float y = demoConfig.boardOrigin.y + boardHeight * 0.5f - (coord.Row - 1) * stepY;
            return new Vector3(x, y, demoConfig.boardOrigin.z);
        }

        public Vector3 GetPathPosition(BoardCoord coord)
        {
            return GetTilePosition(coord) + demoConfig.pathOffset;
        }

        public Vector2 GetTileSize()
        {
            return new Vector2(demoConfig.tileWorldWidth, demoConfig.tileWorldHeight);
        }
    }
}
