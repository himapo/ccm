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
float4x4 World;
float4x4 View;
float4x4 Projection;


struct VertexShaderInput
{
    float4	Position	: POSITION0;
    float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
};

struct VertexShaderOutput
{
    float4	PositionPS	: POSITION0;		// Position in projection space
	float4	Diffuse		: COLOR0;
	float4	Specular	: COLOR1;
	float2	TexCoord	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
	float4	PositionWS	: TEXCOORD2;		// Position in world space
};

VertexShaderOutput VertexShaderFunction(
	VertexShaderInput input, 
	float4x4 instanceTransform : BLENDWEIGHT)
{
    VertexShaderOutput output;

	float4x4 transform = mul(World, transpose(instanceTransform));

    float4 worldPosition = mul(input.Position, transform);
    float4 viewPosition = mul(worldPosition, View);
    output.PositionPS = mul(viewPosition, Projection);
	output.PositionWS = worldPosition;

    output.TexCoord = input.TexCoord;

	output.Normal = normalize(mul(input.Normal, (float3x3)transform));

	output.Diffuse = float4(1, 1, 1, Alpha);
	output.Specular = 1;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 output;

    float3 N = input.Normal;
	float3 E = normalize(EyePosition - input.PositionWS.xyz);
	float3 L = -DirLight0Direction;
	float3 H = normalize(E + L);
		
	float2 ret = lit(dot(N, L), dot(N, H), SpecularPower).yz;

	// Half Lambert lighting.
	float diffuseIntensity = ret.x * 0.5f + 0.5f;
	diffuseIntensity = diffuseIntensity * diffuseIntensity;
		
	float3 d = saturate(DiffuseColor * DirLight0DiffuseColor * diffuseIntensity + AmbientLightColor);
	float3 s = SpecularColor * DirLight0SpecularColor * ret.y;
		
	output = float4(d + s, Alpha);

    return output;
}

technique PixelLighting
{
    pass Pass1
    {
        // TODO: ここでレンダーステートを設定します。
		AlphaBlendEnable = FALSE;
		
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
