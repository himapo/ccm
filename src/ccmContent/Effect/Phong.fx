// Materials
const float3	DiffuseColor	: register(c0) = 1;
const float		Alpha			: register(c1) = 1;
const float3	EmissiveColor	: register(c2) = 0;
const float3	SpecularColor	: register(c3) = 1;
const float		SpecularPower	: register(c4) = 16;

// Other parameters
const float3	EyePosition;

// Lights
const float3	AmbientLightColor;
const float3	DirLight0Direction;
const float3	DirLight0DiffuseColor;
const float3	DirLight0SpecularColor;

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
	float4	Specular	: COLOR1;
	float2	TexCoord	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
	float4	PositionWS	: TEXCOORD2;
};

struct PSInput
{
	float4	Diffuse		: COLOR0;
	float4	Specular	: COLOR1;
	float2	TexCoord	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
	float4	PositionWS	: TEXCOORD2;
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
	vout.PositionWS = pos_ws;
	
	vout.TexCoord = vin.TexCoord;
	
	vout.Normal = normalize(mul(vin.Normal, (float3x3)World));

	if(pixelLighting)
	{
		vout.Diffuse = float4(1, 1, 1, Alpha);
		vout.Specular = 1;
	}
	else
	{
		float3 N = vout.Normal;
		float3 E = normalize(EyePosition - pos_ws);
		float3 L = -DirLight0Direction;
		float3 H = normalize(E + L);
		
		float2 ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;

		// Half Lambert lighting.
		float diffuseIntensity = ret.x * 0.5f + 0.5f;
		diffuseIntensity = diffuseIntensity * diffuseIntensity;
		
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		vout.Diffuse = float4(d.rgb, Alpha);
		
		vout.Specular = float4(SpecularColor * DirLight0SpecularColor * ret.y, 0);
	}
	
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
		float3 N = pin.Normal;
		float3 E = normalize(EyePosition - pin.PositionWS.xyz);
		float3 L = -DirLight0Direction;
		float3 H = normalize(E + L);
		
		float2 ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;

		// Half Lambert lighting.
		float diffuseIntensity = ret.x * 0.5f + 0.5f;
		diffuseIntensity = diffuseIntensity * diffuseIntensity;
		
		float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
		float3 s = SpecularColor * DirLight0SpecularColor * ret.y;
		
		output = float4(d + s, Alpha);
	}
	else
	{
		output = pin.Diffuse + pin.Specular;
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
