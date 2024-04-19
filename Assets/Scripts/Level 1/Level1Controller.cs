using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Level1Controller : MonoBehaviour
{
    public static float borderDistance = 10f;

    public static float stardustMeter = 500f;
    public static float maxStardustMeter = 2000f;
    float stardustRatio;

    public int spawnCount = 5;
    public GameObject stardustLine;
    public static Material skyboxMaterial;
    public static Light lanternLight;

    public static List<GameObject> meteors = new List<GameObject>();
    public static List<GameObject> comets = new List<GameObject>();
    void Start()
    {
        lanternLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>();
        skyboxMaterial = RenderSettings.skybox;

        stardustMeter = 500f;
        stardustRatio = stardustMeter / maxStardustMeter;

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(stardustLine, Vector3.zero, Quaternion.identity);
        }
        
        SetSkyboxLightness(stardustRatio);
    }

    void Update()
    {
        stardustRatio = stardustMeter / maxStardustMeter;
        if (stardustMeter % 100 == 0)
        {
            StartCoroutine(ChangeSkyboxLightness(stardustRatio, stardustRatio / 100));
        }
        StartCoroutine(ChangeLightRange(20 * stardustRatio, stardustRatio / 10));
    }

    public static IEnumerator ChangeLightRange(float targetValue, float changeValue)
    {
        float range = lanternLight.range;
        if (range < targetValue)
        {
            for (float i = range; range <= targetValue; i += Time.deltaTime)
            {
                range += changeValue;
                lanternLight.range = range;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (float i = range; range > targetValue; i += Time.deltaTime)
            {
                range -= changeValue;
                lanternLight.range = range;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    public static IEnumerator ChangeSkyboxLightness(float targetValue, float changeValue)
    {
        float h = 0f;
        float s = 0f;
        float v = 0f;

        UnityEngine.Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));

        UnityEngine.Color.RGBToHSV(color, out h, out s, out v);
        if (v < targetValue)
        {
            for (float i = v; v <= targetValue; i += Time.deltaTime)
            {
                v += changeValue;
                color = UnityEngine.Color.HSVToRGB(h, s, v);
                skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
                yield return new WaitForSeconds(0.01f);
            }
        }

        else
        {
            for (float i = v; v > targetValue; i += Time.deltaTime)
            {
                v -= changeValue;
                color = UnityEngine.Color.HSVToRGB(h, s, v);
                skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
                yield return new WaitForSeconds(0.01f);
            }
        }

    }
    public static void SetSkyboxLightness(float targetValue)
    {
        float h = 0f;
        float s = 0f;
        float v = 0f;

        UnityEngine.Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));

        UnityEngine.Color.RGBToHSV(color, out h, out s, out v);

        v = targetValue;
        color = UnityEngine.Color.HSVToRGB(h, s, v);
        skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
    }
}
