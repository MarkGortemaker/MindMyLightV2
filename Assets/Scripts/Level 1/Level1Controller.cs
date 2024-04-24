using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level1Controller : MonoBehaviour
{
    public static float borderDistance = 50f;
    public static float safeZoneDistance = 20f;
    public static float dangerZoneDistance = 40f;

    public static Transform starTransform;

    public static float stardustMeter;
    public static float maxStardustMeter = 2000f;
    public static float collectedStardust = 0f;  //win condition: get this to 7500 (a star in the hud fills up by 1/5 for each 1500 collected)  
    public static float stardustRatio;

    public static float h, s, v = 0f;

    public int cometSpawnCount = 0;
    public int meteorSpawnCount = 10;
    public int stardustSpawnCount = 10;

    public GameObject stardustLine;
    public GameObject meteor;
    //public GameObject comet;
    public static Material skyboxMaterial;
    public static Light lanternLight;

    public static List<GameObject> meteors = new List<GameObject>();
    public static List<GameObject> comets = new List<GameObject>();
    void Start()
    {
        lanternLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>();
        skyboxMaterial = RenderSettings.skybox;
        starTransform = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>();

        stardustMeter = 2000f;
        stardustRatio = stardustMeter / maxStardustMeter;

        for (int i = 0; i < stardustSpawnCount; i++) 
        {
            RadiusSpawn.SpawnInCircleArea(stardustLine, safeZoneDistance, borderDistance, starTransform.position);
        }

        for (int i = 0; i < meteorSpawnCount; i++)
        {
            RadiusSpawn.SpawnInCircleArea(meteor, safeZoneDistance, borderDistance, starTransform.position);
        }

        /*for (int i = 0; i < cometSpawnCount; i++)
        {
            RadiusSpawn.SpawnInCircleArea(stardustLine, A, B, player.position); A and B are offscreen
        }*/

        SetSkyboxLightness(stardustRatio);
        SetLightRange(20 * stardustRatio);
    }


    public static IEnumerator IncreaseLightRange(float targetValue, float changeValue) 
    {
        float range = lanternLight.range;
        for (float i = range; range <= targetValue; i += Time.deltaTime)
        {
                range += changeValue;
                lanternLight.range = range;
                yield return new WaitForSeconds(0.01f);
        }
    }

    public static IEnumerator DecreaseLightRange(float targetValue, float changeValue)
    {
        float range = lanternLight.range;
        for (float i = range; range > targetValue; i += Time.deltaTime)
        {
            range -= changeValue;
            lanternLight.range = range;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public static void SetLightRange(float targetValue)
    {
        float range = lanternLight.range;
        range = targetValue;
        lanternLight.range = range;
    }
    public static IEnumerator IncreaseSkyboxLightness(float targetValue, float changeValue)
    {
        UnityEngine.Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        UnityEngine.Color.RGBToHSV(color, out h, out s, out v);

        for (float i = v; v <= targetValue; i += Time.deltaTime)
        {
            v += changeValue;
            color = UnityEngine.Color.HSVToRGB(h, s, v);
            skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public static IEnumerator DecreaseSkyboxLightness(float targetValue, float changeValue)
    {
        UnityEngine.Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        UnityEngine.Color.RGBToHSV(color, out h, out s, out v);

        for (float i = v; v > targetValue; i += Time.deltaTime)
        {
            v -= changeValue;
            color = UnityEngine.Color.HSVToRGB(h, s, v);
            skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public static void SetSkyboxLightness(float targetValue)
    {
        UnityEngine.Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        UnityEngine.Color.RGBToHSV(color, out h, out s, out v);

        v = targetValue;
        color = UnityEngine.Color.HSVToRGB(h, s, v);
        skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
    }
}
