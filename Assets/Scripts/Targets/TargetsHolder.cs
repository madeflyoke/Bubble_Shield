using System;
using EasyButtons;
using UnityEngine;

namespace Targets
{
    public class TargetsHolder : MonoBehaviour
    {
        [SerializeField] private Transform _rootParent;

        private void Start()
        {
            SetCorrectedScale();
        }

        /// <summary>
        /// Parent canvas with Camera screen space has divided small scale (~0.005 usually), so targets spawned inside this holder
        /// will be also small scaled. To be sure that they're same scale as in worlds coordinates, scale corrected to be "==1" by combining
        /// all parents scales and dividing this scale by them (like counter-scale)
        /// </summary>
        
        private void SetCorrectedScale()
        {
            int iterateCount = 30; //safety purpose
            Transform iteratingTr = transform;

            Vector3 allParentsScale = Vector3.zero;
            while (iteratingTr.parent != null && iterateCount>0)
            {
                iterateCount--;
                allParentsScale = Vector3.Scale(iteratingTr.localScale, iteratingTr.parent.localScale);
                if (iteratingTr.parent == _rootParent)
                {
                    transform.localScale = new Vector3(
                        transform.localScale.x / allParentsScale.x,
                        transform.localScale.y / allParentsScale.y,
                        transform.localScale.z / allParentsScale.z);
                    return;
                }
                iteratingTr = iteratingTr.parent;
            }
            
            Debug.LogError("No root parent match!");
        }
    }
}
