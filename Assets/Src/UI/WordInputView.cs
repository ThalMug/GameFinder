using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WordInputView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public TMP_InputField inputField;

    public static event System.Action<string> OnWordSubmitted;

    private Vector3 baseScale;
    private Vector3 targetScale;
    private float animationSpeed = 10f;
    private float scaleFactor = 1.5f;
    private Transform visualTarget;

    private void Awake()
    {
        baseScale = Vector3.one;
        targetScale = baseScale;

        visualTarget = inputField.targetGraphic.transform;
        inputField.onSubmit.AddListener(HandleSubmit);
    }

    private void Update()
    {
        if (visualTarget != null)
            visualTarget.localScale = Vector3.Lerp(visualTarget.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = baseScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = baseScale;
    }

    private void HandleSubmit(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        
        OnWordSubmitted?.Invoke(value.Trim());
    }

    public void Show()
    {
        gameObject.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}