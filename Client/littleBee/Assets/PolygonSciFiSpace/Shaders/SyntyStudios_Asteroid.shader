// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/Asteroid"
{
	Properties
	{
		_RimColor("RimColor", Color) = (0,0,0,0)
		_Overlay("Overlay", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_RimPower("RimPower", Range( 0 , 10)) = 0
		_Normal_Tiling("Normal_Tiling", Float) = 0
		_Cloud_Tiling("Cloud_Tiling", Float) = 0
		_Tiling("Tiling", Float) = 0
		_Contrast("Contrast", Float) = 0
		_Float2("Float 2", Float) = 0
		_Cloud_Contrast("Cloud_Contrast", Float) = 0
		_Falloff("Falloff", Float) = 0
		_Cloud_Falloff("Cloud_Falloff", Float) = 0
		_NormalFalloff("NormalFalloff", Float) = 0
		_Second_Color("Second_Color", Color) = (0,0,0,0)
		_BaseColor("BaseColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 viewDir;
		};

		uniform float _Float2;
		uniform sampler2D _Normal;
		uniform float _Normal_Tiling;
		uniform float _NormalFalloff;
		uniform float4 _BaseColor;
		uniform float4 _Second_Color;
		uniform float _Contrast;
		uniform sampler2D _Overlay;
		uniform float _Tiling;
		uniform float _Falloff;
		uniform float _Cloud_Contrast;
		uniform float _Cloud_Tiling;
		uniform float _Cloud_Falloff;
		uniform sampler2D Normals;
		uniform float4 Normals_ST;
		uniform float _RimPower;
		uniform float4 _RimColor;


		inline float4 TriplanarSamplingCF( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm = ( tex2D( midTexMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
			yNorm = ( tex2D( topTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			yNormN = ( tex2D( botTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
			zNorm = ( tex2D( midTexMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 localPos = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) );
			float3 localNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 triplanar64 = TriplanarSamplingCF( _Normal, _Normal, _Normal, localPos, localNormal, _NormalFalloff, _Normal_Tiling, float3( 1,1,1 ), float3(0,0,0) );
			o.Normal = CalculateContrast(_Float2,triplanar64).rgb;
			float4 triplanar14 = TriplanarSamplingCF( _Overlay, _Overlay, _Overlay, localPos, localNormal, _Falloff, _Tiling, float3( 1,1,1 ), float3(0,0,0) );
			float4 lerpResult24 = lerp( _BaseColor , _Second_Color , CalculateContrast(_Contrast,triplanar14));
			float4 triplanar36 = TriplanarSamplingCF( _Overlay, _Overlay, _Overlay, localPos, localNormal, _Cloud_Falloff, _Cloud_Tiling, float3( 1,1,1 ), float3(0,0,0) );
			float4 lerpResult56 = lerp( _BaseColor , _Second_Color , CalculateContrast(_Cloud_Contrast,triplanar36));
			float4 clampResult34 = clamp( ( lerpResult24 + lerpResult56 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Albedo = clampResult34.rgb;
			float2 uvNormals = i.uv_texcoord * Normals_ST.xy + Normals_ST.zw;
			float3 normalizeResult52 = normalize( i.viewDir );
			float dotResult53 = dot( UnpackNormal( tex2D( Normals, uvNormals ) ) , normalizeResult52 );
			o.Emission = ( pow( ( 1.0 - saturate( dotResult53 ) ) , _RimPower ) * _RimColor ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
2567;32;2546;1397;2970.427;1404.474;1.9;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;51;-1449.361,-971.3867;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;13;-1394.566,105.6121;Float;False;Property;_Falloff;Falloff;11;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-1449.361,-1243.387;Float;True;Global;Normals;Normals;8;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;52;-1226.664,-974.4866;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1397.955,29.14948;Float;False;Property;_Tiling;Tiling;6;0;Create;True;0;0;False;0;0;1.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;-1445.385,-164.1653;Float;True;Property;_Overlay;Overlay;1;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1567.476,391.4465;Float;False;Property;_Cloud_Tiling;Cloud_Tiling;5;0;Create;True;0;0;False;0;0;1.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1564.087,467.9091;Float;False;Property;_Cloud_Falloff;Cloud_Falloff;12;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;36;-1150.284,216.6518;Float;True;Cylindrical;Object;False;Top Texture 1;_TopTexture1;white;2;None;Mid Texture 1;_MidTexture1;white;3;None;Bot Texture 1;_BotTexture1;white;5;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-969.606,498.4164;Float;False;Property;_Cloud_Contrast;Cloud_Contrast;10;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;14;-1147.765,-156.7452;Float;True;Cylindrical;Object;False;Top Texture 0;_TopTexture0;white;2;None;Mid Texture 0;_MidTexture0;white;1;None;Bot Texture 0;_BotTexture0;white;3;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;53;-1034.964,-1052.887;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-947.6887,43.25977;Float;False;Property;_Contrast;Contrast;7;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-1036.213,-532.3901;Float;False;Property;_BaseColor;BaseColor;15;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;32;-719.0522,-140.2551;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;25;-1036.413,-351.4901;Float;False;Property;_Second_Color;Second_Color;14;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;-859.7645,-1077.387;Float;False;1;0;FLOAT;1.23;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;40;-657.9718,183.1023;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;49;-690.7615,-1035.088;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1301.226,945.2168;Float;False;Property;_NormalFalloff;NormalFalloff;13;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1304.615,868.7543;Float;False;Property;_Normal_Tiling;Normal_Tiling;4;0;Create;True;0;0;False;0;0;1.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-787.3632,-919.6873;Float;False;Property;_RimPower;RimPower;3;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;56;-412.8538,12.16079;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;24;-460.3137,-340.9896;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;66;-1312.625,627.2128;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PowerNode;48;-497.9637,-1011.687;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;-504.1632,-831.8879;Float;False;Property;_RimColor;RimColor;0;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-706.746,975.7241;Float;False;Property;_Float2;Float 2;9;0;Create;True;0;0;False;0;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-234.7532,-190.6395;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;64;-887.4242,693.9595;Float;True;Cylindrical;Object;False;Top Texture 2;_TopTexture2;white;2;None;Mid Texture 2;_MidTexture2;white;3;None;Bot Texture 2;_BotTexture2;white;5;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-255.5634,-996.4875;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;34;-60.48365,-334.0549;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;65;-395.113,660.41;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;131.4001,-345;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;SyntyStudios/Asteroid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.09;1,0,0,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;52;0;51;0
WireConnection;36;0;11;0
WireConnection;36;1;11;0
WireConnection;36;2;11;0
WireConnection;36;3;38;0
WireConnection;36;4;39;0
WireConnection;14;0;11;0
WireConnection;14;1;11;0
WireConnection;14;2;11;0
WireConnection;14;3;12;0
WireConnection;14;4;13;0
WireConnection;53;0;54;0
WireConnection;53;1;52;0
WireConnection;32;1;14;0
WireConnection;32;0;33;0
WireConnection;50;0;53;0
WireConnection;40;1;36;0
WireConnection;40;0;41;0
WireConnection;49;0;50;0
WireConnection;56;0;22;0
WireConnection;56;1;25;0
WireConnection;56;2;40;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;24;2;32;0
WireConnection;48;0;49;0
WireConnection;48;1;47;0
WireConnection;55;0;24;0
WireConnection;55;1;56;0
WireConnection;64;0;66;0
WireConnection;64;1;66;0
WireConnection;64;2;66;0
WireConnection;64;3;62;0
WireConnection;64;4;61;0
WireConnection;45;0;48;0
WireConnection;45;1;46;0
WireConnection;34;0;55;0
WireConnection;65;1;64;0
WireConnection;65;0;63;0
WireConnection;0;0;34;0
WireConnection;0;1;65;0
WireConnection;0;2;45;0
ASEEND*/
//CHKSM=DCA5F7842357D615D66AD8F8E2CD1BF5F836CDC3