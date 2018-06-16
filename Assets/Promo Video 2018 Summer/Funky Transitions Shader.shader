Shader "Custom/Funky Transitions Shader"
{
 Properties
 {
     _MainTex ("Texture", 2D) = "white" {}
     _OtherTex ("Other Texture", 2D) = "white" {}
     _GradientTex ("Gradient Texture", 2D) = "white" {}
     _Amount ("Amount", Range(0.0, 1.0)) = 0.0
 }
 SubShader
 {
     // No culling or depth
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
         sampler2D _OtherTex;
         sampler2D _GradientTex;
         float _Amount;

         fixed4 frag (v2f i) : SV_Target
         {
             fixed4 col = tex2D(_MainTex, i.uv);
             fixed4 otherCol = tex2D(_OtherTex, i.uv);
             fixed gradientValue = tex2D(_GradientTex, i.uv).r;

             if (gradientValue < lerp(-0.01, 1.01, _Amount))
             {
                return col;
             }
             else
             {
                return otherCol;
             }
         }

          ENDCG
     }
 }
}
