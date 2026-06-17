using System.Collections.Generic;

namespace Game.Onet
{
    public sealed class MatchPath
    {
        public MatchPath(IReadOnlyList<BoardCoord> points, int turnCount)
        {
            Points = points;
            TurnCount = turnCount;
        }

        public IReadOnlyList<BoardCoord> Points { get; }

        public int TurnCount { get; }
    }
}
