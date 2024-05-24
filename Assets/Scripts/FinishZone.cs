using System;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public event Action<Collider2D> TargetTriggeredFinish;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        TargetTriggeredFinish?.Invoke(col);
    }
}
