#include "Const.fxh"
#include "BoneTexture.fxh"

// Materials
float4	Shininess;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

// Textures

struct VSInput
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
};

struct VSInputSkinning
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
	float4	BoneIndices : BLENDINDICES0;
    float4	BoneWeights : BLENDWEIGHT0;
};

struct VSOutput
{
	float4	Position	: POSITION;
	float4	PositionPS	: TEXCOORD0;
	float3	Normal		: TEXCOORD1;
};

struct PSOutput
{
	float4	Dummy		: COLOR0;
	float4	Normal		: COLOR1;
	float4	Depth		: COLOR2;
};

VSOutput VSStatic(VSInput input)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	output.Position = mul(pos_vs, Projection);
	
	output.PositionPS = output.Position;
	
	output.Normal = input.Normal;
	
	return output;
}

VSOutput VSInstancing(VSInput input, float4x4 instanceTransform : BLENDWEIGHT)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, mul(World, transpose(instanceTransform)));
	float4 pos_vs = mul(pos_ws, View);
	output.Position = mul(pos_vs, Projection);
	
	output.PositionPS = output.Position;
	
	output.Normal = input.Normal;
	
	return output;
}

VSOutput VSSkinning(VSInputSkinning input)
{
	VSOutput output = (VSOutput)0;
	
	float4x4 skinTransform = CreateTransformFromBoneTexture( input.BoneIndices, input.BoneWeights );

	float4 pos_ws = mul(input.Position, mul( skinTransform, World ));
	float4 pos_vs = mul(pos_ws, View);
	output.Position = mul(pos_vs, Projection);
	
	output.PositionPS = output.Position;
	
	output.Normal = input.Normal;
	
	return output;
}

PSOutput PSMain(VSOutput input)
{
	PSOutput output = (PSOutput)0;
	
	float3 normal = mul(float4(input.Normal, 0), World).xyz;
	//float3 normal = mul(mul(float4(input.Normal, 0), World), View).xyz;
	
	output.Normal.rgb = (normalize(normal) + 1.0f) * 0.5f;
	output.Normal.a = clamp(Shininess, 1.0f, SHININESS_MAX) / SHININESS_MAX;
	
	float Q = Projection._m22;		// Q = Far / (Far - Near)
	float far = Projection._m32 / (1.0f - Q);
	
	output.Depth.r = input.PositionPS.z / far;
	
	output.Dummy = 0;
	
	return output;
}

Technique Static
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain();
	}
}

Technique Instancing
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSInstancing();
		PixelShader		= compile ps_2_0 PSMain();
	}
}

Technique Skinning
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_3_0 VSSkinning();
		PixelShader		= compile ps_3_0 PSMain();
	}
}
