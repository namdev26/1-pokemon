using System;

namespace Game.Onet
{
    public readonly struct DirectionOffset
    {
        public DirectionOffset(OnetDirection direction, int rowDelta, int columnDelta)
        {
            Direction = direction;
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public OnetDirection Direction { get; }

        public int RowDelta { get; }

        public int ColumnDelta { get; }

        public BoardCoord Move(BoardCoord coord)
        {
            return new BoardCoord(coord.Row + RowDelta, coord.Column + ColumnDelta);
        }
    }
}
