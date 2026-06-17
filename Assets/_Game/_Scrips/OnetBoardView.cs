using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Onet
{
    public sealed class OnetBoardView : MonoBehaviour
    {
        [SerializeField]
        private Transform boardRoot;

        [SerializeField]
        private OnetTileView tilePrefab;

        private readonly Dictionary<BoardCoord, OnetTileView> tileViews = new Dictionary<BoardCoord, OnetTileView>();
        private Action<BoardCoord> selectionHandler;
        private OnetWorldCoordMapper coordMapper;
        private OnetDemoConfig demoConfig;
        private Vector2 cellSize;

        public void Build(BoardState boardState, OnetDemoConfig config, Action<BoardCoord> onTileSelected)
        {
            demoConfig = config;
            selectionHandler = onTileSelected;
            coordMapper = new OnetWorldCoordMapper(config);
            cellSize = coordMapper.GetCellSize();
            EnsureTiles(boardState);
        }

        public void Refresh(BoardState boardState, OnetDemoPalette palette, BoardCoord? selectedCoord, AvailableMove hintMove)
        {
            foreach (KeyValuePair<BoardCoord, OnetTileView> pair in tileViews)
            {
                BoardCoord coord = pair.Key;
                OnetTileView tileView = pair.Value;

                if (boardState.IsEmpty(coord))
                {
                    tileView.Hide();
                    continue;
                }

                int iconId = boardState.GetTile(coord).IconId;
                bool isSelected = selectedCoord.HasValue && selectedCoord.Value == coord;
                bool isHinted = hintMove != null && (hintMove.StartCoord == coord || hintMove.TargetCoord == coord);
                tileView.Bind(iconId, palette, isSelected, isHinted);
            }
        }

        private void EnsureTiles(BoardState boardState)
        {
            for (int row = 1; row < boardState.TotalRowCount - 1; row++)
            {
                for (int column = 1; column < boardState.TotalColumnCount - 1; column++)
                {
                    BoardCoord coord = new BoardCoord(row, column);
                    if (tileViews.ContainsKey(coord))
                    {
                        continue;
                    }

                    Vector3 worldPosition = coordMapper.GetTilePosition(coord);
                    OnetTileView tileView = Instantiate(tilePrefab, worldPosition, Quaternion.identity, boardRoot);
                    tileView.name = $"Tile_{row}_{column}";
                    tileView.Initialize(coord, selectionHandler, demoConfig, cellSize);
                    tileView.SetPosition(worldPosition);
                    tileViews.Add(coord, tileView);
                }
            }
        }
    }
}
