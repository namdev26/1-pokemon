using UnityEngine;
using UnityEngine.UI;

namespace Game.Onet
{
    public sealed class OnetDemoController : MonoBehaviour
    {
        private const string DefaultTitle = "Onet Demo";
        private const string DefaultStatusMessage = "Select two matching tiles that can be connected with up to 2 turns.";

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private OnetDemoConfig demoConfig = new OnetDemoConfig();

        [SerializeField]
        private OnetDemoPalette palette = new OnetDemoPalette();

        [SerializeField]
        private OnetBoardView boardView;

        [SerializeField]
        private OnetHudView hudView;

        [SerializeField]
        private OnetPathView pathView;

        private IMatchValidationService matchValidationService;
        private IAvailableMoveScanner availableMoveScanner;
        private IBoardGenerator boardGenerator;
        private IBoardShuffleService boardShuffleService;
        private OnetGameSession gameSession;
        private MatchPath activePath;
        private AvailableMove activeHintMove;
        private string statusMessage;

        private void Awake()
        {
            InitializeServices();
            InitializeViews();
            StartNewGame();
        }

        private void InitializeServices()
        {
            matchValidationService = new MatchValidationService();
            availableMoveScanner = new AvailableMoveScanner(matchValidationService);
            boardGenerator = new BoardGenerator(availableMoveScanner);
            boardShuffleService = new BoardShuffleService(availableMoveScanner);
        }

        private void InitializeViews()
        {
            if (backgroundImage != null)
            {
                backgroundImage.color = palette.backgroundColor;
            }

            hudView.Initialize(StartNewGame, ShowHint);
            pathView.Initialize(demoConfig);
        }

        private void StartNewGame()
        {
            BoardState boardState = boardGenerator.Generate(
                demoConfig.playableRowCount,
                demoConfig.playableColumnCount,
                demoConfig.distinctIconCount);

            gameSession = new OnetGameSession(boardState, matchValidationService, availableMoveScanner, boardShuffleService);
            boardView.Build(boardState, demoConfig, HandleSelection);
            activePath = null;
            activeHintMove = null;
            statusMessage = DefaultStatusMessage;
            RefreshViews();
        }

        private void HandleSelection(BoardCoord coord)
        {
            activeHintMove = null;
            SelectionResult result = gameSession.Select(coord);
            UpdateStatus(result);
            RefreshViews();
        }

        private void ShowHint()
        {
            if (!gameSession.TryGetHint(out AvailableMove availableMove))
            {
                activeHintMove = null;
                activePath = null;
                statusMessage = "No available move found.";
                RefreshViews();
                return;
            }

            activeHintMove = availableMove;
            activePath = null;
            statusMessage = "Hint highlighted.";
            RefreshViews();
        }

        private void UpdateStatus(SelectionResult result)
        {
            switch (result.Outcome)
            {
                case OnetSelectionOutcome.FirstSelected:
                    statusMessage = $"Selected {result.PrimaryCoord}. Choose a matching tile.";
                    activePath = null;
                    break;

                case OnetSelectionOutcome.SelectionCleared:
                    statusMessage = "Selection cleared.";
                    activePath = null;
                    break;

                case OnetSelectionOutcome.MatchSucceeded:
                    statusMessage = $"Matched {result.PrimaryCoord} with {result.SecondaryCoord}.";
                    activePath = result.Path;
                    break;

                case OnetSelectionOutcome.MatchFailed:
                    statusMessage = $"Cannot connect {result.PrimaryCoord} with {result.SecondaryCoord}.";
                    activePath = null;
                    break;

                case OnetSelectionOutcome.BoardShuffled:
                    statusMessage = "Matched pair. Board was shuffled because no move remained.";
                    activePath = result.Path;
                    break;

                case OnetSelectionOutcome.GameWon:
                    statusMessage = "You cleared the board. Demo win state reached.";
                    activePath = result.Path;
                    break;

                case OnetSelectionOutcome.InvalidSelection:
                    statusMessage = "Invalid selection.";
                    activePath = null;
                    break;

                default:
                    statusMessage = "No action.";
                    activePath = null;
                    break;
            }
        }

        private void RefreshViews()
        {
            boardView.Refresh(gameSession.BoardState, palette, gameSession.SelectedCoord, activeHintMove);
            hudView.Bind(palette, DefaultTitle, statusMessage, BuildHintMessage());

            if (activeHintMove != null)
            {
                pathView.Draw(activeHintMove.Path, demoConfig, palette.hintTileColor);
                return;
            }

            if (activePath != null)
            {
                pathView.Draw(activePath, demoConfig, palette.matchedTileColor);
                return;
            }

            pathView.Clear();
        }

        private string BuildHintMessage()
        {
            return activeHintMove == null
                ? string.Empty
                : $"Hint: {activeHintMove.StartCoord} -> {activeHintMove.TargetCoord}";
        }
    }
}
