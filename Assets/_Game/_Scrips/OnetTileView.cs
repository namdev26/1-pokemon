using UnityEngine;
using TMPro;

namespace Game.Onet
{
    public sealed class OnetTileView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer tileRenderer;

        [SerializeField]
        private SpriteRenderer selectedIndicatorRenderer;

        [SerializeField]
        private SpriteRenderer hintIndicatorRenderer;

        [SerializeField]
        private BoxCollider2D tileCollider;

        [SerializeField]
        private TextMeshPro labelText;

        private System.Action<BoardCoord> selectionHandler;
        private BoardCoord coord;

        public BoardCoord Coord => coord;

        public void Initialize(BoardCoord tileCoord, System.Action<BoardCoord> onSelected, OnetDemoConfig demoConfig)
        {
            coord = tileCoord;
            selectionHandler = onSelected;
            transform.position = Vector3.zero;
            tileCollider.size = new Vector2(demoConfig.tileWorldWidth, demoConfig.tileWorldHeight);
            tileRenderer.sortingOrder = demoConfig.tileSortingOrder;
            selectedIndicatorRenderer.sortingOrder = demoConfig.indicatorSortingOrder;
            hintIndicatorRenderer.sortingOrder = demoConfig.indicatorSortingOrder;
        }

        public void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public void Bind(int iconId, OnetDemoPalette palette, bool isSelected, bool isHinted)
        {
            gameObject.SetActive(true);
            tileRenderer.color = ResolveTileColor(iconId, palette, isSelected);
            labelText.text = OnetIconLibrary.GetLabel(iconId);
            labelText.color = palette.textColor;
            selectedIndicatorRenderer.gameObject.SetActive(isSelected);
            hintIndicatorRenderer.gameObject.SetActive(isHinted);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private Color ResolveTileColor(int iconId, OnetDemoPalette palette, bool isSelected)
        {
            if (isSelected)
            {
                return palette.selectedTileColor;
            }

            Color iconColor = OnetIconLibrary.GetColor(iconId);
            return Color.Lerp(palette.defaultTileColor, iconColor, 0.3f);
        }

        private void OnMouseUpAsButton()
        {
            selectionHandler?.Invoke(coord);
        }
    }
}
