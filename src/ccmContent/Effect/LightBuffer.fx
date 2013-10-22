// Lights
struct DirectionalLight
{
	float3 Direction;
	float3 Color;
};

DirectionalLight gDirectionalLight;

struct PointLight
{
	float3 Position;
	float AttenuationBegin;
	float3 Color;
	float AttenuationEnd;
};

PointLight gPointLight;

// Materials

// Other
float3	EyePosition;
float2	NearPlaneSize;
float	Near;
float	Far;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

float4x4	InvView;

// Textures
texture NormalMap;
sampler NormalMapSampler = sampler_state
{
	Texture = (NormalMap);
};

texture DepthMap;
sampler DepthMapSampler = sampler_state
{
	Texture = (DepthMap);
	MipFilter = Point;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VSInputDirectional
{
	float4	Position	: POSITION;
	float2	TexCoord	: TEXCOORD0;
};

struct VSInputPoint
{
	float4	Position	: POSITION;
};

struct VSOutputDirectional
{
	float4	PositionPS	: POSITION;
	float2	TexCoord	: TEXCOORD0;
	float4	PositionVS	: TEXCOORD1;
};

struct VSOutputPoint
{
	float4	Position	: POSITION;
	float4	PositionVS	: TEXCOORD0;
	float4	PositionPS	: TEXCOORD1;
};

struct PSOutput
{
	float4	Diffuse		: COLOR0;
	float4	Specular	: COLOR1;
};

VSOutputDirectional VSDirectional(VSInputDirectional input)
{
	VSOutputDirectional output;
	
	float4 pos_ws = mul(input.Position, World);
	output.PositionVS = mul(pos_ws, View);
	output.PositionPS = mul(output.PositionVS, Projection);
	
	output.TexCoord = input.TexCoord;
	
	return output;
}

VSOutputPoint VSPoint(VSInputPoint input)
{
	VSOutputPoint output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.Position = pos_ps;
	
	output.PositionVS = pos_vs;
	output.PositionPS = pos_ps;
	
	return output;
}

float Lambert(float3 N, float3 L)
{
	return dot(N, L);
}

float BlinnPhong(float3 N, float3 L, float3 E, float m)
{
	float3 H = normalize(E + L);
	
	return pow(saturate(dot(N, H)), m);
}

// ���݃����_�����O���Ă���_renderPosVS�ƃX�N���[�����W�������Ő[�x��depth�̓_�̃r���[���W�����߂�
// depth�͐��`�[�x�ł��邱��
float4 CalcViewPosition(float4 renderPosVS, float depth)
{
	// ���`��depth����r���[��Ԃ�Z���W�͋��܂�
	float Q = Projection._m22;	// Q = Far / (Far - Near)
	float near = -Projection._m32 / Q;
	float far = Projection._m32 / (1.0f - Q);
	float zPS = depth * far;
	float zVS = (zPS / Q) + near;
	
	// �X�N���[�����W�������ꍇ�A�r���[��Ԃł͑����֌W�ɂ���
	float2 xyVS = renderPosVS.xy * zVS / renderPosVS.z;
	
	return float4(xyVS.xy, zVS, 1.0f);
}

float4 CalcViewPositionDirectional(float2 texCoord, float depth)
{
	float3 renderPosVS;	// �r���[��Ԃ̃j�A�v���[���ɂ����錻�݂̕`��ʒu
	renderPosVS.xy = (texCoord * float2(1.0f, -1.0f) + float2(-0.5f, 0.5f)) * NearPlaneSize;
	renderPosVS.z = Near;

	float zVS = depth * (Far - Near) + Near;
	float2 xyVS = renderPosVS.xy * zVS / renderPosVS.z;

	return float4(xyVS.xy, zVS, 1.0f);
}

PSOutput PSDirectional(VSOutputDirectional input)
{
	PSOutput output;
	
	float4 normalShininess = tex2D(NormalMapSampler, input.TexCoord);
	float3 normalWS = normalShininess.rgb * 2.0f - 1.0f;
	float shininess = normalShininess.a * 255.0f;
	float depth = tex2D(DepthMapSampler, input.TexCoord).r;
	
	float3 L = normalize(-gDirectionalLight.Direction);

	float diffuseIntensity = Lambert(normalWS, L);
	float3 diffuse = gDirectionalLight.Color * diffuseIntensity;
	
	// ���[���h��Ԃł̃W�I���g���̈ʒu�����߂�
	float4 posVS = CalcViewPositionDirectional(input.TexCoord, depth);
	float4 posWS = mul(posVS, InvView);

	float specularIntensity = BlinnPhong(normalWS, L, normalize(EyePosition - posWS.xyz), shininess);
	float3 specular = gDirectionalLight.Color * specularIntensity;

	output.Diffuse = float4(diffuse, 1.0f);
	output.Specular = float4(specular, 1.0f);
	
	return output;
}

float4 PSNull(VSOutputPoint input) : COLOR
{
	return float4(0, 0, 0, 0);
}

PSOutput PSPoint(VSOutputPoint input)
{
	PSOutput output = (PSOutput)0;
	
	// ���C�g���\�ʂ̃s�N�Z���̈ʒu
	float4 spherePositionVS = input.PositionVS;
	float4 spherePositionPS = input.PositionPS;
	
	// �e�N�X�`���ł̂��̃s�N�Z���̈ʒu��������
	float2 texCoord = 0.5 * spherePositionPS.xy / spherePositionPS.w + float2( 0.5, 0.5 );
	texCoord.y = 1.0f - texCoord.y;
	
	// �W�I���g���̖@���Ɛ[�x�����������Ă���
	float4 normalShininess = tex2D(NormalMapSampler, texCoord);
	float3 normalWS = normalShininess.rgb * 2.0f - 1.0f;
	//float3 normalVS = normalize(mul(float4(normalWS, 0), View).xyz);
	//float3 normalPS = normalize(mul(float4(normalVS, 0), Projection).xyz);
	float shininess = normalShininess.a * 255.0f;
	float depth = tex2D(DepthMapSampler, texCoord).r;
	
	// ���[���h��Ԃł̃W�I���g���̈ʒu�����߂�
	float4 posVS = CalcViewPosition(spherePositionVS, depth);
	float4 posWS = mul(posVS, InvView);
	
	// �����ƃW�I���g���̈ʒu�֌W���甽�˂��v�Z
	float3 lightPosWS = gPointLight.Position;
	//float4 lightPosVS = mul(float4(gPointLight.Position, 1), View);
	//float4 lightPosPS = mul(float4(gPointLight.Position, 1), viewProj);
	
	float3 lightDirection = posWS.xyz - lightPosWS;
	//float3 lightDirection = posVS - lightPosVS.xyz;
	//float3 lightDirection = posPS - lightPosPS.xyz;
	float lightDistance = length(lightDirection);
	
	float attenuation = lerp(1.0f, 0.0f, 
		(lightDistance - gPointLight.AttenuationBegin) / (gPointLight.AttenuationEnd - gPointLight.AttenuationBegin));
	attenuation = clamp(attenuation, 0.0f, 1.0f);
	//attenuation = 1.0f;

	float3 L = -lightDirection / lightDistance;
	
	float diffuseIntensity = Lambert(normalWS, L);
	float specularIntensity = BlinnPhong(normalWS, L, normalize(EyePosition - posWS.xyz), shininess);
	
	output.Diffuse = float4(gPointLight.Color * diffuseIntensity * attenuation, 1.0f);
	output.Specular = float4(gPointLight.Color * specularIntensity * attenuation, 1.0f);
	
	return output;
}

Technique Directional
{
	Pass P0
	{
		ZEnable = FALSE;
		ZWriteEnable = FALSE;
		StencilEnable = FALSE;
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSDirectional();
		PixelShader		= compile ps_2_0 PSDirectional();
	}
}

Technique Point
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = FALSE;
		
		// �W�I���g������O�ɂ���\�ʂ������X�e���V���Ƀ}�[�N
		ZFunc = LESS;
		StencilEnable = TRUE;
		StencilFunc = ALWAYS;
		StencilPass = REPLACE;
		//StencilFail = KEEP;
		StencilZFail = KEEP;
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
		//ColorWriteEnable = 0;		// �����0�ɂ���Ɛ[�x�o�b�t�@�������ɂȂ��Ă��܂�
		CullMode = CCW;
		
		VertexShader	= compile vs_2_0 VSPoint();
		PixelShader		= compile ps_2_0 PSNull();
	}
	
	Pass P1
	{
		ZEnable = TRUE;
		ZWriteEnable = FALSE;
		
		// P0�Ń}�[�N����A���W�I���g����艜�ɂ��闠�ʂ����Z�`��
		ZFunc = GREATER;
		StencilEnable = TRUE;
		StencilFunc = EQUAL;
		StencilPass = KEEP;
		StencilFail = KEEP;
		StencilZFail = KEEP;
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
		ColorWriteEnable = RED | GREEN | BLUE | ALPHA;
		CullMode = CW;
		
		VertexShader	= compile vs_2_0 VSPoint();
		PixelShader		= compile ps_2_0 PSPoint();
	}
}

// �J���������C�g�{�����[���̓����ɂ���ꍇ
Technique PointInLight
{
	Pass P0
	{
		ZEnable = TRUE;
		ZWriteEnable = FALSE;
		
		// �W�I���g����艜�ɂ��闠�ʂ����Z�`��
		ZFunc = GREATER;
		StencilEnable = FALSE;
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
		ColorWriteEnable = RED | GREEN | BLUE | ALPHA;
		CullMode = CW;
		
		VertexShader	= compile vs_2_0 VSPoint();
		PixelShader		= compile ps_2_0 PSPoint();
	}
}
