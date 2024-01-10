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

            #include "Includes/UniversalRenderPipeline.hlsl"
            #include "Includes/core.hlsl"
            #include "Includes/vert.hlsl"
            #include "Includes/frag.hlsl"
            ENDHLSL
        }
    }
}