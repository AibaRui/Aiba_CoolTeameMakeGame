float4 FragmentMain(Vertex2Fragment i): SV_Target
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
