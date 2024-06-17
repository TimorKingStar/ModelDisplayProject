Shader "Unlit/OutLine"
{
    Properties
    {
        _BaseMap ("BaseMap", 2D) = "white" {}
        _Diffuse("Diffuse", Color) = (1,1,1,1)
        _Specular("Specular", Color) = (1,1,1,1)
        _RoughnessMap("RoughnessMap", 2D) = "black" {}
        _Roughness("Roughness", Range(1,256)) = 1
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("BumpScale", float) = 1
        _AlphaScale ("Alpha", Range(0,1)) = 1
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
            Cull front
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            fixed4 _Diffuse;
            float _Roughness;
            fixed4 _Specular;
            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            float _AlphaScale;
            sampler2D _RoughnessMap;
            float4 _RoughnessMap_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtiW0 : TEXCOORD1;
                float4 TtiW1 : TEXCOORD2;
                float4 TtiW2 : TEXCOORD3;
            };

            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _BaseMap);
                o.uv.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);

                //计算世界坐标下的顶点位置、法线、切线、副法线
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                //按列摆放得到从切线空间到世界空间的变换矩阵
                o.TtiW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
                o.TtiW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
                o.TtiW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldPos = float3(i.TtiW0.w, i.TtiW1.w, i.TtiW2.w);

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tangentNormal = UnpackNormal(packedNormal);
                tangentNormal.xy *= packedNormal.xy * _BumpScale;

                fixed3 worldNormal = normalize(float3(dot(i.TtiW0.xyz, tangentNormal), dot(i.TtiW1.xyz, tangentNormal), dot(i.TtiW2.xyz, tangentNormal)));


                fixed3 ambinet = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 albedo = tex2D(_BaseMap, i.uv.xy).rgb;

                fixed3 diffuse = _LightColor0.rgb * albedo.rgb * _Diffuse.rgb * (saturate(dot(lightDir,worldNormal)));

                fixed3 halfDir = normalize(lightDir + viewDir);
                fixed roughnessmap = tex2D(_RoughnessMap, i.uv).r;
                fixed3 specular = _LightColor0.rgb * lerp(_Specular.rgb, float3(0,0,0), roughnessmap) * pow(saturate(dot(worldNormal,halfDir)), _Roughness);


                fixed3 final_color = ambinet + diffuse + specular;

                float final_alpha = saturate(_AlphaScale);
                return fixed4(final_color,final_alpha);
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

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            fixed4 _Diffuse;
            float _Roughness;
            fixed4 _Specular;
            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            float _AlphaScale;
            sampler2D _RoughnessMap;
            float4 _RoughnessMap_ST;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 TtiW0 : TEXCOORD1;
                float4 TtiW1 : TEXCOORD2;
                float4 TtiW2 : TEXCOORD3;
            };

            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _BaseMap);
                o.uv.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);

                //计算世界坐标下的顶点位置、法线、切线、副法线
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                //按列摆放得到从切线空间到世界空间的变换矩阵
                o.TtiW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
                o.TtiW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
                o.TtiW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 worldPos = float3(i.TtiW0.w, i.TtiW1.w, i.TtiW2.w);

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

                fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                fixed3 tangentNormal = UnpackNormal(packedNormal);
                tangentNormal.xy *= packedNormal.xy * _BumpScale;

                fixed3 worldNormal = normalize(float3(dot(i.TtiW0.xyz, tangentNormal), dot(i.TtiW1.xyz, tangentNormal), dot(i.TtiW2.xyz, tangentNormal)));


                fixed3 ambinet = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed3 albedo = tex2D(_BaseMap, i.uv.xy).rgb;

                fixed3 diffuse = _LightColor0.rgb * albedo.rgb * _Diffuse.rgb * (saturate(dot(lightDir,worldNormal)));

                fixed3 halfDir = normalize(lightDir + viewDir);
                fixed roughnessmap = tex2D(_RoughnessMap, i.uv).r;
                fixed3 specular = _LightColor0.rgb * lerp(_Specular.rgb, float3(0,0,0), roughnessmap) * pow(saturate(dot(worldNormal,halfDir)), _Roughness);


                fixed3 final_color = ambinet + diffuse + specular;

                float final_alpha = saturate(_AlphaScale);
                return fixed4(final_color,final_alpha);
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
