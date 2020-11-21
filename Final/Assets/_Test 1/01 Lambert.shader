Shader "URP/Lambert"
{

    Properties
    {

        _MainTex ("MainTex", 2D) = "White" { }
        _Color ("BaseColor", Color) = (1, 1, 1, 1)
        _Color1 ("Set Color", Color) = (1, 1, 1, 1)
        _Color2 ("Distroy Color", Color) = (1, 1, 1, 1)
        _Shadow("Shadow Strength",Range(0,1)) = 0.5
    }

    SubShader
    {

        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }
        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(PerMaterials)
        float4 _MainTex_ST;
        half4 _Color,_Color1,_Color2;
        half _Shadow;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        struct a2v
        {
            float4 vertex: POSITION;
            float4 normalOS: NORMAL;
            float2 uv: TEXCOORD;
        };

        struct v2f
        {
            float4 pos: SV_POSITION;
            float2 uv: TEXCOORD;
            float3 normalWS: TEXCOORD1;
        };

        
        ENDHLSL
        
        pass
        {
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
  
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            v2f vert(a2v i)
            {
                v2f o;
                o.pos = TransformObjectToHClip(i.vertex.xyz);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                o.normalWS = TransformObjectToWorldNormal(i.normalOS.xyz);
                return o;
            }

            real4 frag(v2f i): SV_TARGET
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * _Color;
                Light mylight = GetMainLight();

                float3 LightDir = normalize(mylight.direction);
                float LightAtten = saturate(dot(LightDir, i.normalWS)+_Shadow);
                float4 col = tex * LightAtten ;
                clip(col.a-0.9);
                return col;
            }          
            ENDHLSL            
        }
    }
}
