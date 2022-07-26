// Draws a vertical gradient between TopColor and BottomColor
Shader "MoreMountains/VerticalGradient"
 {
     Properties
     {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _TopColor("Color1", Color) = (1,1,1,1)
         _BottomColor("Color2", Color) = (1,1,1,1)
 		 _StencilComp ("Stencil Comparison", Float) = 8
		 _Stencil ("Stencil ID", Float) = 0
		 _StencilOp ("Stencil Operation", Float) = 0
		 _StencilWriteMask ("Stencil Write Mask", Float) = 255
		 _StencilReadMask ("Stencil Read Mask", Float) = 255
		 _ColorMask ("Color Mask", Float) = 15
     }

     SubShader
     {
         Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
         Pass
         {
             ZWrite Off
             Blend SrcAlpha OneMinusSrcAlpha
 
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
 
             #include "UnityCG.cginc"
 
             sampler2D _MainTex;
             float4 _MainTex_ST;
             float4 _MainTex_TexelSize;
 
             fixed4 _TopColor;
             fixed4 _BottomColor;
             half _Value;
 
             struct v2f 
             {
                 float4 position : SV_POSITION;
                 fixed4 color : COLOR;
                 float2 uv : TEXCOORD0;
             };
 
             v2f vert (appdata_full v)
             {
                 v2f o;
                 o.position = UnityObjectToClipPos (v.vertex);
                 o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                 o.color = lerp (_TopColor,_BottomColor, v.texcoord.y);
                 return o;
             }
 
             fixed4 frag(v2f i) : SV_Target
             {
                 float4 color;
                 color.rgb = i.color.rgb;
                 color.a = tex2D (_MainTex, i.uv).a * i.color.a;
                 return color;
             }
             ENDCG
         }
     }
 }