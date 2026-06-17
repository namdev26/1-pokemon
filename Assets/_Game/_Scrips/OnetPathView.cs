using UnityEngine;

namespace Game.Onet
{
    public sealed class OnetPathView : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer lineRenderer;

        private OnetWorldCoordMapper coordMapper;
        private OnetDemoConfig demoConfig;

        public void Initialize(OnetDemoConfig config)
        {
            demoConfig = config;
            coordMapper = new OnetWorldCoordMapper(config);
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = config.pathThickness;
            lineRenderer.endWidth = config.pathThickness;
        }

        public void Clear()
        {
            lineRenderer.positionCount = 0;
        }

        public void Draw(MatchPath path, OnetDemoConfig config, Color color)
        {
            if (coordMapper == null || demoConfig != config)
            {
                Initialize(config);
            }

            if (path == null || path.Points.Count == 0)
            {
                Clear();
                return;
            }

            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.positionCount = path.Points.Count;

            for (int index = 0; index < path.Points.Count; index++)
            {
                lineRenderer.SetPosition(index, coordMapper.GetPathPosition(path.Points[index]));
            }
        }
    }
}
