using System;
using System.Collections.Generic;
using System.Linq;
using UI.Screens;
using UniRx;
using Unity.Burst;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Slicing
{
    public class TargetsSlicer : MonoBehaviour
    {
        [Inject] private ScreensController _screensController;
        
        public static event Action<GameObject> TargetSliced;

        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private GraphicRaycaster _raycaster;

        private Vector3 _prevPosition;
        private Camera _mainCam;
        
        private List<RaycastResult> _cachedRaycastResults = new();

        private void Awake()
        {
            _mainCam = Camera.main;
            
            
            Disable();
        
            _screensController.GetScreen<GameplayScreen>().ObserveEveryValueChanged(x=>x.IsFocused)
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
            EnableTrail();
            gameObject.SetActive(true);
        }
        private void Disable()
        {
            DisableTrail();
            gameObject.SetActive(false);
        }
        
        private void EnableTrail()
        {
            _trail.Clear();
            _trail.enabled = true;
        }
        private void DisableTrail()
        {
            _trail.enabled = false;
            _trail.Clear();
        }

        private void Update()
        {
            if (Input.touchSupported)
            {
                
            }
            else
            {
                HandleInputByMouse();
            }
        }

        private void HandleInputByMouse()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DisableTrail();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                SetPosition(Input.mousePosition);
                EnableTrail();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                SetPosition(Input.mousePosition);
                if (TryActivate())
                {
                    if (_trail.positionCount>0) //if frame rate too low, it can miss handle of mouse position changing,
                                                //so here are points along trail renderer, with increased density, that check all path of slice to guarantee needed slice
                    {
                        _cachedRaycastResults.Clear();
                        var trailPoses = new Vector3[_trail.positionCount];
                        _trail.GetPositions(trailPoses);
                        Vector3[] correctedTrailPoses = trailPoses.Select(x => _mainCam.WorldToScreenPoint(x)).ToArray();

                        var screenSpaceTrailPoints = InterpolateTrailPoints(correctedTrailPoses);
                        
                        for (int i = 0, length = screenSpaceTrailPoints.Length; i < length; i++)
                        {
                            CheckRaycast(screenSpaceTrailPoints[i]);
                        }
                    }
                }
            }
        }

        [BurstCompile]
        private Vector3[] InterpolateTrailPoints(Vector3[] input)
        {
            int density = 5;
            Vector3[] outputPoints = new Vector3[(input.Length-1)*(density+1)];
            int index = 0;
            for (int i = 0, length = input.Length-1; i <length ; i++)
            {
                outputPoints[index] = input[i];
                index++;
                for (int j = 1; j <= density; j++)
                {
                    float t = j / (float)(density);
                    Vector3 interpolatedPoint = Vector3.Lerp(input[i], input[i+1], t);
                    outputPoints[index] = interpolatedPoint;
                    index++;
                }
            }

            return outputPoints;
        }

        private void CheckRaycast(Vector3 pos)
        {
            var eventData = new PointerEventData(EventSystem.current); //TODO pool?
            eventData.position = pos;
            _raycaster.Raycast(eventData, _cachedRaycastResults);
            
            for (int i = 0; i < _cachedRaycastResults.Count; i++)
            {
                TargetSliced?.Invoke(_cachedRaycastResults[i].gameObject);
            }
        }
        
        private bool TryActivate()
        {
            var isActive = Vector3.Distance(transform.position, _prevPosition) >= 0.1f;
            _prevPosition = transform.position;
            return isActive;
        }

        private void SetPosition(Vector3 screenPos)
        {
            transform.position = _mainCam.ScreenToWorldPoint(screenPos);
        }
    }
}
