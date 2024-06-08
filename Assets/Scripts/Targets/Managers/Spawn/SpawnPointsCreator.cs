using System.Collections.Generic;
using UnityEngine;

namespace Targets.Managers.Spawn
{
    public class SpawnPointsCreator
    {
        private readonly float _height;
        private readonly float _sidePaddings;
        private readonly int _pointsCount;
        private readonly RectTransform _parent;
        
        public SpawnPointsCreator(float height, float sidePaddings, int pointsCount, RectTransform parent)
        {
            _height = height;
            _sidePaddings = sidePaddings;
            _pointsCount = pointsCount;
            _parent = parent;
        }
        
        public List<Vector3> CreateSpawnPoints(out Vector3 calculatedTargetScale)
        {
            var cam = Camera.main;
            
            var spawnPoints = new List<Vector3>();
            
            var minX = cam.ViewportToWorldPoint(new Vector3(0, 0f));
            var maxX = cam.ViewportToWorldPoint(new Vector3(1, 0f));
            
            var leftPos = new Vector3(minX.x, _height, 0f);
            var rightPos = new Vector3(maxX.x, _height, 0f);
            var step = (Mathf.Abs(rightPos.x - leftPos.x)-_sidePaddings*2f) / (_pointsCount-1);
            var minSize = Mathf.Min(_sidePaddings * 2f, step);
            
            calculatedTargetScale = Vector3.one*minSize * 0.9f;
            
            for (int i = 0; i < _pointsCount; i++)
            {
                var pos = leftPos+ Vector3.right*step * i+Vector3.right*_sidePaddings;
                pos.z = _parent.transform.position.z;
                spawnPoints.Add(pos);
            }

            return spawnPoints;
        }
    }
}
