﻿
Shader "AlpacaSound/RetroPixelPro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "" {}
        _Colormap ("Colormap", 3D) = "" {}
        _Palette ("Palette", 2D) = "" {}
        _BlueNoise ("BlueNoise", 2D) = "" {}
        _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
        _Dither ("Dither", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma target 3.0
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
            sampler3D _Colormap;
            float4 _Colormap_TexelSize;
            sampler2D _Palette;
            sampler2D _BlueNoise;
            float4 _BlueNoise_TexelSize;
            fixed _Opacity;
            fixed _Dither;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 inputColor = tex2D(_MainTex, i.uv);
                inputColor = saturate(inputColor);

                //
                // colormap lookup
                //
                int colorsteps = _Colormap_TexelSize.z;
                fixed4 colorInColormap = tex3D(_Colormap, inputColor.rgb * ((colorsteps - 1.0) / colorsteps));

                //
                // dither
                //
                fixed paletteIndex;
                fixed blend = colorInColormap.b;
                fixed blueNoiseSample = tex2D(_BlueNoise, i.vertex.xy / _BlueNoise_TexelSize.z).r;
                if (blend + _Dither - 1  >= blueNoiseSample)
                //if (blend * _Dither  > blueNoiseSample)
                {
                    paletteIndex = colorInColormap.g;
                }
                else
                {
                    paletteIndex = colorInColormap.r;
                }

                //
                // palette lookup
                //
                fixed4 result = tex2D(_Palette, fixed2(paletteIndex, 0));

                //
                // opacity
                //
                result = lerp(inputColor, result, _Opacity);

                return result;
            }


        ENDCG
        }

    }

}
