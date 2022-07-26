Shader "Custom/RotateImageEffect" {
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RotScale("Rot Scale",Range(-10,10)) = 1
	}
	SubShader 
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _RotScale;
			struct appdata
			{
				float4 vertex:POSITION;
				float2 uv:TEXCOORD0;
			};
			struct v2f
			{
				float2 uv:TEXCOORD0;
				float4 vertex:SV_POSITION;
			};

			v2f vert(appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i):SV_TARGET
			{
				float2 uv = i.uv;
				float2 dt = uv - float2(0.5,0.5);
				float dist = sqrt(dot(dt,dt));

				float theta = dist*UNITY_PI*_RotScale;
				float2x2 rot={
					cos(theta),sin(theta),
					-sin(theta),cos(theta)

				};
				dt = mul(rot,dt);
				uv = dt+float2(0.5,0.5);
				float4 color = tex2D(_MainTex,uv);
				return Luminance(color);//灰度
				//return color;
			}


			ENDCG
		}
	}
	FallBack "Diffuse"
}
