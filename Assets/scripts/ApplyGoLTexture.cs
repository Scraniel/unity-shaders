using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGoLTexture : MonoBehaviour
{
    public ComputeShader shader;
    public Texture2D currentTexture;
    public Material currentMat;

    void Start() 
    {
        StartCoroutine(ApplyTexture());
    }

    IEnumerator ApplyTexture() {
        for(;;)
        {
            currentTexture = Compute();

            if(currentMat != null)
            {
                currentMat.SetTexture("_MainTex", currentTexture);
            }

            yield return new WaitForSeconds(0.1f);

        }
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public Texture2D Compute()
    {
        RenderTexture result = new RenderTexture(currentTexture.width, currentTexture.height, 24);
        int kernelHandle = shader.FindKernel("CSMain");
        result.enableRandomWrite = true;
        result.Create();

        shader.SetTexture(kernelHandle, "Result", result);
        shader.SetTexture(kernelHandle, "ImageInput", currentTexture);
        shader.Dispatch(kernelHandle, 512/8 , 1024 / 8, 1);

        RenderTexture.active = result;
        
        return toTexture2D(result);
    }
}
