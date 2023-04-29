Shader "Little Warrior/SkillTreeBWShader"
{
	Properties{
		_MainTexture("Texture", 2D) = "white" {}
		_Color("Colour", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range(0.0, 1.0)) = 1.0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
	}

	SubShader{
		
		Tags{"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"}

		Stencil{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass{
			CGPROGRAM

			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/API/D3D11.hlsl"


			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color;
			sampler2D _MainTexture;
			float _Alpha;

			v2f vertexFunc(appdata IN) {
				v2f OUT;

				OUT.position = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			fixed4 fragmentFunc(v2f IN) : SV_Target{

				/*
				float3 returnedColor;

				float4 sampleColor = tex2D(_MainTexture, IN.uv);

				float4 _sampleTex2D_rgba = SAMPLE_TEXTURE2D(_MainTexture, _MainTexture, IN.uv);

				float3 saturationIn = sampleColor;

				float saturation = 0;
				float luma = dot(saturationIn, float3(0.2126729, 0.7151522, 0.0721750));
				float3 saturationOut = luma.xxx + saturation.xxx * (saturationIn - luma.xxx);

				float multAlpha = (sampleColor.a * _Alpha);

				float4 pixelColor = (saturationOut.r, saturationOut.g, saturationOut.b, multAlpha);
				returnedColor = pixelColor;
				fixed4 finalColor = (returnedColor.r, returnedColor.g, returnedColor.b, _Alpha);*/

				SamplerState sampler_Tex;
				Texture2D mTexture;

				float3 textSample = mTexture.Sample(sampler_Tex, IN.uv);

				fixed4 finalColor;

				return finalColor;

			}

			

			ENDCG
		}
	}
}
