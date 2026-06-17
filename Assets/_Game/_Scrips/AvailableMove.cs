namespace Game.Onet
{
    public sealed class AvailableMove
    {
        public AvailableMove(BoardCoord startCoord, BoardCoord targetCoord, MatchPath path)
        {
            StartCoord = startCoord;
            TargetCoord = targetCoord;
            Path = path;
        }

        public BoardCoord StartCoord { get; }

        public BoardCoord TargetCoord { get; }

        public MatchPath Path { get; }
    }
}
