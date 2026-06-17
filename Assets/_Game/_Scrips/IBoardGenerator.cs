namespace Game.Onet
{
    public interface IBoardGenerator
    {
        BoardState Generate(int playableRowCount, int playableColumnCount, int distinctIconCount);
    }
}
