#include "PseudoHDR.fxh"

// Other parameters
static const int MAX_SAMPLES = 16;
float2 SampleOffsets[MAX_SAMPLES];

// Matrices
const float4x4	World		: WORLD;
const float4x4	View		: VIEW;
const float4x4	Projection	: PROJECTION;

// Textures
uniform const texture SrcBuffer;

uniform const sampler SrcBufferSampler : register(s0) = sampler_state
{
	Texture = (SrcBuffer);
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

float4 PSDownScale4x4(VSOutput input) : COLOR
{
    float4 sample = 0.0f;

	for( int i=0; i < 16; i++ )
	{
		sample += tex2D( SrcBufferSampler, input.TexCoord + SampleOffsets[i] );
	}
    
	return sample / 16;
}

float4 PSDownScale4x4PseudoHDR(VSOutput input) : COLOR
{
    float3 sample = 0.0f;

	for( int i=0; i < 16; i++ )
	{
		sample += ToHDR(tex2D( SrcBufferSampler, input.TexCoord + SampleOffsets[i] ));
	}

	return ToLDR(sample / 16);
}


Technique DownScale4x4
{
	Pass P0
	{
		ZEnable = FALSE;
		ZWriteEnable = FALSE;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;

		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSDownScale4x4();
	}
}

Technique DownScale4x4PseudoHDR
{
	Pass P0
	{
		ZEnable = FALSE;
		ZWriteEnable = FALSE;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;

		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSDownScale4x4PseudoHDR();
	}
}
