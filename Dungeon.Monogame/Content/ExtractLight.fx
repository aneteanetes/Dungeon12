// This shader gets the areas that are bright. This will later be blured making bright spots “glow”

sampler TextureSampler : register(s0);

 

// Get the threshold of what brightness level we want to glow


 

 

float4 main(float2 texCoord : TEXCOORD0) : COLOR0
{
	
    float Threshold = 0.15;
    float4 Color = tex2D(TextureSampler, texCoord);

   

    // Get the bright areas that is brighter than Threshold and return it.

    return saturate((Color- Threshold) / (1- Threshold));

}

 

 

technique Bloom
{

    pass P0
    {

            // A post process shader only needs a pixel shader.

        PixelShader = compile ps_2_0 main();

    }

}