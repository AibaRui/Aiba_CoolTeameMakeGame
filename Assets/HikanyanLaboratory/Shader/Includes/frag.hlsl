float Dot2(float2 v)
{
    return dot(v, v);
}

float2 Mod(float2 x, float2 y)
{
    return x - y * floor(x / y);
}
//OneMinusの計算を行う.
float OneMinus(float x)
{
    return 1.0f - x;
}
// Addの計算を行う.
float Add(float x, float y)
{
    return x + y;
}
// Subtractの計算を行う.
float Subtract(float x, float y)
{
    return x - y;
}
// Multiplyの計算を行う.
float Multiply(float x, float y)
{
    return x * y;
}
// Divideの計算を行う.
float Divide(float x, float y)
{
    return x / y;
}
// Smoothstepの計算を行う.
float Smoothstep(float edge0, float edge1, float x)
{
    float t = saturate((x - edge0) / (edge1 - edge0));
    return t * t * (3.0f - 2.0f * t);
}
// Lerpの計算を行う.
float Lerp(float x, float y, float a)
{
    return x + (y - x) * a;
}
//Absoluteの計算を行う.
float Absolute(float x)
{
    return abs(x);
}
//Posterizeの計算を行う.
float Posterize(float x, float levels)
{
    return floor(x * levels) / levels;
}
//ホログラムの計算を行う.
float Hologram(float x, float y, float z, float w)
{
    return x * y * z * w;
}


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
