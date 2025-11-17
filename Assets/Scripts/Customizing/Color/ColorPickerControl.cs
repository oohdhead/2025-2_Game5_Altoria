using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerControl : MonoBehaviour
{
    public float CurrenrHue, CurrentSat, CurrentVal;

    [SerializeField] RawImage hueImg, satValImg, outputImg;
    [SerializeField] Slider hueSlider;
    [SerializeField] TMP_InputField hexInputField;

    [SerializeField] MeshRenderer changeThisColor;

    Texture2D hueTexture, svTexture, outputTexture;

    void Start()
    {
        CreateHueImage();
        CreatSVImage();
        CreateOutputImage();
        UpdateOutputImage();
    }

    void CreateHueImage()
    {
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for(int i = 0; i < hueTexture.height; i++)
        {
            hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1f, 0.05f));
        }   

        hueTexture.Apply();
        CurrenrHue = 0f;

        hueImg.texture = hueTexture;
    }

    void CreatSVImage()
    {
        svTexture = new Texture2D(16, 16);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SatValTexture";

        for (int y = 0; y < svTexture.height; y++)
        {
            for(int x = 0; x < svTexture.width; x++)
            {

                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    CurrenrHue, (float)x / svTexture.width, (float)y / svTexture.height));
            }
        }

        svTexture.Apply();
        CurrentSat = 0f;
        CurrentVal = 0f;

        satValImg.texture = svTexture;
    }

    void CreateOutputImage()
    {
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(CurrenrHue, CurrentSat, CurrentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        outputImg.texture = outputTexture;
    }

    void UpdateOutputImage()
    {
        Color currentColor = Color.HSVToRGB(CurrenrHue, CurrentSat, CurrentVal);

        for (int i = 0; i < outputTexture.height; i++)
        {
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        changeThisColor.material.SetColor("_BaseColor", currentColor);
    }

    public void SetSV(float S, float V)
    {
        CurrenrHue = hueSlider.value;

        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {

                svTexture.SetPixel(x, y, Color.HSVToRGB(
                    CurrenrHue, (float)x / svTexture.width, (float)y / svTexture.height));
            }
        }

        svTexture.Apply();
        UpdateOutputImage();
    }
}

