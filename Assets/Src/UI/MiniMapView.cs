using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private RectTransform containerRect;
    [SerializeField] private RectTransform mapImage;
    [SerializeField] private RectTransform markerPrefab;
    [SerializeField] private RectTransform markerContainer;
    [SerializeField] private Canvas canvas;

    [Header("Sizes")]
    [SerializeField] private Vector2 normalSize = new Vector2(200, 200);
    [SerializeField] private Vector2 hoverSize = new Vector2(400, 300);
    [SerializeField] private Vector2 expandedSize = new Vector2(800, 600);

    [Header("Settings")]
    [SerializeField] private float sizeLerpSpeed = 10f;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3f;

    private Vector2 _targetSize;
    private bool _isExpanded = false;
    private bool _isDragging = false;

    private void Awake()
    {
        _targetSize = normalSize;
        containerRect.sizeDelta = _targetSize;
    }

    private void Update()
    {
        containerRect.sizeDelta = Vector2.Lerp(containerRect.sizeDelta, _targetSize, Time.deltaTime * sizeLerpSpeed);

        if (_isExpanded && IsMouseInside())
        {
            HandlePan();
            HandleZoom();
        }
        
        if (!_isDragging)
            ClampBackSmoothly();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isExpanded)
            _targetSize = hoverSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isExpanded)
            _targetSize = normalSize;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isExpanded = !_isExpanded;
            _targetSize = _isExpanded ? expandedSize : normalSize;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _isExpanded)
        {
            PlaceMarker(eventData);
        }
    }

    private void PlaceMarker(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImage, eventData.position, canvas.worldCamera, out Vector2 localPoint))
        {
            RectTransform marker = Instantiate(markerPrefab, markerContainer);
            marker.anchoredPosition = localPoint;
        }
    }
    
    private bool IsMouseInside()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(containerRect, Input.mousePosition, canvas.worldCamera);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            Vector3 newScale = mapImage.localScale + Vector3.one * scroll;
            newScale = Vector3.Max(newScale, Vector3.one * minZoom);
            newScale = Vector3.Min(newScale, Vector3.one * maxZoom);
            mapImage.localScale = newScale;
        }
    }

    private void HandlePan()
    {
        if (!_isExpanded) return;

        if (Input.GetMouseButtonDown(0))
            _isDragging = true;
        if (Input.GetMouseButtonUp(0))
            _isDragging = false;

        if (_isDragging)
        {
            Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            mapImage.anchoredPosition += (Vector2)delta * 10f;
        }
    }

    
    private void ClampBackSmoothly()
    {
        Vector2 containerSize = containerRect.rect.size;
        Vector2 mapSize = Vector2.Scale(mapImage.rect.size, mapImage.localScale);

        Vector2 maxOffset = (mapSize - containerSize) / 2f;

        Vector2 pos = mapImage.anchoredPosition;

        float smoothing = 5f * Time.deltaTime;

        float targetX = Mathf.Clamp(pos.x, -maxOffset.x, maxOffset.x);
        float targetY = Mathf.Clamp(pos.y, -maxOffset.y, maxOffset.y);

        pos.x = Mathf.Lerp(pos.x, targetX, smoothing);
        pos.y = Mathf.Lerp(pos.y, targetY, smoothing);

        mapImage.anchoredPosition = pos;
    }
}
