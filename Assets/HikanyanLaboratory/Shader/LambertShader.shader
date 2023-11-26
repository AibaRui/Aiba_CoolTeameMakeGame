Shader "Hikanyan/CharacterToonLambert"
{
    Properties
    {
        [Header(Lambert)]
        _MainTex ("MainTex", 2D) = "white" {}
        _LambertThresh("LambertThresh", float) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "Character-Toon"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex VertexMain
            #pragma fragment FragmentMain

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Appdata_full
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Vertex2Fragment
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_ST;
            float _LambertThresh;
            
            #include "vert.hlsl"
            #include "frag.hlsl"
            ENDHLSL
        }
    }
}