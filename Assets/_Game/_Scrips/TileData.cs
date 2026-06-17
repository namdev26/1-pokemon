namespace Game.Onet
{
    public sealed class TileData
    {
        public const int EmptyIconId = -1;

        public TileData(int iconId)
        {
            IconId = iconId;
        }

        public int IconId { get; private set; }

        public bool IsEmpty => IconId == EmptyIconId;

        public void Clear()
        {
            IconId = EmptyIconId;
        }

        public static TileData CreateEmpty()
        {
            return new TileData(EmptyIconId);
        }
    }
}
