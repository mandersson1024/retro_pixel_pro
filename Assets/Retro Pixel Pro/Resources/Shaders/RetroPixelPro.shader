// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "AlpacaSound/RetroPixelPro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "" {}
        _Colormap3D ("Colormap3D", 3D) = "" {}
        _Colormap2D ("Colormap2D", 2D) = "" {}
        _Palette ("Palette", 2D) = "" {}
        _Strength ("Strength", Range(0.0, 1.0)) = 1.0
        _Use3DTexture ("Int", int) = 1
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma target 2.0
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
            sampler2D _Colormap2D;
            float4 _Colormap2D_TexelSize;
            sampler2D _Palette;
            int _PaletteSize;
            float _Strength;

            int _Use3DTexture;
            sampler3D _Colormap3D;
            float4 _Colormap3D_TexelSize;


            fixed4 frag (v2f i) : SV_Target
            {
                float4 inputColor = tex2D(_MainTex, i.uv);
                inputColor = saturate(inputColor);

                int colorsteps = _Colormap3D_TexelSize.z;
                int size2D = _Colormap2D_TexelSize.z;

                int3 byteInputColor = floor(inputColor.rgb * (colorsteps - 1));
                int index = byteInputColor.r + (byteInputColor.g * colorsteps) + (byteInputColor.b * colorsteps * colorsteps);
                int2 xy = int2(round(fmod(index, size2D)), round(index / size2D));

                fixed4 colorInColormap = tex2D(_Colormap2D, saturate(float2(xy) / size2D));

                if (_Use3DTexture != 0) 
                    colorInColormap = tex3D(_Colormap3D, inputColor.rgb * ((colorsteps - 1.0) / colorsteps));

                float paletteIndex1D = colorInColormap.a;
                float2 paletteIndex2D = float2(paletteIndex1D, 0);
                fixed4 result = tex2D(_Palette, paletteIndex2D);
                fixed4 blended = lerp(inputColor, result, _Strength);
                return blended;
            }


        ENDCG
        }

    }

}
