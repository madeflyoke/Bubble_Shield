using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using Targets;
using Targets.Enums;
using Targets.Managers;
using Targets.Utility;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private RectTransform _holder;
    [SerializeField] private TargetsFactory _targetsFactory;

    [Button]
    public void Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            _targetsFactory.SetCommonSpecifications(Vector3.one*1.5f);
            _targetsFactory.SetCurrentSpecifications(new TargetStats()
            {
                Speed = 0f
            });
            var pos = Vector3.left * 2f + Vector3.right * i;
            pos.z = _holder.transform.position.z;
            _targetsFactory.CreateTarget(new TargetsFactory.TargetSpawnData()
            {
                Parent = _holder,
                Position = pos,
                Variant = TargetVariant.ENEMY
            });
        }
  
    }
}
