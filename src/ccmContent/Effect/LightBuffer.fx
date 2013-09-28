// Lights
float3	DirLight0Direction;
float3	DirLight0DiffuseColor;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

// Textures
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
	float4	PositionPS	: POSITION;
	float2	TexCoord	: TEXCOORD0;
};

struct PSOutput
{
	float4	Diffuse		: COLOR0;
	float4	Specular	: COLOR1;
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

PSOutput PSDirectional(VSOutput input)
{
	PSOutput output;
	
	float4 normalDepth = tex2D(NormalDepthMapSampler, input.TexCoord);
	float3 normal = normalDepth.rgb * 2.0f - 1.0f;
	//float depth = normalDepth.a;
	
	float diffuseIntensity = dot(normal, normalize(-DirLight0Direction));
	float3 diffuse = DirLight0DiffuseColor * diffuseIntensity;
	
	output.Diffuse = float4(diffuse, 1.0f);
	
	output.Specular = 0.0f;
	
	return output;
}

PSOutput PSPoint(VSOutput input)
{
	PSOutput output;
	
	float4 normalDepth = tex2D(NormalDepthMapSampler, input.TexCoord);
	float3 normal = normalDepth.rgb * 2.0f - 1.0f;
	//float depth = normalDepth.a;
	
	float diffuseIntensity = dot(normal, normalize(-DirLight0Direction));
	float3 diffuse = DirLight0DiffuseColor * diffuseIntensity;
	
	output.Diffuse = float4(diffuse, 1.0f);
	
	output.Specular = 0.0f;
	
	return output;
}

Technique Directional
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSDirectional();
	}
}

Technique Point
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain();
		PixelShader		= compile ps_2_0 PSPoint();
	}
}
