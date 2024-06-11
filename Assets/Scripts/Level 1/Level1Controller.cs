using AudioManager.Core;
using AudioManager.Locator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level1Controller : MonoBehaviour
{
    public static float borderDistance = 200f;
    public static float safeZoneDistance = 25f;
    public static float dangerZoneDistance = 150f;

    public static float stardustMeter;
    public static float maxStardustMeter = 1500f;
    public static float collectedStardust = 0f;
    public static float stardustWinGoal = 5000f;
    public static float stardustRatio;

    public static float h, s, v = 0f;

    public static int difficulty = 1;
    public static int cometSpawnCount = 1;
    public int meteorSpawnCount = 30;
    public int stardustSpawnCount = 90;

    public GameObject meteor;
    public GameObject comet;

    public GameObject[] spawnableStardustLines;

    public static List<GameObject> stardustLines;
    public static List<GameObject> meteors;
    public static List<GameObject> comets;

    public static Transform playerTransform;
    public static Transform CameraTransform;

    public static Transform starTransform;

    public static Material skyboxMaterial;
    public static Light lanternLight;

    private IAudioManager am;

    void Start()
    {
        borderDistance = 200f;
        safeZoneDistance = 25f;
        dangerZoneDistance = 100f;

        lanternLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Light>();
        skyboxMaterial = RenderSettings.skybox;

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        starTransform = GameObject.FindGameObjectWithTag("Star").GetComponent<Transform>();

        stardustLines = new List<GameObject>();
        meteors = new List<GameObject>();
        comets = new List<GameObject>();

        difficulty = 1; 
        cometSpawnCount = 1;
        //meteorSpawnCount = 30;
        //stardustSpawnCount = 90;

        collectedStardust = 3000f; //initial is 0, set to 3000 for demo build only
        stardustMeter = 500f;
        stardustRatio = stardustMeter / maxStardustMeter;

        SpawnStardust();
        SpawnMeteor();

        InvokeRepeating("SpawnComet", 0f, 15f);

        SetSkyboxLightness(stardustRatio);
        SetLightRange(20 * stardustRatio);

        am = ServiceLocator.GetService();

        // Calling method in AudioManager

        am.Play("StarMusic");
    }

    private void FixedUpdate()
    {
        starTransform.LookAt(CameraTransform.position);

        SpawnComet();

        int progress = Mathf.FloorToInt(collectedStardust / (stardustWinGoal / 5));

        if (progress >= difficulty) //every full stardust meter sent to the star
        {
            dangerZoneDistance -= 15f;

            difficulty++;

            /* Number of comets stays the same in demo build
            if (difficulty == 4 || difficulty == 5)
            {
                cometSpawnCount++;
            }
            */

            foreach (GameObject stardust in stardustLines)
            {
                Destroy(stardust);
            }

            foreach (GameObject meteor in meteors)
            {
                Destroy(meteor);
            }

            stardustLines = new List<GameObject>();
            meteors = new List<GameObject>();

            SpawnMeteor();
            SpawnStardust();
        }
    }

    void SpawnComet()
    {
        float distance = (playerTransform.position - starTransform.position).magnitude;

        if (comets.Count < cometSpawnCount && distance > dangerZoneDistance)
        {
            Debug.Log("Spawning!!");
            comets.Add(RadiusSpawn.SpawnInCircleArea(comet, 30f, 40f, 10f, playerTransform.position));
        }
    }

    void SpawnStardust()
    {
        if (stardustLines.Count < stardustSpawnCount)
        {
            for (int i = 0; i < stardustSpawnCount / 3; i++)
            {
                stardustLines.Add(RadiusSpawn.SpawnInCircleArea(spawnableStardustLines[Random.Range(0, 2)], safeZoneDistance, dangerZoneDistance, 20f, starTransform.position));
            }

            for (int i = 0; i < stardustSpawnCount / 3; i++)
            {
                stardustLines.Add(RadiusSpawn.SpawnInCircleArea(spawnableStardustLines[Random.Range(1, 4)], dangerZoneDistance / 2, 0.9f * borderDistance, 30f, starTransform.position));
            }

            for (int i = 0; i < stardustSpawnCount / 3; i++)
            {
                stardustLines.Add(RadiusSpawn.SpawnInCircleArea(spawnableStardustLines[Random.Range(3, spawnableStardustLines.Length)], dangerZoneDistance,
                    0.9f * borderDistance, 50f, starTransform.position));
            }
        }
    }

    void SpawnMeteor()
    {
        if (meteors.Count < meteorSpawnCount)
        {
            for (int i = 0; i < meteorSpawnCount / 3; i++)
            {
                meteors.Add(RadiusSpawn.SpawnInCircleArea(meteor, 1.1f * safeZoneDistance, 0.9f * borderDistance, 20f, starTransform.position));
            }

            for (int i = 0; i < 2 * meteorSpawnCount / 3; i++)
            {
                meteors.Add(RadiusSpawn.SpawnInCircleArea(meteor, dangerZoneDistance, 0.9f * borderDistance, 20f, starTransform.position));
            }
        }
    }

    /// <summary>
    /// Destroys the GameObject if it exceeds a distance of 40 between itself and the player. Also returns a true value if the player is in the safe zone to turn the Comets around. 
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static bool DespawnComet(GameObject gameObject)
    {
        if ((gameObject.transform.position - playerTransform.position).magnitude > 40f)
        {
            comets.Remove(gameObject);
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
    public static void WinGame()
    {
        GeneralControls.canPause = false;
        LevelLoader.LoadLevel("Level1End");
    }

    public static void ShowGameOver(GameObject screen)
    {
        GeneralControls.PauseGame();
        GeneralControls.canPause = false;
        screen.SetActive(true);
    }
    void OnDisable()
    {
        am.Stop("StarMusic");
    }

}
