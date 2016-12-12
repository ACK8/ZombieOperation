Shader "Custom/Transition"
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("MainAlbedo (RGB)", 2D) = "white" {}
		_SecondTex("SecondAlbedo (RGB)", 2D) = "white" {}
		_HeightTex("HeightMap (RGB)", 2D) = "white" {}
		_TimeTransition("Time", Float) = -1.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondTex;
		sampler2D _HeightTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _TimeTransition;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float4 firstTex = tex2D (_MainTex, IN.uv_MainTex);
			float4 secondTex = tex2D(_SecondTex, IN.uv_MainTex);
			float heightTex = tex2D(_HeightTex, IN.uv_MainTex).r;

			float t = lerp(0, heightTex, _TimeTransition);

			float4 color = lerp(firstTex, secondTex, _TimeTransition) * _Color;

			o.Albedo = color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
