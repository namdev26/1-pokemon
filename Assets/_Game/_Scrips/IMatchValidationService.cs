namespace Game.Onet
{
    public interface IMatchValidationService
    {
        MatchResult TryMatch(BoardState boardState, BoardCoord startCoord, BoardCoord targetCoord);
    }
}
