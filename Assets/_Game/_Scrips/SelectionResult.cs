namespace Game.Onet
{
    public sealed class SelectionResult
    {
        private SelectionResult(OnetSelectionOutcome outcome, BoardCoord? primaryCoord, BoardCoord? secondaryCoord, MatchPath path)
        {
            Outcome = outcome;
            PrimaryCoord = primaryCoord;
            SecondaryCoord = secondaryCoord;
            Path = path;
        }

        public OnetSelectionOutcome Outcome { get; }

        public BoardCoord? PrimaryCoord { get; }

        public BoardCoord? SecondaryCoord { get; }

        public MatchPath Path { get; }

        public static SelectionResult Create(OnetSelectionOutcome outcome, BoardCoord? primaryCoord = null, BoardCoord? secondaryCoord = null, MatchPath path = null)
        {
            return new SelectionResult(outcome, primaryCoord, secondaryCoord, path);
        }
    }
}
