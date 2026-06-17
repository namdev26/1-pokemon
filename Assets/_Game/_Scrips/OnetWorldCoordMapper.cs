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
            float x = GetCellCenterX(coord.Column);
            float y = GetCellCenterY(coord.Row);
            return new Vector3(x, y, demoConfig.boardOrigin.z);
        }

        public Vector3 GetPathPosition(BoardCoord coord)
        {
            Vector3 tilePosition = GetTilePosition(coord);
            return tilePosition + demoConfig.pathOffset;
        }

        public Vector2 GetCellSize()
        {
            return new Vector2(demoConfig.cellWidth, demoConfig.cellHeight);
        }

        private float GetCellCenterX(int column)
        {
            float stepX = demoConfig.cellWidth + demoConfig.cellSpacingX;
            float boardWidth = (demoConfig.playableColumnCount - 1) * stepX;
            return demoConfig.boardOrigin.x - boardWidth * 0.5f + (column - 1) * stepX;
        }

        private float GetCellCenterY(int row)
        {
            float stepY = demoConfig.cellHeight + demoConfig.cellSpacingY;
            float boardHeight = (demoConfig.playableRowCount - 1) * stepY;
            return demoConfig.boardOrigin.y + boardHeight * 0.5f - (row - 1) * stepY;
        }
    }
}
