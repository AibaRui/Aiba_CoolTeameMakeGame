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
        [MainTexture]
        _MainTex ("Texture", 2D) = "white" {}
        [MainColor]
        _MainColor ("Color", Color) = (1,1,1,1)
        [_OutlineColor]
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        [Toggle(_)]
        _OutlineWidth ("Outline Width", Float) = 1.0
        [MatCapTexture]
        _MatCap ("MatCap Texture", 2D) = "white" {}
        [RimLightColor]
        _RimColor ("Rim Light Color", Color) = (1,1,1,1)
        [Toggle(_)]
        _RimPower ("Rim Power", Float) = 1
        [EmissionTexture]
        _EmissionTex ("Emission Texture", 2D) = "white" {}
        [EmissionColor]
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        [Toggle(_)]
        _EmissionStrength ("Emission Strength", Float) = 1
    }
    SubShader
    {
        // SubShader Tags では SubShader ブロックまたはパスが実行されるタイミングと条件を
        // 定義します。
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            // HLSL コードブロック。Unity SRP では HLSL 言語を使用します。
            HLSLPROGRAM
            // この行では頂点シェーダーの名前を定義します。
            #pragma vertex vert
            // この行ではフラグメントシェーダーの名前を定義します。
            #pragma fragment frag

            // Core.hlsl ファイルには、よく使用される HLSL マクロおよび関数の
            // 定義が含まれ、その他の HLSL ファイル (Common.hlsl、
            // SpaceTransforms.hlsl など) への #include 参照も含まれています。
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // この構造体定義では構造体に含まれる変数を定義します。
            // この例では Attributes 構造体を頂点シェーダーの入力構造体として
            // 使用しています。
            struct Attributes
            {
                // positionOS 変数にはオブジェクト空間内での頂点位置が
                // 含まれます。
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                // この構造体内の位置には SV_POSITION セマンティクスが必要です。
                float4 positionHCS : SV_POSITION;
            };

            // Varyings 構造体内に定義されたプロパティを含む頂点シェーダーの
            // 定義。vert 関数の型は戻り値の型 (構造体) に一致させる
            // 必要があります。
            Varyings vert(Attributes IN)
            {
                // Varyings 構造体での出力オブジェクト (OUT) の宣言。
                Varyings OUT;
                // TransformObjectToHClip 関数は頂点位置をオブジェクト空間から
                // 同種のクリップスペースに変換します。
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // 出力を返します。
                return OUT;
            }

            half4 _MainColor;
            
            // フラグメントシェーダーの定義。
            half4 frag() : SV_Target
            {
                // 色変数を定義して返します。
                half4 customColor = _MainColor;
                return customColor;
            }
            ENDHLSL
        }
    }
    CustomEditor "HikanyanLaboratory.HikanyanToonShader.HikanyanCustomShaderInspector"
}