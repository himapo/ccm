// Materials
float3	DiffuseColor;
float	Alpha;

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
	float4	PositionPS	: POSITION;		// Position in projection space
	float4	Diffuse		: COLOR0;
	float2	TexCoord	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
	float	Depth		: TEXCOORD2;
};

struct PSOutput
{
	float4	Albedo		: COLOR0;
	float4	Depth		: COLOR1;
	float4	Normal		: COLOR2;
	float4	Dummy3		: COLOR3;
};

VSOutput VSMain(VSInput input,
	uniform bool useTexture)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.Diffuse = float4(DiffuseColor, Alpha);
	
	output.TexCoord = input.TexCoord;
	
	output.Normal = normalize(mul(input.Normal, (float3x3)World));
	
	output.Depth = output.PositionPS.z / output.PositionPS.w;
	
	return output;
}

PSOutput PSMain(VSOutput input,
	uniform bool useTexture)
{
	PSOutput output;
	
	output.Albedo = input.Diffuse;
	
	if(useTexture)
	{
		output.Albedo *= tex2D(DiffuseMapSampler, input.TexCoord);
	}
	
	output.Depth.rgb = input.Depth;
	output.Depth.a = 1.0f;
	
	output.Normal = float4(input.Normal, 1.0f);
	
	output.Dummy3 = 0;
	
	return output;
}

Technique GBuffer
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(false);
		PixelShader		= compile ps_2_0 PSMain(false);
	}
}

Technique GBufferTexture
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true);
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}
