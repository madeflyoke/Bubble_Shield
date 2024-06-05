using System;
using UI.Screens;
using UniRx;
using UnityEngine;
using Zenject;

public class TargetsBlade : MonoBehaviour
{
    [Inject] private ScreensController _screensController;
    
    public static event Action<Collider2D> TargetTouched;
    
    [SerializeField] private Collider2D _triggerCollider;
    [SerializeField] private TrailRenderer _trail;
    private Camera _cam;
    private Vector2 _previousPos;
    private IDisposable _enableChecker;

    private void Awake()
    {
        _cam = Camera.main;
        Disable();
        
        _enableChecker = _screensController.GameplayScreen.ObserveEveryValueChanged(x=>x.IsFocused)
            .Subscribe(OnGameplayScreenFocusChanged).AddTo(this);
    }

    private void OnGameplayScreenFocusChanged(bool isFocused)
    {
        if (isFocused)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }
    
    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        _trail.Clear();
        _triggerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TargetTouched?.Invoke(other);
    }
    
    private void Update()
    {
        if (Input.touchSupported)
        {
           UpdatePositionByTouch();
        }
        else
        {
            UpdatePositionByMouse();
        }
        HandleCollider();
    }
    
    private void UpdatePositionByTouch()
    {
        if(Input.touchCount>0)
        {
            var touchPhase = Input.GetTouch(0).phase;
            if (touchPhase==TouchPhase.Began)
            {
                SetPosition(Input.GetTouch(0).position);
                EnableTrail();
            }
            else if (touchPhase == TouchPhase.Ended)
            {
                DisableTrail();
            }
            else
            {
                SetPosition(Input.GetTouch(0).position);
            }
        }
    }
    
    private void UpdatePositionByMouse()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetPosition(Input.mousePosition);
            EnableTrail();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DisableTrail();
        }
        else if(Input.GetKey(KeyCode.Mouse0))
        {
            SetPosition(Input.mousePosition);
        }
    }
    
    private void EnableTrail()
    {
        _trail.Clear();
        _trail.enabled = true;
    }

    private void DisableTrail()
    {
        _trail.Clear();
        _trail.enabled = false;
    }

    private void SetPosition(Vector3 screenPos)
    {
        var pos =_cam.ScreenToWorldPoint(screenPos);
        transform.position = pos;
    }

    private void HandleCollider()
    {
        _triggerCollider.enabled = Vector2.Distance(transform.position, _previousPos)>=0.03f && _trail.positionCount>0;
        _previousPos = transform.position;
    }
}
