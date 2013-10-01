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
float4x4	LightViewProjection;

float DepthBias = 0.005f;

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

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
	Texture = (ShadowMap);
};

texture DiffuseLightMap;
sampler DiffuseLightMapSampler = sampler_state
{
	Texture = (DiffuseLightMap);
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
	
	// ライト マップでのこのピクセルの位置を見つける
	float2 LightTexCoord = 0.5 * projPosition.xy / projPosition.w + float2( 0.5, 0.5 );
	LightTexCoord.y = 1.0f - LightTexCoord.y;
	
	// ライト マップに格納された放射輝度を取得する
	float3 radiance = tex2D(DiffuseLightMapSampler, LightTexCoord).rgb;
	
	// マテリアルカラー、アンビエントライトと合成する
	float3 d = saturate(DiffuseColor * radiance + AmbientLightColor);
	
	output = float4(d.rgb, Alpha);
	
	
	if(useTexture)
	{
		output *= tex2D(ModelTextureSampler, input.TexCoord);
	}
	
	if(shadowEnabled)
	{
	    // ライト空間でのこのピクセルの位置を見つける
		float4 lightingPosition = mul(input.PositionWS, LightViewProjection);
		
		// シャドウ マップでのこのピクセルの位置を見つける
		float2 ShadowTexCoord = 0.5 * lightingPosition.xy / lightingPosition.w + float2( 0.5, 0.5 );
		ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;
		
		// シャドウ マップに格納された現在の深度を取得する
		float shadowdepth = tex2D(ShadowMapSampler, ShadowTexCoord).r;
        
		// 現在のピクセル深度を計算する
		// バイアスは、オクルーダーのピクセルが描画されるときに起きる
		// 浮動小数点誤差を防止するために使用される
		float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
		
		// このピクセルがシャドウ マップで値の前にあるか後にあるかを調べる
		if (shadowdepth < ourdepth)
		{
			// 輝度を低くすることによってピクセルをシャドウする
			output *= float4(0.5, 0.5, 0.5, 1);
		};
	}
	
	return output;
}

Technique LightMap
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSMain(false);
		PixelShader		= compile ps_2_0 PSMain(false, false);
	}
}

Technique LightMapShadow
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSMain(false);
		PixelShader		= compile ps_2_0 PSMain(false, true);
	}
}

Technique LightMapModTexture
{
	Pass P0
	{
		AlphaBlendEnable = FALSE;
		VertexShader	= compile vs_2_0 VSMain(true);
		PixelShader		= compile ps_2_0 PSMain(true, false);
	}
}
