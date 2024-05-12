Shader "Custom/GulitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VoronoiScale ("Voronoi Scale", Vector) = (5, 5, 0, 0)
        _ScanlineScale ("ScanlineScale", Float) = 7
        _SpecularColor ("Specular Color", Color) = (1, 0, 1, 1)
        _GlitchScale ("Glitch Scale", Float) = 10
        _GlitchSpeed ("Glitch Speed", Float) = 10
        _GlitchIntensity ("Glitch Intensity", Float) = 3
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Assets/AssetStoreTools/OMC/OMC.URPShaderLibrary/ShaderLibrary/Noise.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float3 normalWS : NORMAL;
                float2 positionSS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;

            // voronoi
            float2 _VoronoiScale;

            // Scanline
            float _ScanlineScale;

            // Phone
            float4 _SpecularColor;

            // Glitch
            float _GlitchScale;
            float _GlitchSpeed;
            float _GlitchIntensity;
            
            CBUFFER_END

            float2 glitch(float2 seed, float scale, float speed, float intensity)
            {
                float noise = BlockNoise(seed.y * scale, 1);
                //noise += rand(seed.x) * 0.3F;
                float perlin = PerlinNoise(float2(seed.y, _Time.y * speed), 2);
                seed.x += lerp(
                    0,
                    (noise * 2 - 1) * intensity,
                    step(perlin, 0.3));
                return seed;
            }

            Varyings vert (Attributes input)
            {
                VertexPositionInputs posInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normInput = GetVertexNormalInputs(input.normalOS);
                
                Varyings output = (Varyings)0;
                
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.positionWS = posInput.positionWS;
                output.positionSS = posInput.positionNDC.xy / posInput.positionNDC.w;
                float aspcet = _ScreenParams.x / _ScreenParams.y;
                output.positionSS.y /= aspcet;
                
                output.normalWS = normInput.normalWS;

                // 頂点Glitch
                float3 positionVS = TransformWorldToView(output.positionWS);
                positionVS.xy = glitch(positionVS.xy, _GlitchScale, _GlitchSpeed, _GlitchIntensity);
                output.positionCS = TransformWViewToHClip(positionVS);
                
                return output;
            }

            inline half4 Scanline(half4 col, float height, float scale, float strength)
            {
                half factor = pow((sin(height * scale) + 1) * 0.5, strength);
                return col * factor;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float2 screenUV = input.positionSS;
                screenUV *= _VoronoiScale.xy;
                
                half4 sampledCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // VoronoiGlitch
                half voronoiAngleOffset = rand(floor(PerlinNoise(_Time.y, 2) * 15.0H)) * 30.0H + 2.0H;
                float cell, voronoi;
                Voronoi(screenUV, voronoiAngleOffset, 1,cell, voronoi);
                half4 col = lerp(sampledCol, Scanline(sampledCol, screenUV.y, _ScanlineScale, rand(voronoi)), step(voronoi, 0.5));

                // PhongLighting
                Light mainLit = GetMainLight();
                float3 lightDir = normalize(reflect(-mainLit.direction, input.normalWS));
                float spec = dot(lightDir, GetWorldSpaceNormalizeViewDir(input.positionWS));
                spec = pow(saturate(spec), 5);
                col += spec * _SpecularColor;
                
                return col;
            }
            ENDHLSL
        }
    }
}
