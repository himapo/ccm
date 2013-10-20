// Materials
float3	DiffuseColor;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

// Textures
texture DiffuseMap;
sampler DiffuseMapSampler = sampler_state
{
	Texture = (DiffuseMap);
	MipFilter = Linear;
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VSInput
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
};

struct VSOutput
{
	float4	PositionPS	: POSITION;
	float2	TexCoord	: TEXCOORD0;
	float4	NormalDepth	: TEXCOORD1;
	float4	PositionWS	: TEXCOORD2;
};

struct PSOutput
{
	float4	Albedo		: COLOR0;
	float4	PositionWS	: COLOR1;
	float4	NormalDepth	: COLOR2;
	float4	Dummy3		: COLOR3;
};

VSOutput VSStatic(VSInput input)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, World);
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	output.NormalDepth.rgb = (normalize(mul(input.Normal, (float3x3)World)) + 1.0f) * 0.5f;
	
	output.NormalDepth.a = output.PositionPS.z / output.PositionPS.w;
	
	return output;
}

PSOutput PSMain(VSOutput input,
	uniform bool useTexture)
{
	PSOutput output;
	
	output.PositionWS = input.PositionWS;
	
	output.Albedo = float4(DiffuseColor, 1.0f);
	
	if(useTexture)
	{
		output.Albedo *= tex2D(DiffuseMapSampler, input.TexCoord);
	}
	
	output.NormalDepth = input.NormalDepth;
	
	output.Dummy3 = 0;
	
	return output;
}

Technique GBuffer
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(false);
	}
}

Technique GBufferTexture
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}
