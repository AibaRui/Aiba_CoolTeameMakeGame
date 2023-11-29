
// Baseの計算を行う.
float4 BaseMain(Vertex2Fragment i): SV_Target
{
    // Main light情報の取得.
    Light mainLight;
    mainLight = GetMainLight();

    float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

    //  UNorm lambert. 0~1.
    float uNormDot = saturate(dot(mainLight.direction.xyz, i.normal) * 0.5f + 0.5f);
    // _LambertThreshを閾値とした二値化.
    // step(y,x) ... y < x ? 1 : 0
    float ramp = step(uNormDot, _LambertThresh);
    // mainLight.colorの乗算を影色とする.
    color.rgb = lerp(color, color * mainLight.color, ramp);
    return color;
}

//Quantizationを行う.
float4 QuantizationMain(Vertex2Fragment i): SV_Target
{
    float4 color = BaseMain(i);

    // 量子化.
    color.rgb = floor(color.rgb * _Quantization) / _Quantization;
    return color;
}






float4 FragmentMain(Vertex2Fragment i): SV_Target
{
    float4 color = BaseMain(i);
    
    return color;
}
