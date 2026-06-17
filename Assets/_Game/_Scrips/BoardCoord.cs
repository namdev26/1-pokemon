using System;

namespace Game.Onet
{
    [Serializable]
    public readonly struct BoardCoord : IEquatable<BoardCoord>
    {
        public BoardCoord(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; }

        public int Column { get; }

        public bool Equals(BoardCoord other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardCoord other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(BoardCoord left, BoardCoord right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoardCoord left, BoardCoord right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }
    }
}
