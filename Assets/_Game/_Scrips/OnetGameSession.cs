using System;

namespace Game.Onet
{
    public sealed class OnetGameSession
    {
        private readonly IMatchValidationService matchValidationService;
        private readonly IAvailableMoveScanner availableMoveScanner;
        private readonly IBoardShuffleService boardShuffleService;

        private BoardCoord? selectedCoord;

        public OnetGameSession(
            BoardState boardState,
            IMatchValidationService matchValidationService,
            IAvailableMoveScanner availableMoveScanner,
            IBoardShuffleService boardShuffleService)
        {
            BoardState = boardState ?? throw new ArgumentNullException(nameof(boardState));
            this.matchValidationService = matchValidationService ?? throw new ArgumentNullException(nameof(matchValidationService));
            this.availableMoveScanner = availableMoveScanner ?? throw new ArgumentNullException(nameof(availableMoveScanner));
            this.boardShuffleService = boardShuffleService ?? throw new ArgumentNullException(nameof(boardShuffleService));
        }

        public BoardState BoardState { get; }

        public BoardCoord? SelectedCoord => selectedCoord;

        public bool TryGetHint(out AvailableMove availableMove)
        {
            return availableMoveScanner.TryFindAvailableMove(BoardState, out availableMove);
        }

        public SelectionResult Select(BoardCoord coord)
        {
            if (!BoardState.IsPlayable(coord) || BoardState.IsEmpty(coord))
            {
                return SelectionResult.Create(OnetSelectionOutcome.InvalidSelection, selectedCoord, coord);
            }

            if (!selectedCoord.HasValue)
            {
                selectedCoord = coord;
                return SelectionResult.Create(OnetSelectionOutcome.FirstSelected, coord);
            }

            if (selectedCoord.Value == coord)
            {
                selectedCoord = null;
                return SelectionResult.Create(OnetSelectionOutcome.SelectionCleared, coord);
            }

            BoardCoord firstCoord = selectedCoord.Value;
            selectedCoord = null;

            MatchResult matchResult = matchValidationService.TryMatch(BoardState, firstCoord, coord);
            if (!matchResult.IsMatched)
            {
                return SelectionResult.Create(OnetSelectionOutcome.MatchFailed, firstCoord, coord);
            }

            BoardState.ClearTile(firstCoord);
            BoardState.ClearTile(coord);

            if (IsBoardCleared())
            {
                return SelectionResult.Create(OnetSelectionOutcome.GameWon, firstCoord, coord, matchResult.Path);
            }

            if (availableMoveScanner.TryFindAvailableMove(BoardState, out _))
            {
                return SelectionResult.Create(OnetSelectionOutcome.MatchSucceeded, firstCoord, coord, matchResult.Path);
            }

            bool isShuffled = boardShuffleService.TryShuffle(BoardState);
            return SelectionResult.Create(
                isShuffled ? OnetSelectionOutcome.BoardShuffled : OnetSelectionOutcome.MatchSucceeded,
                firstCoord,
                coord,
                matchResult.Path);
        }

        private bool IsBoardCleared()
        {
            for (int row = 1; row < BoardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < BoardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    if (!BoardState.IsEmpty(coord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
