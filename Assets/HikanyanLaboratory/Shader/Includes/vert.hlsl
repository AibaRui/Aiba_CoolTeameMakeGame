#ifndef  INCLUDE_GUARD_AE7F6B9F9F0B4B0F9
#define  INCLUDE_GUARD_AE7F6B9F9F0B4B0F9
Vertex2Fragment internal_constructor_Vertex2Fragment(Appdata_full v)
{
    Vertex2Fragment _auto_generated_initializer_ = (Vertex2Fragment) 0;
    VertexPositionInputs inputs = GetVertexPositionInputs(v.vertex.xyz);
    // スクリーン座標に変換.
    _auto_generated_initializer_.vertex = inputs.positionCS;
    // ワールド座標系変換.
    _auto_generated_initializer_.normal = normalize(TransformObjectToWorldNormal(v.normal));
                
    _auto_generated_initializer_.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
    return _auto_generated_initializer_;
}
Vertex2Fragment VertexMain(Appdata_full i)
{
    return internal_constructor_Vertex2Fragment(i);
}
#endif