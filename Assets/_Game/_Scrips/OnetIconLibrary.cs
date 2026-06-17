using UnityEngine;

namespace Game.Onet
{
    public static class OnetIconLibrary
    {
        private static readonly string[] IconLabels =
        {
            "Cat",
            "Fox",
            "Monkey",
            "Duck",
            "Koala",
            "Fish",
            "Lion",
            "Whale",
            "Chameleon",
            "Giraffe",
            "Hippo",
            "Bee",
            "Pig",
            "Parrot",
            "Frog",
            "Shark"
        };

        private static readonly Color[] IconColors =
        {
            new Color(0.95f, 0.55f, 0.25f, 1f),
            new Color(0.9f, 0.45f, 0.25f, 1f),
            new Color(0.68f, 0.48f, 0.28f, 1f),
            new Color(0.96f, 0.82f, 0.2f, 1f),
            new Color(0.6f, 0.65f, 0.68f, 1f),
            new Color(0.55f, 0.34f, 0.95f, 1f),
            new Color(0.78f, 0.52f, 0.2f, 1f),
            new Color(0.33f, 0.67f, 0.92f, 1f),
            new Color(0.4f, 0.82f, 0.38f, 1f),
            new Color(0.97f, 0.78f, 0.34f, 1f),
            new Color(0.55f, 0.67f, 0.92f, 1f),
            new Color(0.95f, 0.85f, 0.22f, 1f),
            new Color(0.95f, 0.5f, 0.74f, 1f),
            new Color(0.34f, 0.78f, 0.31f, 1f),
            new Color(0.29f, 0.76f, 0.45f, 1f),
            new Color(0.4f, 0.68f, 0.96f, 1f)
        };

        public static string GetLabel(int iconId)
        {
            int safeIndex = Mathf.Abs(iconId) % IconLabels.Length;
            return IconLabels[safeIndex];
        }

        public static Color GetColor(int iconId)
        {
            int safeIndex = Mathf.Abs(iconId) % IconColors.Length;
            return IconColors[safeIndex];
        }
    }
}
