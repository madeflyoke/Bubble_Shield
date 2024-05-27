using System.Collections.Generic;
using UnityEngine;

namespace Targets.Managers.Spawn
{
    public class SpawnPointsCreator : MonoBehaviour
    {
        [SerializeField] private float _spawnHeight =6f;
        [SerializeField] private float _sidesSpawnPadding=0.75f;
        [SerializeField] private RectTransform _spawnPointsHolder;

        public List<RectTransform> CreateSpawnPoints(int targetsCount, out Vector3 targetScale)
        {
            var spawnPoints = new List<RectTransform>();
            
            var minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f));
            var maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0f));
            
            var leftPos = new Vector3(minX.x, _spawnHeight, 0f);
            var rightPos = new Vector3(maxX.x, _spawnHeight, 0f);
            var step = (Mathf.Abs(rightPos.x - leftPos.x)-_sidesSpawnPadding*2f) / (targetsCount-1);
            var minSize = Mathf.Min(_sidesSpawnPadding * 2f, step);
            
            targetScale = Vector3.one*minSize * 0.9f;
            
            for (int i = 0; i < targetsCount; i++)
            {
                var pos = leftPos+ Vector3.right*step * i+Vector3.right*_sidesSpawnPadding;
                pos.z = _spawnPointsHolder.transform.position.z;
                var go = new GameObject();
                go.transform.position = pos;
                go.name = "SpawnPoint " + i;
                var rt = go.AddComponent<RectTransform>();
                go.transform.SetParent(_spawnPointsHolder);
                spawnPoints.Add(rt);
            }

            return spawnPoints;
        }
    }
}
