using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Canvas overlayCanvas;          
    [SerializeField] private RectTransform maskContainer;   
    [SerializeField] private RectTransform mapImage;        
    [SerializeField] private RectTransform markerContainer; 
    [SerializeField] private RectTransform markerPrefab;    
    [SerializeField] private Button closeButton;

    public event Action OnResultScreenClosed;

    private RectTransform _m1, _m2;

    private void Awake()
    {
        if (closeButton) closeButton.onClick.AddListener(Hide);

        if (markerContainer.parent != mapImage) markerContainer.SetParent(mapImage, false);
        markerContainer.anchorMin = Vector2.zero;
        markerContainer.anchorMax = Vector2.one;
        markerContainer.offsetMin = Vector2.zero;
        markerContainer.offsetMax = Vector2.zero;
        markerContainer.pivot = new Vector2(0.5f, 0.5f);

        gameObject.SetActive(false);
    }
    
    public void Show(Sprite mapSprite, Vector2 marker1Norm, Vector2 marker2Norm)
    {
        if (mapSprite != null)
        {
            var img = mapImage.GetComponent<Image>();
            img.sprite = mapSprite;
        }

        if (_m1 == null) _m1 = Instantiate(markerPrefab, markerContainer);
        if (_m2 == null) _m2 = Instantiate(markerPrefab, markerContainer);
        
        SetupMarker(_m1, NormToLocal(marker1Norm));
        SetupMarker(_m2, NormToLocal(marker2Norm));

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        OnResultScreenClosed?.Invoke();  
        gameObject.SetActive(false);
    }


    private void SetupMarker(RectTransform m, Vector2 local)
    {
        m.anchorMin = m.anchorMax = new Vector2(0.5f, 0.5f);
        m.pivot = new Vector2(0.5f, 0.5f);
        m.localScale = Vector3.one;
        m.anchoredPosition = ClampInsideMap(local, m);
    }

    private Vector2 NormToLocal(Vector2 norm)
    {
        Vector2 sz = mapImage.rect.size;

        float x = (norm.x - 0.5f) * sz.x;
        float y = ((1f - norm.y) - 0.5f) * sz.y;

        return new Vector2(x, y);
    }


    private Vector2 ClampInsideMap(Vector2 local, RectTransform marker)
    {
        var rect = mapImage.rect;
        var half = rect.size * 0.5f;
        var margin = marker.rect.size * 0.5f;

        local.x = Mathf.Clamp(local.x, -half.x + margin.x, half.x - margin.x);
        local.y = Mathf.Clamp(local.y, -half.y + margin.y, half.y - margin.y);
        return local;
    }
}
