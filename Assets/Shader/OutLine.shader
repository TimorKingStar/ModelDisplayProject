Shader "Unlit/OutLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _AlphaScale ("Alpha", float) = 1
        _EnableOutLine ("Enable OutLine (0 or 1)", int) = 0
        _OutLineColor ("OutLine Color", Color) = (0,0,0,1)
        _OutLineWidth ("OutLine Width", float) = 1

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" "LightMode"="ForwardBase"}
        LOD 100
 
        Pass
        {
            Cull Front
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            fixed4 _MainColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AlphaScale;

            struct v2f
            {
                fixed3 worldNormal : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
                SHADOW_COORDS(3)
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 texColor = tex2D(_MainTex, i.uv) * _MainColor;
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
                fixed3 diffuse = _LightColor0.rgb * texColor.rgb * saturate(dot(i.worldNormal, worldLightDir)*0.5 + 0.5);
                fixed3 color = ambient + diffuse;

                fixed atten = SHADOW_ATTENUATION(i);
                color.rgb *= atten;
                fixed finalalpha = saturate(_AlphaScale);
                return fixed4(color,finalalpha);
            }
            ENDCG
        }

        Pass
        {
            Cull back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            fixed4 _MainColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AlphaScale;

            struct v2f
            {
                fixed3 worldNormal : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
                SHADOW_COORDS(3)
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 texColor = tex2D(_MainTex, i.uv) * _MainColor;
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
                fixed3 diffuse = _LightColor0.rgb * texColor.rgb * saturate(dot(i.worldNormal, worldLightDir)*0.5 + 0.5);
                fixed3 color = ambient + diffuse;

                fixed atten = SHADOW_ATTENUATION(i);
                color.rgb *= atten;
                fixed finalalpha = saturate(_AlphaScale);
                return fixed4(color,finalalpha);
            }
            ENDCG
        }

        Pass
        {
            Name "Outline"
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutLineWidth;
            int _EnableOutLine;
            fixed4 _OutLineColor;

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldnormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 viewNormal = TransformViewToProjection(worldnormal.xy);
                o.pos.xy += viewNormal * _OutLineWidth * 0.01 * saturate(_EnableOutLine);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                return _OutLineColor;
            }

            ENDCG
        }
    }
}
