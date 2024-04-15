Shader "Custom/GulitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VoronoiScale ("Voronoi Scale", Vector) = (5, 5, 0, 0)
        _ScanlineScale ("ScanlineScale", Float) = 7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/AssetStoreTools/OMC/OMC.URPShaderLibrary/ShaderLibrary/Noise.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varygings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float4 positionSS : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;

            // voronoi
            float2 _VoronoiScale;

            // Scanline
            float _ScanlineScale;
            
            CBUFFER_END

            Varygings vert (Attributes input)
            {
                VertexPositionInputs posInput = GetVertexPositionInputs(input.positionOS.xyz);
                
                Varygings output;

                output.positionCS = posInput.positionCS;
                output.positionSS = posInput.positionNDC;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                
                return output;
            }

            inline half4 Scanline(half4 col, float height, float scale, float strength)
            {
                half factor = pow((sin(height * scale) + 1) * 0.5, strength);
                return col * factor;
            }

            half4 frag (Varygings input) : SV_Target
            {
                float2 screenUV = input.positionSS.xy / input.positionSS.w;
                float aspcet = _ScreenParams.x / _ScreenParams.y;
                screenUV.y /= aspcet;

                screenUV *= _VoronoiScale.xy;

                half voronoiAngleOffset = PerlinNoise(float2(1, _Time.y), 2.0F) * 15.0F + 5.0F;
                voronoiAngleOffset = rand(floor(voronoiAngleOffset)) * 30.0F;

                float cell;
                float voronoi;
                Voronoi(screenUV, voronoiAngleOffset, 1,cell, voronoi);

                half4 sampledCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half4 col = sampledCol;

                col = lerp(col, Scanline(sampledCol, screenUV.y, _ScanlineScale, voronoiAngleOffset), step(voronoi, 0.5));
                
                return col;
            }
            ENDHLSL
        }
    }
}
