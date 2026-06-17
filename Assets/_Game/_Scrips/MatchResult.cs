namespace Game.Onet
{
    public sealed class MatchResult
    {
        private MatchResult(bool isMatched, MatchPath path)
        {
            IsMatched = isMatched;
            Path = path;
        }

        public bool IsMatched { get; }

        public MatchPath Path { get; }

        public static MatchResult Succeeded(MatchPath path)
        {
            return new MatchResult(true, path);
        }

        public static MatchResult Failed()
        {
            return new MatchResult(false, null);
        }
    }
}
