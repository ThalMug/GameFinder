using UnityEngine;
using UnityEngine.EventSystems;

namespace Src.Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] public float speed = 3.5f;
        [SerializeField] private float zoomAmplitude = 2f;
        [SerializeField] private MiniMapView miniMap;
    
        private float _x;
        private float _y;
    
        private bool _isMousePressed = false;
    
        void Update()
        {
            HandleMouseInput();
            HandleZoom();
        }

        private void HandleMouseInput()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (BlockGameInputByMiniMap())
                return;
            
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                _isMousePressed = true;

            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && _isMousePressed)
                UpdateRotation();

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                _isMousePressed = false;
        }

        private void HandleZoom()
        {
            if (BlockGameInputByMiniMap())
                return;

            float fov = mainCamera.fieldOfView;
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel > 0f)
            {
                fov -= zoomAmplitude;
                mainCamera.fieldOfView = fov;
            }
            else if (wheel < 0f)
            {
                fov += zoomAmplitude;
                mainCamera.fieldOfView = Mathf.Min(fov, 60f);
            }

        }

        private void UpdateRotation()
        {
            _x += Input.GetAxis("Mouse Y") * speed;
            _x = Mathf.Clamp(_x, -80f, 80f);
        
            _y -= Input.GetAxis("Mouse X") * speed;
            transform.rotation = Quaternion.Euler(_x, _y, 0);
        }
        
        private bool BlockGameInputByMiniMap()
        {
            return miniMap != null && (miniMap.isExpanded || miniMap.isPointerInsideMiniMap);
        }
    }
}