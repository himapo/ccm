// Lights
float3	DirLight0Direction;
float3	DirLight0DiffuseColor;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

// Textures
texture AlbedoMap;
sampler AlbedoMapSampler = sampler_state
{
	Texture = (AlbedoMap);
};

texture PositionMap;
sampler PositionMapSampler = sampler_state
{
	Texture = (PositionMap);
};

texture NormalDepthMap;
sampler NormalDepthMapSampler = sampler_state
{
	Texture = (NormalDepthMap);
};

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

struct PSOutput
{
	float4	Color		: COLOR0;
};

VSOutput VSMain(VSInput input)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

float4 PSMain(VSOutput input) : COLOR
{
	float4 output;
	
	//float3 position = tex2D(PositionMapSampler, input.TexCoord).rgb;
	
	float4 normalDepth = tex2D(NormalDepthMapSampler, input.TexCoord);
	float3 normal = normalDepth.rgb * 2.0f - 1.0f;
	//float depth = normalDepth.a;
	
	float4 albedo = tex2D(AlbedoMapSampler, input.TexCoord);
	
	float diffuseIntensity = dot(normal, normalize(-DirLight0Direction));
	float3 diffuse = albedo.rgb * DirLight0DiffuseColor * diffuseIntensity;
	
	output = float4(diffuse, albedo.a);
	
	return output;
}

Technique Deferred
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSMain();
	}
}
