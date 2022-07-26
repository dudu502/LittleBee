// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadertoy/Galaxy" {
	Properties{
		iMouse("Mouse Pos", Vector) = (100, 100, 0, 0)
		iChannel0("iChannel0", 2D) = "white" {}
		iChannelResolution0("iChannelResolution0", Vector) = (100, 100, 0, 0)
	}

		CGINCLUDE
#include "UnityCG.cginc"   
#pragma target 3.0      

#define vec2 float2
#define vec3 float3
#define vec4 float4
#define mat2 float2x2
#define mat3 float3x3
#define mat4 float4x4
#define iGlobalTime _Time.x
#define mod fmod
#define mix lerp
#define fract frac
#define texture2D tex2D
#define iResolution _ScreenParams
#define gl_FragCoord ((_iParam.scrPos.xy/_iParam.scrPos.w) * _ScreenParams.xy)

#define PI2 6.28318530718
#define pi 3.14159265358979
#define halfpi (pi * 0.5)
#define oneoverpi (1.0 / pi)

			fixed4 iMouse;
		sampler2D iChannel0;
		fixed4 iChannelResolution0;
		struct v2f {
			float4 pos : SV_POSITION;
			float4 scrPos : TEXCOORD0;
		};

		v2f vert(appdata_base v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.scrPos = ComputeScreenPos(o.pos);
			return o;
		}

		vec4 main(vec2 fragCoord);
		void mainImage(out vec4 fragColor, in vec2 fragCoord);
		fixed4 frag(v2f _iParam) : COLOR0{
			vec2 fragCoord = gl_FragCoord;
			return main(gl_FragCoord);
		}

			vec4 main(vec2 fragCoord) {
			vec4 fragColor;
			mainImage(fragColor, fragCoord);
			return fragColor;
		}


		float field(in vec3 p, float s) {
			float strength = 7. + .03 * log(1.e-6 + fract(sin(iGlobalTime) * 4373.11));
			float accum = s / 4.;
			float prev = 0.;
			float tw = 0.;
			for (int i = 0; i < 26; ++i) {
				float mag = dot(p, p);
				p = abs(p) / mag + vec3(-.5, -.4, -1.5);
				float w = exp(-float(i) / 7.);
				accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
				tw += w;
				prev = mag;
			}
			return max(0., 5. * accum / tw - .7);
		}

		// Less iterations for second layer
		float field2(in vec3 p, float s) {
			float strength = 7. + .03 * log(1.e-6 + fract(sin(iGlobalTime) * 4373.11));
			float accum = s / 4.;
			float prev = 0.;
			float tw = 0.;
			for (int i = 0; i < 18; ++i) {
				float mag = dot(p, p);
				p = abs(p) / mag + vec3(-.5, -.4, -1.5);
				float w = exp(-float(i) / 7.);
				accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
				tw += w;
				prev = mag;
			}
			return max(0., 5. * accum / tw - .7);
		}

		vec3 nrand3(vec2 co)
		{
			vec3 a = fract(cos(co.x * 8.3e-3 + co.y) * vec3(1.3e5, 4.7e5, 2.9e5));
			vec3 b = fract(sin(co.x * 0.3e-3 + co.y) * vec3(8.1e5, 1.0e5, 0.1e5));
			vec3 c = mix(a, b, 0.5);
			return c;
		}


		void mainImage(out vec4 fragColor, in vec2 fragCoord) {
			vec2 uv = 2. * fragCoord.xy / iResolution.xy - 1.;
			vec2 uvs = uv * iResolution.xy / max(iResolution.x, iResolution.y);
			vec3 p = vec3(uvs / 4., 0) + vec3(1., -1.3, 0.) + .2 * vec3(sin(iGlobalTime / 16), sin(iGlobalTime / 12), sin(iGlobalTime / 128));


			float freqs[4];
			//Sound
			freqs[0] = tex2D(iChannel0, vec2(0.01, 0.25)).x;
			freqs[1] = tex2D(iChannel0, vec2(0.07, 0.25)).x;
			freqs[2] = tex2D(iChannel0, vec2(0.15, 0.25)).x;
			freqs[3] = tex2D(iChannel0, vec2(0.30, 0.25)).x;

			float t = field(p, freqs[2]);
			float v = (1. - exp((abs(uv.x) - 1.) * 6.)) * (1. - exp((abs(uv.y) - 1.) * 6.));

			//Second Layer
			vec3 p2 = vec3(uvs / (4. + sin(t * 0.11) * 0.2 + 0.2 + sin(t * 0.15) * 0.3 + 0.4), 1.5) + vec3(2., -1.3, -1.);
			p2 += 0.25 * vec3(sin(t / 16.), sin(t / 12.), sin(t / 128.));
			float t2 = field2(p2, freqs[3]);
			vec4 c2 = mix(.4, 1., v) * vec4(1.3 * t2 * t2 * t2, 1.8 * t2 * t2, t2 * freqs[0], t2);


			//Let's add some stars
			//Thanks to http://glsl.heroku.com/e#6904.0
			vec2 seed = p.xy * 2.0;
			seed = floor(seed * iResolution.x);
			vec3 rnd = nrand3(seed);
			vec4 starcolor = vec4(pow(rnd.y, 40.0), 0, 0, 0);

			//Second Layer
			vec2 seed2 = p2.xy * 2.0;
			seed2 = floor(seed2 * iResolution.x);
			vec3 rnd2 = nrand3(seed2);
			starcolor += vec4(pow(rnd2.y, 40.0), 0, 0, 0);

			fragColor = mix(freqs[3] - .3, 1., v) * vec4(1.5 * freqs[2] * t * t * t, 1.2 * freqs[1] * t * t, freqs[3] * t, 1.0) + c2 + starcolor;
		}


		ENDCG

			SubShader{
				Pass {
					CGPROGRAM

					#pragma vertex vert    
					#pragma fragment frag    
					#pragma fragmentoption ARB_precision_hint_fastest     

					ENDCG
				}
		}
			FallBack Off
}
