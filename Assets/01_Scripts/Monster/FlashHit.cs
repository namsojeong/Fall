using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashHit : MonoBehaviour
{
    private readonly int hashEmmision = Shader.PropertyToID("_EmissionColor");

    private List<Color> colors = new List<Color>();
    private SkinnedMeshRenderer[] renderers;

    private Color32 flashColor = Color.red;

    private bool isDamage = false;
    private float flashDuration = 0.2f;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        //SetBeforeColor();
    }

    // 바꿀 색 받기 default : red
    public void SetColor(Color color)
    {
        float factor = Mathf.Pow(2, 2f);

        color.r *= factor;
        color.g *= factor;
        color.b *= factor;
        color.a *= factor;

        flashColor = color;
    }

    // 맨 처음 오브젝트의 색 저장
    private void SetBeforeColor()
    {
        Texture2D texture = new Texture2D(128, 128);

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            //colors.Add(renderer.material.mainTexture.(hashEmmision));
            renderer.material.mainTexture = texture;
            texture.SetPixel(128, 128, Color.red);
        }
        texture.Apply();
    }

    // 색바꾸기
    public void DamageEffect()
    {
        if (isDamage)
        {
            StopCoroutine(TwinkleDamageEffect());
            ChangeBeforeColor();
        }
        isDamage = true;
        StartCoroutine(TwinkleDamageEffect());
    }

    private IEnumerator TwinkleDamageEffect()
    {
        ChangeColor();
        yield return new WaitForSeconds(flashDuration);
        ChangeBeforeColor();
        isDamage = false;
    }

    private void ChangeColor()
    {

        //Texture2D texture = new Texture2D(128, 128);

        //foreach (SkinnedMeshRenderer renderer in renderers)
        //{
        //    colors.Add(renderer.material.mainTexture.(hashEmmision));
        //    renderer.material.mainTexture = texture;
        //    texture.SetPixel(128, 128, Color.red);
        //}
        //texture.Apply();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material.SetColor(hashEmmision, flashColor);
        }
    }

    private void ChangeBeforeColor()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetColor(hashEmmision, Color.white);
         //   renderers[i].material.SetColor(hashEmmision, colors[i]);
        }
    }
}
