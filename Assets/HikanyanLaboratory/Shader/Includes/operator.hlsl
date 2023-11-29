//比較演算を行うためのカスタム関数を定義

// トゥーンシェーディングのライトモデルを計算する関数
float4 ToonLightModel(float3 normal, float3 lightDir)
{
    // 法線とライト方向のドット積を計算し、負の値を0にクランプ
    float ndotl = max(dot(normal, lightDir), 0.0);
    // ステップ関数を使用して硬い境界を作り、トゥーンのようなライティング効果を生成
    return step(0.5, ndotl);
}

// カラーランプを使用して色をトゥーンスタイルに変換する関数
float4 ToonRamp(float4 color, float4 ramp)
{
    // 元の色の赤チャネルを使用してランプの色を補間
    float3 rampColor = lerp(ramp.rgb, ramp.aaa, color.r);
    // 補間された色と元のアルファ値を組み合わせて返す
    return float4(rampColor, color.a);
}

// xがedge以上の場合に1を返し、そうでない場合は0を返す関数
float GreaterThanOrEquals(float x, float edge)
{
    return step(edge, x);
}

// xがedge以下の場合に1を返し、そうでない場合は0を返す関数
float LessThanOrEquals(float x, float edge)
{
    return step(x, edge);
}

// xがedgeより大きい場合に1を返し、そうでない場合は0を返す関数
float GreaterThan(float x, float edge)
{
    return x > edge ? 1 : 0;
}

// xがedgeより小さい場合に1を返し、そうでない場合は0を返す関数
float LessThan(float x, float edge)
{
    return x < edge ? 1 : 0;
}

// xとedgeが等しくない場合に1を返し、等しい場合は0を返す関数
float NotEquals(float x, float edge)
{
    return abs(sign(x - edge));
}

// xとedgeが等しい場合に1を返し、等しくない場合は0を返す関数
float Equals(float x, float edge)
{
    return 1 - abs(sign(x - edge));
}
// オブジェクトのアウトラインを生成する関数
float4 GenerateOutline(float edgeWidth, float3 normal, float3 viewDir) {
    float edge = 1.0 - smoothstep(0.0, edgeWidth, dot(normal, viewDir));
    return edge;
}
// 光沢のあるハイライトを生成する関数
float4 GenerateSpecularHighlight(float3 lightDir, float3 viewDir, float3 normal, float shininess) {
    float3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
    return spec;
}
// テクスチャの色を量子化する関数
float3 QuantizeTextureColor(float3 color, int levels) {
    float quantized = floor(color * levels) / levels;
    return quantized;
}
