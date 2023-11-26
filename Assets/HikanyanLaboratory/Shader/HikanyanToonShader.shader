/*
Playerと背景のShader
セルルックシェーダー
ToonShaderをBaseにして作成

影の範囲

マットキャップ
部分的に光沢をつけることができるテクスチャ
リムライト

アウトラインシェーダーのオンオフを切り替えることができる
アウトラインの色を変更することができる
アウトラインの太さを変更することができる

エミッション用のテクスチャ
エミッションの色を変更することができる
エミッションの強さを変更することができる


*/
Shader "Unlit/HikanyanToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)

        _LambertThresh("LambertThresh",  Range(0.0, 1.0)) = 0.5
        _GradWidth("ShadowWidth", Range(0.003,1)) = 0.1
        _Sat("Sat", Range(0, 2)) = 0.5

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 1.0

        _MatCap ("MatCap Texture", 2D) = "white" {}
        _RimColor ("Rim Light Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Float) = 1

        _EmissionTex ("Emission Texture", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD1;
            };

            struct attributes
            {
                float4 positionOS : POSITION;
            };

            struct varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            varyings vert(attributes IN)
            {
                varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            half4 _MainColor;
            float _LambertThresh;
            float _GradWidth;
            float _Sat;

            v2f vert(appdata v)
            {
                v2f o;

                VertexPositionInputs inputs = GetVertexPositionInputs(v.vertex.xyz);
                // スクリーン座標に変換.
                o.vertex = inputs.positionCS;
                // ワールド座標系変換.
                o.normal = normalize(TransformObjectToWorldNormal(v.normal));

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Main light情報の取得.
                Light mainLight = GetMainLight();

                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // UNorm lambert. 0~1.
                float uNormDot = saturate(dot(mainLight.direction.xyz, i.normal) * 0.5 + 0.5);
                // step(y,x) ... y < x ? 1 : 0
                float isShaded = step(uNormDot, _LambertThresh);

                if (isShaded)
                {
                    // 影色の決定.
                    half3 multiColor = mainLight.color * color.rgb;
                    // 中間色をオーバーレイでグラデーションするために,彩度を上げる.
                    half3 hsv = RgbToHsv(mainLight.color);
                    hsv.g *= _Sat;
                    half3 overlayInputColor = HsvToRgb(hsv);

                    // オーバーレイ演算.
                    // if(基本色 < 0.5) 結果色 = 基本色 * 合成色 * 2
                    // else if(基本色 ≧ 0.5) 結果色 = 1 – 2 ×（1 – 基本色）×（1 – 合成色）
                    half3 overlayThreshold = step(0.5f, multiColor);
                    // overlayThreshold == 0 ... 乗算, overlayThreshold == 1 ... スクリーン
                    // 乗算カラーをベースに,オーバーレイ効果ブレンドする.
                    half3 overlayColor = lerp(overlayInputColor * multiColor * 2.f,
                                              1.f - 2 * (1.f - overlayInputColor) * (1.f - multiColor),
                                              overlayThreshold);
                    // オーバーレイと乗算をグラデーションして最終陰色を決定.
                    color.rgb = lerp(overlayColor, multiColor,
                                     1 - saturate(uNormDot - (_LambertThresh - _GradWidth)) / _GradWidth);
                }
                return color;
            }
            ENDHLSL
        }
    }
    CustomEditor "HikanyanLaboratory.HikanyanToonShader.HikanyanCustomShaderInspector"
}