// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AlpacaSound/RetroPixelPro"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "" {}
		_ColorMap ("ColorMap", 3D) = "" {}
		_Palette ("Palette", 2D) = "" {}
		_Strength ("Strength", Range(0.0, 1.0)) = 1.0
	}
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
	  		sampler3D _ColorMap;
	  		sampler2D _Palette;
	  		int _PaletteSize;
	  		float _Strength;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				fixed4 colorInColorMap = tex3D(_ColorMap, col.rgb);
				float paletteIndex1D = colorInColorMap.a;
				float2 paletteIndex2D = float2(paletteIndex1D, 0);
				fixed4 result = tex2D(_Palette, paletteIndex2D);
				fixed4 blended = lerp(col, result, _Strength);
				return blended;
			}


			ENDCG
		}
		
	}
	
}
