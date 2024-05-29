using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StardustBar : MonoBehaviour
{
    public Slider slider;

    public Image image;
    public Image dangerTint;

    public Color defaultColor;
    public Color dangerColor;

    public bool IsReversed = false;

    private void Start()
    {
        slider = GetComponent<Slider>();

        defaultColor = image.color;

        slider.maxValue = Level1Controller.maxStardustMeter - 500;
        slider.value = 0;
    }

    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, Level1Controller.stardustMeter - 450, 0.4f);

        if (Level1Controller.stardustMeter < 500)
        {
            image.color = InterpolateColor(image.color, dangerColor, 0.2f);

            if (IsReversed)
            {
                dangerColor = InterpolateAlpha(dangerColor, 0.5f, 0.1f);
                IsReversed = dangerColor.a <= 0.45f;
            }
            else
            {
                dangerColor = InterpolateAlpha(dangerColor, 0.1f, 0.1f);
                IsReversed = dangerColor.a <= 0.15f;
            }

            dangerTint.color = InterpolateAlpha(dangerTint.color, 0.08f, 0.2f);
        }

        else
        {
            image.color = InterpolateColor(image.color, defaultColor, 0.1f);
            dangerTint.color = InterpolateAlpha(dangerTint.color, 0f, 0.1f);
        }
    }

    /// <summary>
    /// Returns a color with linear interpolation between the given color and the target color at the given rate.
    /// </summary>
    /// <param name="originalColor"></param>
    /// <param name="newColor"></param>
    /// <param name="transitionRate"></param>
    /// <returns></returns>
    Color InterpolateColor(Color originalColor, Color newColor, float transitionRate)
    {
        float[] originalValues = {originalColor.r, originalColor.g, originalColor.b, originalColor.a};
        float[] newValues = { newColor.r, newColor.g, newColor.b, newColor.a };
        
        for (int i = 0; i < originalValues.Length; i++)
        {
            originalValues[i] = Mathf.Lerp(originalValues[i], newValues[i], transitionRate);
        }

        return new Color(originalValues[0], originalValues[1], originalValues[2], originalValues[3]);
    }

    /// <summary>
    /// Returns the color with linear interpolation between the given alpha and the target alpha at the given rate.
    /// </summary>
    /// <param name="originalColor"></param>
    /// <param name="newAlpha"></param>
    /// <param name="transitionRate"></param>
    /// <returns></returns>
    Color InterpolateAlpha(Color originalColor, float newAlpha, float transitionRate)
    {
        float originalAlpha = originalColor.a;  

        return new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalAlpha, newAlpha, transitionRate)); 
    }

}
