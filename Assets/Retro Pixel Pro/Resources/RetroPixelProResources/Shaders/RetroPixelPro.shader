Shader "AlpacaSound/RetroPixelPro"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "" {}
     _ColorMap ("ColorMap", 2D) = "" {}
     _Palette ("Palette", 2D) = "" {}
     _BlueNoise ("BlueNoise", 2D) = "" {}
     _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
     _Dither ("Dither", Range(0.0, 1.0)) = 1.0
 }
 
 SubShader
 {
     Cull Off 
     ZWrite Off 
     ZTest Always

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

         v2f vert(appdata v)
         {
             v2f o;
             o.vertex = UnityObjectToClipPos(v.vertex);
             o.uv = v.uv;
             return o;
         }
         
         sampler2D _MainTex;
         sampler2D _ColorMap;
         sampler2D _Palette;
         sampler2D _BlueNoise;
         int _PaletteSize;
         half _Dither;
         half _Opacity;

        float2 getColorMapUV(half4 col)
        {
            // Texture3D size = 64 x 64 x 64
            // Texture2D size = 512 x 512

            col = saturate(col);
            col = floor(col * 63);

            float pixel = 0;
            pixel += col.r;
            pixel += col.g * 64;
            pixel += col.b * 64 * 64;

            float2 coord2D = float2(pixel % 512, floor(pixel / 512));
            float2 uv = coord2D / 512.0;
            uv = saturate(uv);

            return uv;
        }

        half4 getPaletteColor(half index)
        {
            half2 index2D = half2(index, 0);
            return tex2D(_Palette, index2D);
        }

        half4 frag(v2f i) : SV_Target
        {
            half4 col = tex2D(_MainTex, i.uv);
            float2 colorMapUV = getColorMapUV(col);
            half4 colormapValue = tex2D(_ColorMap, colorMapUV);
            float blueNoiseSample = tex2D(_BlueNoise, i.vertex.xy / 64).r;
            //float blend = 0.95;
            float blend = getPaletteColor(colormapValue.b);

            float4 result; 

            if (Luminance(col) * blueNoiseSample < (1-blend * _Dither))
            {
                //result = getPaletteColor(colormapValue.a);
                result = getPaletteColor(colormapValue.r);
            }
            else
            {
                //result = half4(1,1,1,1);
                result = getPaletteColor(colormapValue.g);
            }

            result = lerp(col, result, _Opacity);

            return result;
        }


         ENDCG
     }
     
 }
 
}
