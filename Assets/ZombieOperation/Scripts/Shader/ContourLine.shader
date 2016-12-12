Shader "Custom/ContourLine"
{
	Properties
	{
		_Color("Color", Color) = (0, 0, 0, 0)
		_LineSize("LineSize", Float) = 0.01
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		LOD 100
		Tags{ "LightMode" = "Always" }
		Cull Front
		ZWrite On
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			float _LineSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float3 offset = TransformViewToProjection(norm);
				o.vertex.xy += offset * o.vertex.z * _LineSize;

				return o;
			}
			
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}
