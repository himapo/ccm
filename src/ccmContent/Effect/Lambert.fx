#include "LightMapping.fxh"
#include "Shadow.fxh"

// Materials
float3	DiffuseColor;
float	Alpha;

// Other parameters

// Lights
float3	AmbientLightColor;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

// Textures
texture ModelTexture;
sampler ModelTextureSampler = sampler_state
{
	Texture = (ModelTexture);
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
	float2	TexCoord	: TEXCOORD0;
	float4	PositionWS	: TEXCOORD1;	// Position in world space
};

VSOutput VSMain(VSInput input,
	uniform bool useTexture)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, World);
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

float4 PSMain(VSOutput input,
	uniform bool useTexture,
	uniform bool shadowEnabled) : COLOR
{
	float4 output;
	
	// 射影空間でのこのピクセルの位置を見つける
	float4 projPosition = mul(mul(input.PositionWS, View), Projection);
	
	// ライト マップに格納された放射輝度を取得する
	float3 radiance = GetDiffuseRadiance(projPosition);
	
	// マテリアルカラー、アンビエントライトと合成する
	float3 d = saturate(DiffuseColor * radiance + AmbientLightColor);
	
	output = float4(d.rgb, Alpha);
	
	if(useTexture)
	{
		output *= tex2D(ModelTextureSampler, input.TexCoord);
	}
	
	if(shadowEnabled)
	{
	    output *= CalcShadow(input.PositionWS);
	}
	
	return output;
}

Technique LightMap
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSMain(false);
		PixelShader		= compile ps_2_0 PSMain(false, false);
	}
}

Technique LightMapShadow
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSMain(false);
		PixelShader		= compile ps_2_0 PSMain(false, true);
	}
}

Technique LightMapModTexture
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSMain(true);
		PixelShader		= compile ps_2_0 PSMain(true, false);
	}
}
