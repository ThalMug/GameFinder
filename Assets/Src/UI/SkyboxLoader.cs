using UnityEngine;

public static class SkyboxLoader
{
    public static void LoadPanoramicSkybox(Texture2D panoramicTexture)
    {
        if (panoramicTexture == null)
        {
            Debug.LogError("Skybox texture is null");
            return;
        }

        Material skyboxMaterial = new Material(Shader.Find("Skybox/Panoramic"));

        skyboxMaterial.SetTexture("_MainTex", panoramicTexture);
        skyboxMaterial.SetFloat("_Mapping", 0);
        skyboxMaterial.SetFloat("_ImageType", 0);
        skyboxMaterial.SetFloat("_Exposure", 1.0f);

        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
    }
}