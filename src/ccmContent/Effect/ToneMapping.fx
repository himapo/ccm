#include "PseudoHDR.fxh"

// Other parameters
const float		Alpha		: register(c0) = 1;

// Matrices
const float4x4	World		: WORLD;
const float4x4	View		: VIEW;
const float4x4	Projection	: PROJECTION;

// Textures
uniform const texture HDRScene;

uniform const sampler HDRSceneSampler : register(s0) = sampler_state
{
	Texture = (HDRScene);
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

float4 PSMain(VSOutput input) : COLOR
{
	float4 HDRColor = float4(ToHDR(tex2D(HDRSceneSampler, input.TexCoord)), 1.0f);

	return HDRColor;
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
		PixelShader		= compile ps_2_0 PSMain();
	}
}
