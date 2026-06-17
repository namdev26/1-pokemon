using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Onet
{
    public sealed class OnetHudView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text titleText;

        [SerializeField]
        private TMP_Text statusText;

        [SerializeField]
        private TMP_Text hintText;

        [SerializeField]
        private Button newGameButton;

        [SerializeField]
        private Button hintButton;

        public void Initialize(System.Action onNewGameSelected, System.Action onHintSelected)
        {
            newGameButton.onClick.RemoveAllListeners();
            newGameButton.onClick.AddListener(() => onNewGameSelected?.Invoke());

            hintButton.onClick.RemoveAllListeners();
            hintButton.onClick.AddListener(() => onHintSelected?.Invoke());
        }

        public void Bind(OnetDemoPalette palette, string title, string status, string hint)
        {
            titleText.text = title;
            titleText.color = palette.infoColor;
            statusText.text = status;
            statusText.color = palette.infoColor;
            hintText.text = hint;
            hintText.color = palette.hintTileColor;
        }
    }
}
