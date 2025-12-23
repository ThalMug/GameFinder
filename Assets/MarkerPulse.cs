using UnityEngine;
using UnityEngine.UI;

public class MarkerStylizedController : MonoBehaviour
{
    public Color startColor = Color.green;
    public Color endColor = Color.red;
    public float pulseSpeed = 1f;

    private Material mat;

    void Awake()
    {
        Image img = GetComponent<Image>();
        mat = Instantiate(img.material);
        img.material = mat;
    }

    void Update()
    {
        // Pulse oscillant 0→1
        float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        mat.SetFloat("_Pulse", pulse);

        // Couleur dynamique (optionnel)
        mat.SetColor("_Color", Color.Lerp(startColor, endColor, pulse));
    }
}