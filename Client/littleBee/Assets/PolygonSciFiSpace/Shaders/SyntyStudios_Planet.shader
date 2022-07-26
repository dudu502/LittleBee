// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/Planets"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0.5181034,0.4426903,0.6764706,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.01
		_RimColor("Rim Color", Color) = (0,0,0,0)
		_RimPower("Rim Power", Range( 0 , 10)) = 0
		_PlanetOverlayMiddle("Planet Overlay Middle", 2D) = "white" {}
		_PlanetOverlayTop("Planet Overlay Top", 2D) = "white" {}
		_PlanetSurfaceContrast("PlanetSurfaceContrast", Float) = 0
		_Planet_Tiling("Planet_Tiling", Float) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_Second_Color("Second_Color", Color) = (0,0,0,0)
		_Planet_Falloff("Planet_Falloff", Float) = 0
		_Cloud_Contrast("Cloud_Contrast", Float) = 0
		_Clouds("Clouds", 2D) = "white" {}
		_CloudPower("Cloud Power", Float) = 0
		_CloudColor("Cloud Color", Color) = (0,0,0,0)
		_CloudCoverage("Cloud Coverage", Float) = 0
		_CloudSpeed("Cloud Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz *= ( 1 + _ASEOutlineWidth);
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float3 viewDir;
		};

		uniform float _CloudCoverage;
		uniform float4 _CloudColor;
		uniform float _CloudPower;
		uniform sampler2D _Clouds;
		uniform float _CloudSpeed;
		uniform float4 _Color0;
		uniform float4 _Second_Color;
		uniform float _PlanetSurfaceContrast;
		uniform sampler2D _PlanetOverlayTop;
		uniform sampler2D _PlanetOverlayMiddle;
		uniform float _Planet_Tiling;
		uniform float _Planet_Falloff;
		uniform float _Cloud_Contrast;
		uniform sampler2D Normals;
		uniform float4 Normals_ST;
		uniform float _RimPower;
		uniform float4 _RimColor;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 appendResult91 = (float4(_CloudSpeed , 0.0 , 0.0 , 0.0));
			float4 appendResult89 = (float4((float)1 , (float)1 , 0.0 , 0.0));
			float2 uv_TexCoord74 = i.uv_texcoord * appendResult89.xy + float2( 1,0 );
			float2 panner72 = ( 1.0 * _Time.y * appendResult91.xy + uv_TexCoord74);
			float div81=256.0/float((int)_CloudPower);
			float4 posterize81 = ( floor( tex2D( _Clouds, panner72 ) * div81 ) / div81 );
			float4 lerpResult85 = lerp( _CloudColor , float4( 0,0,0,0 ) , posterize81);
			float4 lerpResult99 = lerp( lerpResult85 , float4( 0,0,0,0 ) , i.vertexColor.r);
			float3 desaturateInitialColor86 = CalculateContrast(_CloudCoverage,lerpResult99).rgb;
			float desaturateDot86 = dot( desaturateInitialColor86, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar86 = lerp( desaturateInitialColor86, desaturateDot86.xxx, 0.0 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 localPos = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) );
			float3 localNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 triplanar14 = TriplanarSamplingCF( _PlanetOverlayTop, _PlanetOverlayMiddle, _PlanetOverlayTop, localPos, localNormal, _Planet_Falloff, _Planet_Tiling, float3( 1,1,1 ), float3(0,0,0) );
			float4 lerpResult127 = lerp( _Color0 , _Second_Color , CalculateContrast(_PlanetSurfaceContrast,triplanar14));
			float4 triplanar36 = TriplanarSamplingCF( _PlanetOverlayTop, _PlanetOverlayMiddle, _PlanetOverlayTop, localPos, localNormal, _Planet_Falloff, _Planet_Tiling, float3( 1,1,1 ), float3(0,0,0) );
			float4 lerpResult132 = lerp( _Color0 , _Second_Color , ( CalculateContrast(_Cloud_Contrast,triplanar36) * -1.0 ));
			float4 clampResult129 = clamp( ( lerpResult127 + lerpResult132 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 blendOpSrc76 = float4( desaturateVar86 , 0.0 );
			float4 blendOpDest76 = clampResult129;
			o.Albedo = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc76 ) * ( 1.0 - blendOpDest76 ) ) )).rgb;
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
			#pragma target 3.0
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
				half4 color : COLOR0;
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
				o.color = v.color;
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
				surfIN.vertexColor = IN.color;
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
2567;32;2546;1397;2632.625;2277.768;1.710158;True;False
Node;AmplifyShaderEditor.CommentaryNode;97;-1007.108,-1509.225;Float;False;1803.85;522.332;Cloud;17;93;94;89;74;92;91;72;71;82;81;85;83;86;77;78;99;98;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;94;-984.517,-1241.32;Float;False;Constant;_CloudTileY;Cloud Tile Y;18;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;93;-981.2471,-1324.589;Float;False;Constant;_CloudTileX;Cloud Tile X;19;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.DynamicAppendNode;89;-798.2519,-1313.596;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-736.9037,-1135.937;Float;False;Property;_CloudSpeed;Cloud Speed;15;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;74;-643.8028,-1434.535;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;91;-549.9039,-1141.541;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;113;-1024.156,-2954.278;Float;False;1766.6;1423.4;Color;18;132;131;130;129;128;127;126;125;124;122;121;119;14;11;110;12;36;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;72;-395.5857,-1274.241;Float;False;3;0;FLOAT2;1,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;67;-543.836,-81.81092;Float;False;1339.502;405.9783;Rim;10;51;52;47;46;45;48;49;50;53;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-931.6427,-1785.618;Float;False;Property;_Planet_Falloff;Planet_Falloff;8;0;Create;True;0;0;False;0;0;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;-947.4514,-2373.609;Float;True;Property;_PlanetOverlayTop;Planet Overlay Top;3;0;Create;True;0;0;False;0;None;fd15a6fab7475954594f39602e5711e5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;110;-944.3723,-2134.526;Float;True;Property;_PlanetOverlayMiddle;Planet Overlay Middle;2;0;Create;True;0;0;False;0;None;fd15a6fab7475954594f39602e5711e5;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-929.8002,-1892.94;Float;False;Property;_Planet_Tiling;Planet_Tiling;5;0;Create;True;0;0;False;0;0;47.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;-209.5479,-1283.344;Float;True;Property;_Clouds;Clouds;11;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;119;-331.2539,-1674.532;Float;False;Property;_Cloud_Contrast;Cloud_Contrast;10;0;Create;True;0;0;False;0;0;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;51;-483.3301,172.8518;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TriplanarNode;36;-523.2334,-1933.563;Float;True;Cylindrical;Object;False;Top Texture 1;_TopTexture1;white;2;None;Mid Texture 1;_MidTexture1;white;3;None;Bot Texture 1;_BotTexture1;white;5;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;-70.71586,-1079.091;Float;False;Property;_CloudPower;Cloud Power;12;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;66.38983,-1770.799;Float;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;121;-76.56938,-1979.796;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;83;32.84106,-1461.596;Float;False;Property;_CloudColor;Cloud Color;13;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-511.0708,-29.80848;Float;True;Global;Normals;Normals;9;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;14;-533.099,-2357.572;Float;True;Cylindrical;Object;False;Top Texture 0;_TopTexture0;white;2;None;Mid Texture 0;_MidTexture0;white;1;None;Bot Texture 0;_BotTexture0;white;3;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT3;1,1,1;False;3;FLOAT;1;False;4;FLOAT;100;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;-460.5385,-2279.57;Float;False;Property;_PlanetSurfaceContrast;PlanetSurfaceContrast;4;0;Create;True;0;0;False;0;0;-3.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;52;-206.1242,89.32225;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosterizeNode;81;111.4943,-1272.54;Float;False;179;2;1;COLOR;0,0,0,0;False;0;INT;179;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;124;-89.07512,-2338.329;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;125;-404.011,-2871.463;Float;False;Property;_Color0;Color 0;6;0;Create;True;0;0;False;0;0,0,0,0;0.3088235,0.3005548,0.2543252,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;53;-89.495,-26.26904;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;126;-404.2107,-2690.188;Float;False;Property;_Second_Color;Second_Color;7;0;Create;True;0;0;False;0;0,0,0,0;0.3161765,0.2589056,0.227833,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;191.3434,-1941.338;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;98;306.1798,-1465.888;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;85;302.0085,-1288.386;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;50;60.99469,-17.15991;Float;False;1;0;FLOAT;1.23;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;183.1872,-1075.082;Float;False;Property;_CloudCoverage;Cloud Coverage;14;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;99;500.8588,-1285.859;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;132;264.5507,-2190.077;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;127;178.0381,-2513.938;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;49;244.7356,-17.56054;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;56.44581,99.83198;Float;False;Property;_RimPower;Rim Power;1;0;Create;True;0;0;False;0;0;3.73;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;77;429.6875,-1093.182;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;128;403.5988,-2363.588;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;86;623.4377,-1094.057;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;129;577.8684,-2507.004;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;390.4481,128.85;Float;False;Property;_RimColor;Rim Color;0;0;Create;True;0;0;False;0;0,0,0,0;0.5176471,0.4431373,0.6784314,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;48;439.7539,-19.25805;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;651.093,-26.61939;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;76;851.8689,-1044.632;Float;False;Screen;True;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1084.72,-1039.755;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SyntyStudios/Planets;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.01;0.5181034,0.4426903,0.6764706,0;VertexScale;False;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;89;0;93;0
WireConnection;89;1;94;0
WireConnection;74;0;89;0
WireConnection;91;0;92;0
WireConnection;72;0;74;0
WireConnection;72;2;91;0
WireConnection;71;1;72;0
WireConnection;36;0;11;0
WireConnection;36;1;110;0
WireConnection;36;2;11;0
WireConnection;36;3;12;0
WireConnection;36;4;13;0
WireConnection;121;1;36;0
WireConnection;121;0;119;0
WireConnection;14;0;11;0
WireConnection;14;1;110;0
WireConnection;14;2;11;0
WireConnection;14;3;12;0
WireConnection;14;4;13;0
WireConnection;52;0;51;0
WireConnection;81;1;71;0
WireConnection;81;0;82;0
WireConnection;124;1;14;0
WireConnection;124;0;122;0
WireConnection;53;0;54;0
WireConnection;53;1;52;0
WireConnection;131;0;121;0
WireConnection;131;1;130;0
WireConnection;85;0;83;0
WireConnection;85;2;81;0
WireConnection;50;0;53;0
WireConnection;99;0;85;0
WireConnection;99;2;98;1
WireConnection;132;0;125;0
WireConnection;132;1;126;0
WireConnection;132;2;131;0
WireConnection;127;0;125;0
WireConnection;127;1;126;0
WireConnection;127;2;124;0
WireConnection;49;0;50;0
WireConnection;77;1;99;0
WireConnection;77;0;78;0
WireConnection;128;0;127;0
WireConnection;128;1;132;0
WireConnection;86;0;77;0
WireConnection;129;0;128;0
WireConnection;48;0;49;0
WireConnection;48;1;47;0
WireConnection;45;0;48;0
WireConnection;45;1;46;0
WireConnection;76;0;86;0
WireConnection;76;1;129;0
WireConnection;0;0;76;0
WireConnection;0;2;45;0
ASEEND*/
//CHKSM=91EB35D8F3A4B93160C0767F9C7B13F89F404F83