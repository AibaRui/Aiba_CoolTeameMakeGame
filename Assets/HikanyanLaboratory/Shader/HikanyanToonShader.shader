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
Shader "Hikanyan/HikanyanToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)

        _LambertThresh("LambertThresh", Range(0.0, 1.0)) = 0.5

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 1.0

        _MatCap ("MatCap Texture", 2D) = "white" {}
        _RimColor ("Rim Light Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Float) = 1

        _EmissionTex ("Emission Texture", 2D) = "white" {}
        [HDR] _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Float) = 1


        StencilRef ("StencilRef", Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]
        StencilComp ("StencilComp", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]
        StencilPass ("StencilPass", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]
        StencilFail ("StencilFail", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]
        StencilZFail ("StencilZFail", Int) = 0
        StencilReadMask ("StencilReadMask", Int) = 0
        StencilWriteMask ("StencilWriteMask", Int) = 0
        [Enum(UnityEngine.Rendering.CullMode)]
        Culling ("Culling", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]
        _ZTest ("ZTest", Float) = 2
        [Enum(Off, 0, On, 1)]
        _ZWrite ("ZWrite", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "DisableBatching" = "True"
        }
        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            // アルファブレンディングを設定。ソースのアルファ値に基づいて、ソースとデスティネーションの色をブレンドします。
            Blend SrcAlpha OneMinusSrcAlpha

            // カリングモードを設定。[Culling]は外部からのパラメーターを参照しています。
            Cull [Culling]

            // 深度テストの方法を設定。[_ZTest]は外部からのパラメーターを参照しています。
            ZTest [_ZTest]

            // 深度書き込みの有無を設定。[_ZWrite]は外部からのパラメーターを参照しています。
            ZWrite [_ZWrite]
            // ステンシルバッファの設定。
            Stencil
            {
                Ref [StencilRef] // ステンシルテストに使用する参照値。
                ReadMask [StencilReadMask] // ステンシルバッファを読み取る際のマスク。
                WriteMask [StencilWriteMask] // ステンシルバッファに書き込む際のマスク。
                Comp [StencilComp] // ステンシルテストの比較関数。
                Pass [StencilPass] // ステンシルテストと深度テストに合格した場合の動作。
                Fail [StencilFail] // ステンシルテストに不合格の場合の動作。
                ZFail [StencilZFail] // 深度テストに不合格の場合の動作。
            }

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex VertexMain
            #pragma fragment FragmentMain
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "operator.hlsl"
            #include "core.hlsl"
            #include "vert.hlsl"
            #include "frag.hlsl"
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Unlit"
    CustomEditor "HikanyanLaboratory.HikanyanToonShader.HikanyanCustomShaderInspector"
}