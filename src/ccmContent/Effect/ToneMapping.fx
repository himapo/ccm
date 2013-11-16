#include "PseudoHDR.fxh"

// Other parameters
static const int MAX_SAMPLES = 16;
float2 SampleOffsets[MAX_SAMPLES];

// Y = 0.2126 × R + 0.7152 × G + 0.0722 × B
static const float3 LUMINANCE_VECTOR  = float3(0.2126f, 0.7152f, 0.0722f);

// Matrices
const float4x4	World		: WORLD;
const float4x4	View		: VIEW;
const float4x4	Projection	: PROJECTION;

// Textures
uniform const texture Texture0;

uniform const sampler Texture0Sampler : register(s0) = sampler_state
{
	Texture = (Texture0);
	MipFilter = Point;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

uniform const texture Texture1;

uniform const sampler Texture1Sampler : register(s1) = sampler_state
{
	Texture = (Texture1);
	MipFilter = Point;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

//-----------------------------------------------------------------------------
// Vertex shader inputs
//-----------------------------------------------------------------------------

struct VSInput
{
	float4	Position	: POSITION;
	float2	TexCoord	: TEXCOORD0;
};

struct VSOutput
{
	float4	PositionPS	: POSITION;		// Position in projection space
	float2	TexCoord	: TEXCOORD0;
};

//-----------------------------------------------------------------------------
// Vertex shaders
//-----------------------------------------------------------------------------

VSOutput VSMain(VSInput input)
{
	VSOutput output = (VSOutput)0;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS	= pos_ps;

	output.TexCoord = input.TexCoord;

	return output;
}

//-----------------------------------------------------------------------------
// Pixel shaders
//-----------------------------------------------------------------------------

// 縮小バッファから平均輝度のlogに変換
float4 SampleLumInitial(VSOutput input) : COLOR
{
    float3 vSample = 0.0f;
    float  fLogLumSum = 0.0f;

    for(int iSample = 0; iSample < 15; iSample++)
    {
        // Compute the sum of log(luminance) throughout the sample points
        vSample = ToHDR(tex2D(Texture0Sampler, input.TexCoord+SampleOffsets[iSample]));
        fLogLumSum += log(dot(vSample, LUMINANCE_VECTOR)+0.0001f);
    }
    
    // Divide the sum to complete the average
    fLogLumSum /= 15;

    return float4(fLogLumSum, fLogLumSum, fLogLumSum, 1.0f);
}

// 平均輝度を繰り返しダウンサンプリング
float4 SampleLumIterative(VSOutput input) : COLOR
{
    float fResampleSum = 0.0f; 
    
    for(int iSample = 0; iSample < 16; iSample++)
    {
        // Compute the sum of luminance throughout the sample points
        fResampleSum += tex2D(Texture0Sampler, input.TexCoord+SampleOffsets[iSample]);
    }
    
    // Divide the sum to complete the average
    fResampleSum /= 16;

    return float4(fResampleSum, fResampleSum, fResampleSum, 1.0f);
}

// 1x1バッファにダウンサンプリングするときはexpを取る
float4 SampleLumFinal(VSOutput input) : COLOR
{
    float fResampleSum = 0.0f;
    
    for(int iSample = 0; iSample < 16; iSample++)
    {
        // Compute the sum of luminance throughout the sample points
        fResampleSum += tex2D(Texture0Sampler, input.TexCoord+SampleOffsets[iSample]);
    }
    
    // Divide the sum to complete the average, and perform an exp() to complete
    // the average luminance calculation
    fResampleSum = exp(fResampleSum/16);
    
    return float4(fResampleSum, fResampleSum, fResampleSum, 1.0f);
}

float4 PSFinalPass(VSOutput input) : COLOR
{
	float4 HDRColor = float4(ToHDR(tex2D(Texture0Sampler, input.TexCoord)), 1.0f);

	float luminance = tex2D(Texture1Sampler, float2(0.5f, 0.5f));

	HDRColor.rgb *= 1.0f / (luminance + 0.001f);
	HDRColor.rgb /= (1.0f + HDRColor);

	return HDRColor;
}

technique SampleAvgLum
{
    pass P0
    {
		VertexShader	= compile vs_3_0 VSMain();
        PixelShader		= compile ps_3_0 SampleLumInitial();
    }
}

technique ResampleAvgLum
{
    pass P0
    {
		VertexShader	= compile vs_2_0 VSMain();
        PixelShader		= compile ps_2_0 SampleLumIterative();
    }
}

technique ResampleAvgLumExp
{
    pass P0
    {
		VertexShader	= compile vs_2_0 VSMain();
        PixelShader		= compile ps_2_0 SampleLumFinal();
    }
}

Technique FinalPass
{
	Pass P0
	{
		ZEnable = FALSE;
		ZWriteEnable = FALSE;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;

		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSFinalPass();
	}
}
