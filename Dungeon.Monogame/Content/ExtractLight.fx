#include "Macros.fxh"

sampler TextureSampler : register(s0);

float4 main(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(TextureSampler, texCoord);
    float BloomThreshold = 0.05;

    return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}

technique ThresholdEffect
{
    pass DefaultPass
    {
        PixelShader = compile ps_2_0 main();
    }
}
