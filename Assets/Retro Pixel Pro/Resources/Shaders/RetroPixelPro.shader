Shader "AlpacaSound/RetroPixelPro"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "" {}
     _ColorMap ("ColorMap", 2D) = "" {}
     _Palette ("Palette", 2D) = "" {}
     _Opacity ("Opacity", Range(0.0, 1.0)) = 1.0
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

         v2f vert (appdata v)
         {
             v2f o;
             o.vertex = UnityObjectToClipPos(v.vertex);
             o.uv = v.uv;
             return o;
         }
         
         sampler2D _MainTex;
         sampler2D _ColorMap;
         sampler2D _Palette;
         int _PaletteSize;
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

        half4 frag(v2f i) : SV_Target
        {
            half4 col = tex2D(_MainTex, i.uv);

            float2 colorMapUV = getColorMapUV(col);
            half4 valueInColorMap = tex2D(_ColorMap, colorMapUV);

            half paletteIndex1D = valueInColorMap.a;
            half2 paletteIndex2D = half2(paletteIndex1D, 0);
            half4 result = tex2D(_Palette, paletteIndex2D);

            result = lerp(col, result, _Opacity);

            return result;
        }


         ENDCG
     }
     
 }
 
}
