// Other parameters
const float		Alpha		: register(c0) = 1;

// Matrices
const float4x4	World		: WORLD;
const float4x4	View		: VIEW;
const float4x4	Projection	: PROJECTION;

// Textures
uniform const texture DiffuseMap;

uniform const sampler DiffuseMapSampler : register(s0) = sampler_state
{
	Texture = (DiffuseMap);
	MipFilter = Point;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

//-----------------------------------------------------------------------------
// Vertex shader inputs
//-----------------------------------------------------------------------------

struct VSInputNm
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
};

struct VSInputTx
{
	float4	Position	: POSITION;
	float2	TexCoord	: TEXCOORD0;
};

struct VSInputVc
{
	float4	Position	: POSITION;
	float4	Color		: COLOR0;
};

struct VSInputNmTx
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
};

struct VSInputNmVc
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float4	Color		: COLOR0;
};

struct VSInputTxVc
{
	float4	Position	: POSITION;
	float2	TexCoord	: TEXCOORD0;
	float4	Color		: COLOR0;
};

struct VSInputNmTxVc
{
	float4	Position	: POSITION;
	float3	Normal		: NORMAL;
	float2	TexCoord	: TEXCOORD0;
	float4	Color		: COLOR0;
};

struct VSOutput
{
	float4	PositionPS	: POSITION;		// Position in projection space
	float4	Diffuse		: COLOR0;
	float2	TexCoord	: TEXCOORD0;
};

struct PSInput
{
	float4	Diffuse		: COLOR0;
	float2	TexCoord	: TEXCOORD0;
};

//-----------------------------------------------------------------------------
// Vertex shaders
//-----------------------------------------------------------------------------

VSOutput VSMainNm(VSInputNm vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;
	
	vout.Diffuse = 1;

	return vout;
}

VSOutput VSMainTx(VSInputTx vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;

	vout.TexCoord = vin.TexCoord;
	
	vout.Diffuse = 1;

	return vout;
}

VSOutput VSMainVc(VSInputVc vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;
	
	vout.Diffuse = vin.Color;

	return vout;
}

VSOutput VSMainNmTx(VSInputNmTx vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;
	
	vout.TexCoord = vin.TexCoord;
	
	vout.Diffuse = 1;

	return vout;
}

VSOutput VSMainNmVc(VSInputNmVc vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;
	
	vout.Diffuse = vin.Color;

	return vout;
}

VSOutput VSMainTxVc(VSInputTxVc vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;

	vout.TexCoord = vin.TexCoord;
	
	vout.Diffuse = vin.Color;

	return vout;
}

VSOutput VSMainNmTxVc(VSInputNmTxVc vin)
{
	VSOutput vout = (VSOutput)0;
	
	float4 pos_ws = mul(vin.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	vout.PositionPS	= pos_ps;

	vout.TexCoord = vin.TexCoord;
	
	vout.Diffuse = vin.Color;

	return vout;
}

//-----------------------------------------------------------------------------
// Pixel shaders
//-----------------------------------------------------------------------------

float4 PSMain(PSInput pin,
	uniform bool useTexture) : COLOR
{
	float4 diffuseTextureColor = tex2D(DiffuseMapSampler, pin.TexCoord);
	
	float4 output = pin.Diffuse;
	
	if(useTexture)
	{
		output *= diffuseTextureColor;
	}

	output.a = Alpha;
	
	return output;
}

Technique TechniqueNm
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainNm();
		PixelShader		= compile ps_2_0 PSMain(false);
	}

}
Technique TechniqueTx
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainTx();
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}

Technique TechniqueVc
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainVc();
		PixelShader		= compile ps_2_0 PSMain(false);
	}
}

Technique TechniqueNmTx
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainNmTx();
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}

Technique TechniqueNmVc
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainNmVc();
		PixelShader		= compile ps_2_0 PSMain(false);
	}
}

Technique TechniqueTxVc
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainTxVc();
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}

Technique TechniqueNmTxVc
{
	Pass P0
	{
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;

		VertexShader	= compile vs_2_0 VSMainNmTxVc();
		PixelShader		= compile ps_2_0 PSMain(true);
	}
}
