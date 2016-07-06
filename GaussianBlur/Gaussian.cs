using UnityEngine;
using System.Collections;

public class Gaussian : PostEffectsBase {
	public Shader gaussianBlurShader;
	Material gaussianBlurMaterial;

	public Material material {  
		get {
			gaussianBlurMaterial = CheckShaderAndCreateMaterial(gaussianBlurShader, gaussianBlurMaterial);
			return gaussianBlurMaterial;
		} 
	}

	[Range(0,4)]
	public int interations = 3;

	[Range(0.2f,3.0f)]
	public float blurSpeed = 0.6f;

	[Range(1,8)]
	public int downSample = 2;

	void OnRenderImage(RenderTexture src,RenderTexture dest)
	{
		int rtH = src.height/downSample;
		int rtW = src.width/downSample;

		RenderTexture buffer0 = RenderTexture.GetTemporary(rtW,rtH,0);
		buffer0.filterMode = FilterMode.Bilinear;

		Graphics.Blit(src,buffer0);

		for (int i = 0; i < interations; i++) {
			material.SetFloat("_BlurSize",1.0f + i * blurSpeed);
			RenderTexture buffer1 = RenderTexture.GetTemporary(rtW,rtH,0);
			//vertical
			Graphics.Blit(buffer0,buffer1,material,0);

			RenderTexture.ReleaseTemporary(buffer0);
			buffer0 = buffer1;
			buffer1 = RenderTexture.GetTemporary(rtW,rtH,0);
			Graphics.Blit(buffer0,buffer1,material,1);

			RenderTexture.ReleaseTemporary(buffer0);
			buffer0 = buffer1;
		}
		Graphics.Blit(buffer0,dest);
		RenderTexture.ReleaseTemporary(buffer0);
	}
}
