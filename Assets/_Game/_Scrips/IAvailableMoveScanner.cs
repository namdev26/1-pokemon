namespace Game.Onet
{
    public interface IAvailableMoveScanner
    {
        bool TryFindAvailableMove(BoardState boardState, out AvailableMove availableMove);
    }
}
