using System;
using Src.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private RectTransform containerRect;
    [SerializeField] private RectTransform mapImage;
    [SerializeField] private RectTransform markerPrefab;
    [SerializeField] private RectTransform markerContainer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;

    [Header("Sizes")]
    [SerializeField] private Vector2 normalSize = new Vector2(200, 200);
    [SerializeField] private Vector2 hoverSize = new Vector2(400, 300);
    [SerializeField] private Vector2 expandedSize = new Vector2(800, 600);

    [Header("Settings")]
    [SerializeField] private float sizeLerpSpeed = 10f;
    [SerializeField] private float panSpeed = 100f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3f;

    [SerializeField] private Button bottomButton;

    public static event Action OnPositionSelected;
    public bool isExpanded => _isExpanded;
    public bool isPointerInsideMiniMap =>
        RectTransformUtility.RectangleContainsScreenPoint(containerRect, Input.mousePosition, canvas.worldCamera);

    
    private Vector2 _targetSize;
    private bool _isExpanded;
    private bool _isDragging;
    private RectTransform _markerInstance;
    private Vector2? _savedNorm;

    private void Awake()
    {
        _targetSize = normalSize;
        containerRect.sizeDelta = _targetSize;
        EnsureMinZoomAndClamp();
        bottomButton = GetComponentInChildren<Button>();
        bottomButton.onClick.AddListener(OnSubmit);
    }

    private void Update()
    {
        containerRect.sizeDelta = Vector2.Lerp(containerRect.sizeDelta, _targetSize, Time.deltaTime * sizeLerpSpeed);

        if (_isExpanded && IsMouseInside())
        {
            HandlePan();
            HandleZoom();
        }

        if (_isExpanded && Input.GetMouseButtonDown(0) && !IsMouseInside())
        {
            _isExpanded = false;
            _targetSize = normalSize;
            if (bottomButton != null) bottomButton.enabled = false;
        }
        
        if (Input.GetMouseButtonUp(0))
            _isDragging = false;

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
        if (eventData.button == PointerEventData.InputButton.Left && !_isExpanded && IsMouseInside())
        {
            _isExpanded = true;
            _targetSize = expandedSize;
            if (bottomButton != null) bottomButton.enabled = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _isExpanded)
        {
            PlaceMarker(eventData);
        }
    }

    public void OnSubmit()
    {
        OnPositionSelected?.Invoke();
    }

    private void PlaceMarker(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mapImage, eventData.position, canvas.worldCamera, out var localPoint))
        {
            if (_markerInstance == null)
                _markerInstance = Instantiate(markerPrefab, markerContainer);

            _markerInstance.anchorMin = _markerInstance.anchorMax = new Vector2(0.5f, 0.5f);
            _markerInstance.pivot = new Vector2(0.5f, 0.5f);
            _markerInstance.localScale = Vector3.one;
            _markerInstance.anchoredPosition = localPoint;

            _savedNorm = LocalToNorm(localPoint);
        }
    }
    
    private bool IsMouseInside()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(containerRect, Input.mousePosition, canvas.worldCamera);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;

        float target = mapImage.localScale.x + scroll;
        float minCover = GetMinZoomToCover();
        target = Mathf.Clamp(target, minCover, maxZoom);

        mapImage.localScale = Vector3.one * target;

        ClampBackSmoothly();
        if (_savedNorm.HasValue) ApplyMarkerFromNormalized(_savedNorm.Value);
    }

    
    private Vector2 LocalToNorm(Vector2 local)
    {
        Vector2 sz = mapImage.rect.size;
        return new Vector2(local.x / sz.x + 0.5f, local.y / sz.y + 0.5f);
    }

    private Vector2 NormToLocal(Vector2 norm)
    {
        Vector2 sz = mapImage.rect.size;
        return new Vector2((norm.x - 0.5f) * sz.x, (norm.y - 0.5f) * sz.y);
    }

    private void HandlePan()
    {
        if (!_isExpanded) return;

        if (Input.GetMouseButtonDown(0) && IsMouseInside())
            _isDragging = true;

        if (_isDragging)
        {
            Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            mapImage.anchoredPosition += (Vector2)delta * panSpeed;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        EnsureMinZoomAndClamp();
    }

    private void ApplyMarkerFromNormalized(Vector2 norm)
    {
        Vector2 local = NormToLocal(norm);

        if (_markerInstance == null)
            _markerInstance = Instantiate(markerPrefab, markerContainer);

        _markerInstance.anchorMin = _markerInstance.anchorMax = new Vector2(0.5f, 0.5f);
        _markerInstance.pivot = new Vector2(0.5f, 0.5f);
        _markerInstance.localScale = Vector3.one;
        _markerInstance.anchoredPosition = local;
    }
    
    private float GetMinZoomToCover()
    {
        Vector2 containerSize = containerRect.rect.size;
        Vector2 baseMapSize   = mapImage.rect.size;

        if (baseMapSize.x <= 0f || baseMapSize.y <= 0f) return minZoom;

        float sx = containerSize.x / baseMapSize.x;
        float sy = containerSize.y / baseMapSize.y;

        return Mathf.Max(sx, sy, minZoom);
    }

    private void EnsureMinZoomAndClamp()
    {
        float minCover = GetMinZoomToCover();
        float clamped  = Mathf.Clamp(mapImage.localScale.x, minCover, maxZoom);
        mapImage.localScale = Vector3.one * clamped;

        ClampBackSmoothly();
        if (_savedNorm.HasValue) ApplyMarkerFromNormalized(_savedNorm.Value);
    }
    
    public void Show(GameSequenceData data)
    {
        image.sprite = data.mapSprite;
        gameObject.SetActive(true);
        _markerInstance = null;
    }

    public void Hide()
    {
        _markerInstance = null;
        gameObject.SetActive(false);
    }

    
    private void ClampBackSmoothly()
    {
        Vector2 containerSize = containerRect.rect.size;
        Vector2 mapSize = Vector2.Scale(mapImage.rect.size, mapImage.localScale);

        Vector2 maxOffset = (mapSize - containerSize) / 2f;

        maxOffset.x = Mathf.Max(0f, maxOffset.x);
        maxOffset.y = Mathf.Max(0f, maxOffset.y);

        Vector2 pos = mapImage.anchoredPosition;
        float smoothing = 5f * Time.deltaTime;

        float targetX = Mathf.Clamp(pos.x, -maxOffset.x, maxOffset.x);
        float targetY = Mathf.Clamp(pos.y, -maxOffset.y, maxOffset.y);

        pos.x = Mathf.Lerp(pos.x, targetX, smoothing);
        pos.y = Mathf.Lerp(pos.y, targetY, smoothing);

        mapImage.anchoredPosition = pos;
    }
}
