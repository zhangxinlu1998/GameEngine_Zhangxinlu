Shader "URP/CircuitShader"
{

    Properties
    {
        [NoScaleOffset]_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_Normal("NormalMap",2D) = "bump"{}
        _NormalScale("Normal",Range(0,2)) = 1.0
        [NoScaleOffset]_ARMTex("AO(R),Roughness(G)，Metallic(B)",2D) = "white"{}
        _AO ("AO", Range(0, 1)) = 1.0
        _Roughness("Roughness",Range(0,1)) = 1.0
        _Metallic ("Metallic", Range(0, 1)) = 1.0
        [IntRange]_SpecularRange ("SpecularRange", Range(1, 200)) = 50
        
        [Toggle]_EmissionAble("Emission",int) = 0
        [NoScaleOffset]_EmissiveTex("Emissive Tex",2D) = "white" {}
        [HDR]_EmissiveCol("Emissive Color",Color) = (1,1,1,1)
        _Speed("Speed",Range(0,2)) = 0.2
    
        _Shadow ("Shadow Strength", Range(0, 1)) = 0.5
        _ShadowSmoothness ("Shadow Smoothness", Range(0.01, 2)) = 0.5
    }
        
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }         
            
            CGPROGRAM
            

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ARMTex;
            fixed _AO;
            fixed _Roughness;
            fixed _Metallic;
            sampler2D _Normal;
            fixed _NormalScale;
            fixed  _Shadow, _ShadowSmoothness;
            fixed _SpecularRange;
            sampler2D _EmissiveTex;
            fixed4 _EmissiveCol;
            fixed _Speed,_EmissionAble;


            struct v2f
            {
                float4 pos:SV_POSITION;//裁剪空间位置输出
                float2 uv: TEXCOORD0; // 贴图UV
                float3 worldPos: TEXCOORD1;//世界坐标
                float3 tSpace0:TEXCOORD2;//TNB矩阵0
                float3 tSpace1:TEXCOORD3;//TNB矩阵1
                float3 tSpace2:TEXCOORD4;//TNB矩阵2
                

                UNITY_FOG_COORDS(5)//雾效坐标
                UNITY_SHADOW_COORDS(6)//阴影坐标 _ShadowCoord
                
            };


            v2f vert(appdata_full v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half3 worldTangent = UnityObjectToWorldDir(v.tangent);
                half3 worldBinormal = cross(worldNormal,worldTangent)*v.tangent.w *unity_WorldTransformParams.w;

                o.tSpace0 = float3(worldTangent.x,worldBinormal.x,worldNormal.x);
                o.tSpace1 = float3(worldTangent.y,worldBinormal.y,worldNormal.y);
                o.tSpace2 = float3(worldTangent.z,worldBinormal.z,worldNormal.z);

                UNITY_TRANSFER_LIGHTING(o, v.texcoord1.xy); 
                UNITY_TRANSFER_FOG(o, o.pos); 

                return o;
            }


            fixed4 frag(v2f i): SV_Target
            {
                
                half3 normalTex = UnpackNormalWithScale(tex2D(_Normal,i.uv),_NormalScale);
                half3 worldNormal = half3(dot(i.tSpace0,normalTex),dot(i.tSpace1,normalTex),dot(i.tSpace2,normalTex));
                worldNormal = normalize(worldNormal);

                float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                

                fixed4 AlbedoColorSampler = tex2D(_MainTex, i.uv) * _Color;
                fixed3 Albedo = AlbedoColorSampler.rgb;

                fixed4 ARMSampler = tex2D(_ARMTex,i.uv);
                fixed Ao = saturate(ARMSampler.r+1-_AO);
                fixed Roughness = ARMSampler.g*_Roughness;
                fixed Metallic = ARMSampler.b*_Metallic;

                float lambert = dot(worldNormal, lightDir);//计算兰伯特
                lambert = saturate(saturate(lambert / _ShadowSmoothness) + _Shadow);
                float3 diff = Albedo * lambert * _LightColor0 *Ao;
                float spe =  saturate(dot(normalize(lightDir + worldViewDir), worldNormal));//计算高光
                spe = pow(spe, _SpecularRange) * (Metallic+Roughness);

                fixed mask = saturate(sin(-_Time.y*_Speed+i.uv.x));
                float4 emissive = _EmissiveCol * tex2D(_EmissiveTex,i.uv)*mask*_EmissionAble;

                float3 c = diff+spe+emissive.rgb;

                //叠加雾效。
                UNITY_EXTRACT_FOG(i);
                UNITY_APPLY_FOG(_unity_fogCoord, c); 
                return float4(c,1);
            }

            ENDCG
            
        }


    }
    FallBack "Diffuse"
}
