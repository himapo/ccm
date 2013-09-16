// Materials
float3	DiffuseColor;
float	Alpha;

// Other parameters

// Lights
 float3	AmbientLightColor;
float3	DirLight0Direction;
float3	DirLight0DiffuseColor;

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
};

VSOutput VSMain(VSInput input,
	uniform bool pixelLighting,
	uniform bool useTexture)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS	= pos_ps;

	if(pixelLighting)
	{
		output.Diffuse = float4(1, 1, 1, Alpha);
	}
	else
	{
		float diffuseIntensity = saturate(dot(input.Normal, -DirLight0Direction));
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		output.Diffuse = float4(d.rgb, Alpha);
	}
	
	output.TexCoord = input.TexCoord;
	
	output.Normal = normalize(mul(input.Normal, (float3x3)World));
	
	return output;
}

float4 PSMain(VSOutput input,
	uniform bool pixelLighting,
	uniform bool useTexture) : COLOR
{
	float4 diffuseTextureColor = tex2D(DiffuseMapSampler, input.TexCoord);
	
	float4 output;
	
	if(pixelLighting)
	{
		float diffuseIntensity = saturate(dot(input.Normal, -DirLight0Direction));
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		output = float4(d.rgb, Alpha);
	}
	else
	{
		output = input.Diffuse;
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
