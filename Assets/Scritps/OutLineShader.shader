Shader "Custom/OutlineShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.01
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

			Pass
			{
				Name "OUTLINE"
				Tags { "LightMode" = "Always" }

				ZWrite Off
				ZTest LEqual
				Cull Front

				ColorMask RGB
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 pos : POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					return _OutlineColor;
				}
				ENDCG
			}

			Pass
			{
				Name "BASE"
				Tags { "LightMode" = "ForwardBase" }

				ZWrite On
				ZTest LEqual
				Cull Back

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 pos : POSITION;
					float4 screenPos : TEXCOORD1;
				};

				uniform float _OutlineWidth;
				uniform sampler2D _MainTex;

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.screenPos = ComputeScreenPos(o.pos);
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					half4 col = tex2D(_MainTex, i.screenPos.xy / i.screenPos.w);
					half4 outlineCol = tex2D(_MainTex, i.screenPos.xy / i.screenPos.w + float2(_OutlineWidth / i.screenPos.w, 0));
					half4 finalCol = lerp(col, _OutlineColor, step(length(col - outlineCol), _OutlineWidth));
					return finalCol;
				}
				ENDCG
			}
		}
}
