#include "LightMapping.fxh"
#include "Shadow.fxh"

// Materials
float3	DiffuseColor;
float3	SpecularColor;
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

VSOutput VSStatic(VSInput input)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, World);
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

VSOutput VSInstancing(VSInput input, float4x4 instanceTransform : BLENDWEIGHT)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, mul(World, transpose(instanceTransform)));
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

float3 ToneMapping(float3 hdr)
{
	return float3(1,1,1) - exp(-hdr * 0.5f);
}

float4 PSMain(VSOutput input,
	uniform bool useTexture,
	uniform bool shadowEnabled) : COLOR
{
	float4 output;
	
	// 射影空間でのこのピクセルの位置を見つける
	float4 projPosition = mul(mul(input.PositionWS, View), Projection);
	
	// ライト マップに格納された放射輝度を取得する
	float3 diffuse;
	float3 specular;
	GetRadiance(diffuse, specular, projPosition);
	
	// マテリアルカラー、アンビエントライトと合成する
	//float3 light = saturate(DiffuseColor * diffuse + AmbientLightColor);
	//float3 light = saturate(DiffuseColor * diffuse + SpecularColor * specular + AmbientLightColor);
	float3 light = DiffuseColor * diffuse + SpecularColor * specular + AmbientLightColor;
	
	output = float4(light.rgb, Alpha);
	
	if(useTexture)
	{
		output *= tex2D(ModelTextureSampler, input.TexCoord);
	}
	
	if(shadowEnabled)
	{
	    output *= CalcShadow(input.PositionWS);
	}
	
	//return float4(ToneMapping(output.rgb), output.a);
	return ToLDR(output.rgb);
}

Technique Static
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(false, false);
	}
}

Technique StaticShadow
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(false, true);
	}
}

Technique StaticModTexture
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(true, false);
	}
}

Technique StaticModTextureShadow
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSStatic();
		PixelShader		= compile ps_2_0 PSMain(true, true);
	}
}

Technique Instancing
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSInstancing();
		PixelShader		= compile ps_2_0 PSMain(false, false);
	}
}

Technique InstancingShadow
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSInstancing();
		PixelShader		= compile ps_2_0 PSMain(false, true);
	}
}

Technique InstancingModTexture
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSInstancing();
		PixelShader		= compile ps_2_0 PSMain(true, false);
	}
}

Technique InstancingModTextureShadow
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSInstancing();
		PixelShader		= compile ps_2_0 PSMain(true, true);
	}
}
