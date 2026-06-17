using System.Collections.Generic;

namespace Game.Onet
{
    public sealed class AvailableMoveScanner : IAvailableMoveScanner
    {
        private readonly IMatchValidationService matchValidationService;

        public AvailableMoveScanner(IMatchValidationService matchValidationService)
        {
            this.matchValidationService = matchValidationService;
        }

        public bool TryFindAvailableMove(BoardState boardState, out AvailableMove availableMove)
        {
            List<BoardCoord> occupiedCoords = GetOccupiedCoords(boardState);

            for (int startIndex = 0; startIndex < occupiedCoords.Count; startIndex++)
            {
                BoardCoord startCoord = occupiedCoords[startIndex];
                TileData startTile = boardState.GetTile(startCoord);

                for (int targetIndex = startIndex + 1; targetIndex < occupiedCoords.Count; targetIndex++)
                {
                    BoardCoord targetCoord = occupiedCoords[targetIndex];
                    TileData targetTile = boardState.GetTile(targetCoord);
                    if (startTile.IconId != targetTile.IconId)
                    {
                        continue;
                    }

                    MatchResult matchResult = matchValidationService.TryMatch(boardState, startCoord, targetCoord);
                    if (!matchResult.IsMatched)
                    {
                        continue;
                    }

                    availableMove = new AvailableMove(startCoord, targetCoord, matchResult.Path);
                    return true;
                }
            }

            availableMove = null;
            return false;
        }

        private static List<BoardCoord> GetOccupiedCoords(BoardState boardState)
        {
            List<BoardCoord> occupiedCoords = new List<BoardCoord>();

            for (int row = 1; row < boardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < boardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    if (boardState.IsEmpty(coord))
                    {
                        continue;
                    }

                    occupiedCoords.Add(coord);
                }
            }

            return occupiedCoords;
        }
    }
}
