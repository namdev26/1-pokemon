using System;

namespace Game.Onet
{
    public sealed class BoardState
    {
        private readonly TileData[,] tiles;

        public BoardState(int playableRowCount, int playableColumnCount)
        {
            if (playableRowCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playableRowCount));
            }

            if (playableColumnCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(playableColumnCount));
            }

            PlayableRowCount = playableRowCount;
            PlayableColumnCount = playableColumnCount;
            TotalRowCount = playableRowCount + 2;
            TotalColumnCount = playableColumnCount + 2;
            tiles = new TileData[TotalRowCount, TotalColumnCount];
            InitializeEmptyTiles();
        }

        public int PlayableRowCount { get; }

        public int PlayableColumnCount { get; }

        public int TotalRowCount { get; }

        public int TotalColumnCount { get; }

        public bool IsInside(BoardCoord coord)
        {
            return coord.Row >= 0
                && coord.Row < TotalRowCount
                && coord.Column >= 0
                && coord.Column < TotalColumnCount;
        }

        public bool IsPlayable(BoardCoord coord)
        {
            return coord.Row > 0
                && coord.Row < TotalRowCount - 1
                && coord.Column > 0
                && coord.Column < TotalColumnCount - 1;
        }

        public TileData GetTile(BoardCoord coord)
        {
            ValidateCoord(coord);
            return tiles[coord.Row, coord.Column];
        }

        public void SetTile(BoardCoord coord, TileData tile)
        {
            if (tile == null)
            {
                throw new ArgumentNullException(nameof(tile));
            }

            ValidateCoord(coord);
            tiles[coord.Row, coord.Column] = tile;
        }

        public bool IsEmpty(BoardCoord coord)
        {
            return GetTile(coord).IsEmpty;
        }

        public void ClearTile(BoardCoord coord)
        {
            ValidateCoord(coord);
            tiles[coord.Row, coord.Column].Clear();
        }

        private void InitializeEmptyTiles()
        {
            for (int row = 0; row < TotalRowCount; row++)
            {
                for (int column = 0; column < TotalColumnCount; column++)
                {
                    tiles[row, column] = TileData.CreateEmpty();
                }
            }
        }

        private void ValidateCoord(BoardCoord coord)
        {
            if (!IsInside(coord))
            {
                throw new ArgumentOutOfRangeException(nameof(coord), coord, "Coordinate is outside the board.");
            }
        }
    }
}
