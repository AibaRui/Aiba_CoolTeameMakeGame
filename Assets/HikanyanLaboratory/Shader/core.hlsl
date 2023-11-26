CBUFFER_START(UnityPerDraw)

float4    _MainColor;
float4    _MainTex_ST;

bool      _IsOutlined;
float4    _OutlineColor;
float     _OutlineWidth;

bool      _IsRimLight;
float4    _RimColor;
float     _LambertThresh;

bool      _IsEmission;
float4    _EmissionColor;

CBUFFER_END

sampler2D _MainTexture;
TEXTURE2D (_MainTex);
SAMPLER   (sampler_MainTex);
sampler2D _MatCap;
sampler2D _EmissionTex;


struct Appdata_full {
    float4 vertex : POSITION;
    float4 tangent : TANGENT;
    float3 normal : NORMAL;
    float2 uv     : TEXCOORD0;
};
struct Vertex2Fragment
{
    float4 vertex : SV_POSITION;
    float4 tangent : TANGENT;
    float3 normal : NORMAL;
    float2 uv     : TEXCOORD1;
};
