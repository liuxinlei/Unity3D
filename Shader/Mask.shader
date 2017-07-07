Shader "Unlit/Mask"
{
	Properties
	{
		_Radius("_Radius",Range(0,1)) = 0.5
		_PosX("_PosX",Range(0,1)) = 0.5	
		_PosY("_PosY",Range(0,1)) = 0.5
		_Alpha("_Alpha",Range(0,1)) = 1
		_MainTex("_MainTex",2D) = "white"{}
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent" 
			"IngnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

	Pass
	{
		Tags{ "LightMode" = "ForwardBase" }

		//必须加入才能实现透明效果
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		#include "UnityCG.cginc"
		#include "Lighting.cginc"  

		#pragma vertex vert
		#pragma fragment frag

		sampler2D _MainTex;
		fixed4 _MainTex_ST;
		float _Radius;
		float _PosX;
		float _PosY;
		float _Alpha;

		struct a2v
		{
			float4 position:POSITION;
			float4 texcoord:TEXCOORD0;
			fixed4 color:COLOR;
		};

		struct v2f
		{
			float4 position:SV_POSITION;
			float2 texcoord:TEXCOORD0;
			fixed4 color:COLOR;
		};

		v2f vert(a2v v)
		{
			v2f f;
			f.position = mul(UNITY_MATRIX_MVP,v.position);

			//获取该顶点下的纹理坐标
			f.texcoord = v.texcoord.xy*_MainTex_ST.xy + _MainTex_ST.zw;

			f.color = v.color;
			return f;
		}

		fixed4 frag(v2f f) :SV_Target
		{
			//获取纹理坐标下的颜色值
			fixed4 color = tex2D(_MainTex,f.texcoord);
			fixed4 outputCol = fixed4(0,0,0,_Alpha);
			float2 _Pos = float2(_PosX,_PosY);
			float len = distance(f.texcoord,_Pos);
			float rate = clamp(len/_Radius * f.color.a,0,1);
			return outputCol*rate;		
		}

		ENDCG
	}
	}

	Fallback "Diffuse"
}