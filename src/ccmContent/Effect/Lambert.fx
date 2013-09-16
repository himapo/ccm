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
float4x4	LightViewProjection;

float DepthBias = 0.005f;

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

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
	Texture = (ShadowMap);
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
	float4	PositionWS	: TEXCOORD2;	// Position in world space
};

VSOutput VSMain(VSInput input,
	uniform bool pixelLighting,
	uniform bool useTexture)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, World);
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;

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
	uniform bool useTexture,
	uniform bool shadowEnabled) : COLOR
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

	if(shadowEnabled)
	{
	    // ���C�g��Ԃł̂��̃s�N�Z���̈ʒu��������
		float4 lightingPosition = mul(input.PositionWS, LightViewProjection);

		// �V���h�E �}�b�v�ł̂��̃s�N�Z���̈ʒu��������
		float2 ShadowTexCoord = 0.5 * lightingPosition.xy / 
								lightingPosition.w + float2( 0.5, 0.5 );
		ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

		// �V���h�E �}�b�v�Ɋi�[���ꂽ���݂̐[�x���擾����
		float shadowdepth = tex2D(ShadowMapSampler, ShadowTexCoord).r;
        
		// ���݂̃s�N�Z���[�x���v�Z����
		// �o�C�A�X�́A�I�N���[�_�[�̃s�N�Z�����`�悳���Ƃ��ɋN����
		// ���������_�덷��h�~���邽�߂Ɏg�p�����
		float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;
    
		// ���̃s�N�Z�����V���h�E �}�b�v�Œl�̑O�ɂ��邩��ɂ��邩�𒲂ׂ�
		if (shadowdepth < ourdepth)
		{
			// �P�x��Ⴍ���邱�Ƃɂ���ăs�N�Z�����V���h�E����
			output *= float4(0.5, 0.5, 0.5, 1);
		};
	}
	
	return output;
}

Technique VertexLighting
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(false, false);
		PixelShader		= compile ps_2_0 PSMain(false, false, false);
	}
}

Technique VertexLightingTexture
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(false, true);
		PixelShader		= compile ps_2_0 PSMain(false, true, false);
	}
}

Technique PixelLighting
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true, false);
		PixelShader		= compile ps_2_0 PSMain(true, false, false);
	}
}

Technique PixelLightingShadow
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true, false);
		PixelShader		= compile ps_2_0 PSMain(true, false, true);
	}
}

Technique PixelLightingTexture
{
	Pass P0
	{
		VertexShader	= compile vs_2_0 VSMain(true, true);
		PixelShader		= compile ps_2_0 PSMain(true, true, false);
	}
}
