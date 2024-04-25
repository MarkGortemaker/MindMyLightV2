using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level1Controller : MonoBehaviour
{
    public static float borderDistance = 200f;
    public static float safeZoneDistance = 30f;
    public static float dangerZoneDistance = 150f;

    public static float stardustMeter;
    public static float maxStardustMeter = 2000f;
    public static float collectedStardust = 0f;  //win condition: get this to 7500 (a star in the hud fills up by 1/5 for each 1500 collected)  
    public static float stardustRatio;

    public static float h, s, v = 0f;

    public static int cometCurrentCount = 0;
    public static int cometSpawnCount = 3;
    public int meteorSpawnCount = 20;
    public int stardustSpawnCount = 20;

    public GameObject stardustLine;
    public GameObject meteor;
    public GameObject comet;

    public static Transform playerTransform;
    public static Transform starTransform;

    public static Material skyboxMaterial;
    public static Light lanternLight;

    void Start()
    {
        lanternLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        skyboxMaterial = RenderSettings.skybox;
        starTransform = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>();

        stardustMeter = 500f;
        stardustRatio = stardustMeter / maxStardustMeter;

        for (int i = 0; i < stardustSpawnCount; i++) 
        {
            RadiusSpawn.SpawnInCircleArea(stardustLine, 1.1f * safeZoneDistance, dangerZoneDistance, starTransform.position);
        }

        for (int i = 0; i < 2 * stardustSpawnCount; i++)
        {
            RadiusSpawn.SpawnInCircleArea(stardustLine, dangerZoneDistance, 0.9f * borderDistance, starTransform.position);
        }

        for (int i = 0; i < meteorSpawnCount; i++)
        {
            RadiusSpawn.SpawnInCircleArea(meteor, 1.1f * safeZoneDistance, dangerZoneDistance, starTransform.position);
        }

        for (int i = 0; i < 2 * meteorSpawnCount; i++)
        {
            RadiusSpawn.SpawnInCircleArea(meteor, dangerZoneDistance, 0.9f * borderDistance, starTransform.position);
        }

        InvokeRepeating("SpawnComet", 0f, 60f);

        SetSkyboxLightness(stardustRatio);
        SetLightRange(20 * stardustRatio);
    }

    private void FixedUpdate()
    {
        SpawnComet();
    }

    void SpawnComet()
    {
        float distance = (playerTransform.position - starTransform.position).magnitude;

        if (cometCurrentCount < cometSpawnCount && distance > dangerZoneDistance)
        {
            Debug.Log("Spawning!!");
            RadiusSpawn.SpawnInCircleArea(comet, 30f, 40f, playerTransform.position);
            cometCurrentCount++;
        }
    }

    public static bool DespawnComet(GameObject gameObject)
    {
        if ((gameObject.transform.position - playerTransform.position).magnitude > 40f)
        {
            cometCurrentCount--;
            Destroy(gameObject);
        }

        if ((playerTransform.position - starTransform.position).magnitude <= safeZoneDistance / 2)
        {
            return true;
        }

        else
        {
            return false;
        }
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
        Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        Color.RGBToHSV(color, out h, out s, out v);

        for (float i = v; v <= targetValue; i += Time.deltaTime)
        {
            v += changeValue;
            color = Color.HSVToRGB(h, s, v);
            skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public static IEnumerator DecreaseSkyboxLightness(float targetValue, float changeValue)
    {
        Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        Color.RGBToHSV(color, out h, out s, out v);

        for (float i = v; v > targetValue; i += Time.deltaTime)
        {
            v -= changeValue;
            color = Color.HSVToRGB(h, s, v);
            skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public static void SetSkyboxLightness(float targetValue)
    {
        Color color = skyboxMaterial.GetColor(Shader.PropertyToID("_Tint"));
        Color.RGBToHSV(color, out h, out s, out v);

        v = targetValue;
        color = Color.HSVToRGB(h, s, v);
        skyboxMaterial.SetColor(Shader.PropertyToID("_Tint"), color);
    }
}
