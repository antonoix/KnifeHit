using System.Collections.Generic;
using UnityEngine;

namespace Internal.Scripts.Infrastructure.HeroRoute
{
    public class HeroRouter : MonoBehaviour
    {
        [field: SerializeField] public RouterPoint StartPoint { get; private set; }
        [SerializeField] private RouterPoint[] points;
    
        private Queue<RouterPoint> _pointsQueue;
        private RouterPoint _currentPoint;

        public int TotalPointsCount { get; private set; }
    
        public bool CurrentPointIsReached => _currentPoint.IsReached;

        public void Initialize()
        {
            _pointsQueue = new Queue<RouterPoint>();

            foreach (var point in points) 
                _pointsQueue.Enqueue(point);

            TotalPointsCount = _pointsQueue.Count;
        }

        public bool TryGetNextPoint(out RouterPoint point)
        {
            point = null;
            if (_pointsQueue.TryDequeue(out _currentPoint))
            {
                point = _currentPoint;
                return true;
            }

            return false;
        }
    }
}
