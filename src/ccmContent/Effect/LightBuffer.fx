// Lights
float3	DirLight0Direction;
float3	DirLight0DiffuseColor;

struct PointLight
{
	float3 Position;
	float AttenuationBegin;
	float3 Color;
	float AttenuationEnd;
};

PointLight gPointLight;

// Matrices
float4x4	World		: World;
float4x4	View		: View;
float4x4	Projection	: Projection;

float4x4	InvView;
float4x4	InvProj;

// Textures
texture NormalDepthMap;
sampler NormalDepthMapSampler = sampler_state
{
	Texture = (NormalDepthMap);
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

struct VSOutput
{
	float4	PositionPS	: POSITION;
	float2	TexCoord	: TEXCOORD0;
	float4	PositionWS	: TEXCOORD1;
};

struct PSOutput
{
	float4	Diffuse		: COLOR0;
	//float4	Specular	: COLOR1;
};

VSOutput VSDirectional(VSInputDirectional input)
{
	VSOutput output;
	
	float4 pos_ws = mul(input.Position, World);
	float4 pos_vs = mul(pos_ws, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = input.TexCoord;
	
	output.PositionWS = 0;
	
	return output;
}

VSOutput VSPoint(VSInputPoint input)
{
	VSOutput output;
	
	output.PositionWS = mul(input.Position, World);
	float4 pos_vs = mul(output.PositionWS, View);
	float4 pos_ps = mul(pos_vs, Projection);
	output.PositionPS = pos_ps;
	
	output.TexCoord = 0;
	
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
	
	//output.Specular = 0.0f;
	
	return output;
}

PSOutput PSPoint(VSOutput input)
{
	PSOutput output;
	
	float4x4 viewProj = mul(View, Projection);
	
	// 射影空間でのライト球表面のピクセルの位置を見つける
	float4 spherePositionPS = mul(input.PositionWS, viewProj);
	
	// テクスチャでのこのピクセルの位置を見つける
	float2 texCoord = 0.5 * spherePositionPS.xy / spherePositionPS.w + float2( 0.5, 0.5 );
	texCoord.y = 1.0f - texCoord.y;
	
	// ジオメトリの法線と深度を引っ張ってくる
	float4 normalDepth = tex2D(NormalDepthMapSampler, texCoord);
	float3 normalWS = normalDepth.rgb * 2.0f - 1.0f;
	//float3 normalVS = normalize(mul(float4(normalWS, 0), View).xyz);
	//float3 normalPS = normalize(mul(float4(normalVS, 0), Projection).xyz);
	float depth = normalDepth.a;
	
	// テクスチャのスクリーン座標
	//float2 texCoordSS = texCoord * float2(2, -2) + float2(-1, 1);
	
	// ビュー空間でのジオメトリの位置
	//float3 posVS;
	//posVS.z = Projection._m32 / (depth - Projection._m22);
	//posVS.xy = texCoordSS.xy * posVS.z / float2(Projection._m00, Projection._m11);
	
	// 射影空間でのジオメトリの位置
	float4 posPS = float4(spherePositionPS.xy, depth * spherePositionPS.w, spherePositionPS.w);
	float4 posVS = mul(posPS, InvProj);
	float4 posWS = mul(posVS, InvView);
	
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
	
	float diffuseIntensity = dot(normalWS, -lightDirection / lightDistance);
	
	output.Diffuse = float4(gPointLight.Color * diffuseIntensity * attenuation, 1.0f);
	
	//output.Specular = 1.0f;
	
	return output;
}

Technique Directional
{
	Pass P0
	{
		ZEnable = FALSE;
		ZWriteEnable = FALSE;
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
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
		AlphaBlendEnable = TRUE;
		BlendOp = ADD;
		SrcBlend = ONE;
		DestBlend = ONE;
		//CullMode = CCW;
		VertexShader	= compile vs_2_0 VSPoint();
		PixelShader		= compile ps_2_0 PSPoint();
	}
}
