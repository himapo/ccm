// Materials
const float3	DiffuseColor	: register(c0) = 1;
const float		Alpha			: register(c1) = 1;

// Other parameters

// Lights
const float3	AmbientLightColor;
const float3	DirLight0Direction;
const float3	DirLight0DiffuseColor;

// Matrices
const float4x4	World		: World;
const float4x4	View		: View;
const float4x4	Projection	: Projection;

// Textures
uniform const texture DiffuseMap;

uniform const sampler DiffuseMapSampler : register(s0) = sampler_state
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
};

struct PSInput
{
	float4	Diffuse		: COLOR0;
	float2	TexCoord	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
};

VSOutput VSMain(VSInput vin,
	uniform bool pixelLighting,
	uniform bool useTexture)
{
	VSOutput vout;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;

	if(pixelLighting)
	{
		vout.Diffuse = float4(1, 1, 1, Alpha);
	}
	else
	{
		float diffuseIntensity = saturate(dot(vin.Normal, -DirLight0Direction));
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		vout.Diffuse = float4(d.rgb, Alpha);
	}
	
	vout.TexCoord = vin.TexCoord;
	
	vout.Normal = normalize(mul(vin.Normal, (float3x3)World));
	
	return vout;
}

float4 PSMain(PSInput pin,
	uniform bool pixelLighting,
	uniform bool useTexture) : COLOR
{
	float4 diffuseTextureColor = tex2D(DiffuseMapSampler, pin.TexCoord);
	
	float4 output;
	
	if(pixelLighting)
	{
		float diffuseIntensity = saturate(dot(pin.Normal, -DirLight0Direction));
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		output = float4(d.rgb, Alpha);
	}
	else
	{
		output = pin.Diffuse;
	}
	
	if(useTexture)
	{
		output *= diffuseTextureColor;
	}
	
	return output;
}

Technique VertexLighting
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(false, false);
		PixelShader		= compile ps_2_0 PSMain(false, false);
	}
}

Technique VertexLightingTexture
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(false, true);
		PixelShader		= compile ps_2_0 PSMain(false, true);
	}
}

Technique PixelLighting
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true, false);
		PixelShader		= compile ps_2_0 PSMain(true, false);
	}
}

Technique PixelLightingTexture
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true, true);
		PixelShader		= compile ps_2_0 PSMain(true, true);
	}
}
