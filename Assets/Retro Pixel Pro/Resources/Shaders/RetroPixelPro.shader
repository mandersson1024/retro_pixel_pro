
Shader "AlpacaSound/RetroPixelPro"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "" {}
        _Colormap ("Colormap", 3D) = "" {}
        _Palette ("Palette", 2D) = "" {}
        _Strength ("Strength", Range(0.0, 1.0)) = 1.0
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
            float _Strength;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 inputColor = tex2D(_MainTex, i.uv);
                inputColor = saturate(inputColor);
                int colorsteps = _Colormap_TexelSize.z;
                fixed4 colorInColormap = tex3D(_Colormap, inputColor.rgb * ((colorsteps - 1.0) / colorsteps));
                fixed paletteIndex1D = colorInColormap.a;
                fixed4 result = tex2D(_Palette, fixed2(paletteIndex1D, 0));
                fixed4 blended = lerp(inputColor, result, _Strength);
                return blended;
            }


        ENDCG
        }

    }

}
