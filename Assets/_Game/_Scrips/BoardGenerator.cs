using System;
using System.Collections.Generic;

namespace Game.Onet
{
    public sealed class BoardGenerator : IBoardGenerator
    {
        private const int MaximumGenerationAttemptCount = 64;

        private readonly IAvailableMoveScanner availableMoveScanner;
        private readonly Random random;

        public BoardGenerator(IAvailableMoveScanner availableMoveScanner, Random random = null)
        {
            this.availableMoveScanner = availableMoveScanner;
            this.random = random ?? new Random();
        }

        public BoardState Generate(int playableRowCount, int playableColumnCount, int distinctIconCount)
        {
            int playableTileCount = playableRowCount * playableColumnCount;
            if (playableTileCount % 2 != 0)
            {
                throw new ArgumentException("Playable tile count must be even.");
            }

            if (distinctIconCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distinctIconCount));
            }

            for (int attemptIndex = 0; attemptIndex < MaximumGenerationAttemptCount; attemptIndex++)
            {
                BoardState boardState = new BoardState(playableRowCount, playableColumnCount);
                List<int> iconIds = CreatePairIconIds(playableTileCount, distinctIconCount);
                Shuffle(iconIds);
                FillBoard(boardState, iconIds);

                if (availableMoveScanner.TryFindAvailableMove(boardState, out _))
                {
                    return boardState;
                }
            }

            throw new InvalidOperationException("Unable to generate a valid board with available moves.");
        }

        private List<int> CreatePairIconIds(int playableTileCount, int distinctIconCount)
        {
            List<int> iconIds = new List<int>(playableTileCount);
            int pairCount = playableTileCount / 2;

            for (int pairIndex = 0; pairIndex < pairCount; pairIndex++)
            {
                int iconId = pairIndex % distinctIconCount;
                iconIds.Add(iconId);
                iconIds.Add(iconId);
            }

            return iconIds;
        }

        private void FillBoard(BoardState boardState, List<int> iconIds)
        {
            int iconIndex = 0;

            for (int row = 1; row < boardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < boardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    boardState.SetTile(coord, new TileData(iconIds[iconIndex]));
                    iconIndex++;
                }
            }
        }

        private void Shuffle(List<int> values)
        {
            for (int index = values.Count - 1; index > 0; index--)
            {
                int swapIndex = random.Next(index + 1);
                int temp = values[index];
                values[index] = values[swapIndex];
                values[swapIndex] = temp;
            }
        }
    }
}
