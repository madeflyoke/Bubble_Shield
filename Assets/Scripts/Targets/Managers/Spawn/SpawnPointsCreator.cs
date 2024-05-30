using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Targets.Managers.Spawn
{
    public class SpawnPointsCreator : MonoBehaviour
    {
        [SerializeField] private float _spawnHeight =6f;
        [SerializeField] private float _sidesSpawnPadding=0.75f;
        private Camera _mainCam;
        
        private void Awake()
        {
            _mainCam = Camera.main;
        }
        
        public List<Vector3> CreateSpawnPoints(int targetsCount, RectTransform parent, out Vector3 targetScale)
        {
            var spawnPoints = new List<Vector3>();
            
            var minX = _mainCam.ViewportToWorldPoint(new Vector3(0, 0f));
            var maxX = _mainCam.ViewportToWorldPoint(new Vector3(1, 0f));
            
            var leftPos = new Vector3(minX.x, _spawnHeight, 0f);
            var rightPos = new Vector3(maxX.x, _spawnHeight, 0f);
            var step = (Mathf.Abs(rightPos.x - leftPos.x)-_sidesSpawnPadding*2f) / (targetsCount-1);
            var minSize = Mathf.Min(_sidesSpawnPadding * 2f, step);
            
            targetScale = Vector3.one*minSize * 0.9f;
            
            for (int i = 0; i < targetsCount; i++)
            {
                var pos = leftPos+ Vector3.right*step * i+Vector3.right*_sidesSpawnPadding;
                pos.z = parent.transform.position.z;
                spawnPoints.Add(pos);
            }

            return spawnPoints;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            var minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f));
            var maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0f));
            
            var leftPos = new Vector3(minX.x, _spawnHeight, 0f);
            var rightPos = new Vector3(maxX.x, _spawnHeight, 0f);
            
            Handles.color = Color.yellow;
            var leftSpawnPadding = leftPos + Vector3.right * _sidesSpawnPadding;
            var rightSpawnPadding = rightPos - Vector3.right * _sidesSpawnPadding;
            Handles.DrawLine(leftSpawnPadding-Vector3.up, leftSpawnPadding+Vector3.up);
            Handles.DrawLine(rightSpawnPadding-Vector3.up, rightSpawnPadding+Vector3.up);
            Handles.DrawLine(leftSpawnPadding, rightSpawnPadding);
        }

#endif
    }
}
