using System;
using System.Collections.Generic;

namespace Game.Onet
{
    public sealed class BoardShuffleService : IBoardShuffleService
    {
        private const int MaximumShuffleAttemptCount = 64;

        private readonly IAvailableMoveScanner availableMoveScanner;
        private readonly Random random;

        public BoardShuffleService(IAvailableMoveScanner availableMoveScanner, Random random = null)
        {
            this.availableMoveScanner = availableMoveScanner;
            this.random = random ?? new Random();
        }

        public bool TryShuffle(BoardState boardState)
        {
            List<int> iconIds = GetOccupiedIconIds(boardState);
            if (iconIds.Count == 0)
            {
                return false;
            }

            for (int attemptIndex = 0; attemptIndex < MaximumShuffleAttemptCount; attemptIndex++)
            {
                Shuffle(iconIds);
                ApplyIconIds(boardState, iconIds);
                if (availableMoveScanner.TryFindAvailableMove(boardState, out _))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<int> GetOccupiedIconIds(BoardState boardState)
        {
            List<int> iconIds = new List<int>();

            for (int row = 1; row < boardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < boardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    if (boardState.IsEmpty(coord))
                    {
                        continue;
                    }

                    iconIds.Add(boardState.GetTile(coord).IconId);
                }
            }

            return iconIds;
        }

        private static void ApplyIconIds(BoardState boardState, List<int> iconIds)
        {
            int iconIndex = 0;

            for (int row = 1; row < boardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < boardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    if (boardState.IsEmpty(coord))
                    {
                        continue;
                    }

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
