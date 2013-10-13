//-----------------------------------------------------------------------------
// SkinnedModel.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//
// このソースコードはクリエータークラブオンラインのSkinning Sampleの
// SkinnedModel.fxのコメントを翻訳、変更したもの
// http://creators.xna.com/en-US/sample/skinnedmodel
//
// 変更点としては、定数レジスタを使う代わりに頂点テクスチャを使っている
// 定数レジスタを使用していないので、ボーン数の制限が256に増えている
//-----------------------------------------------------------------------------

#include "BoneTexture.fxh"

//-----------------------------------------------------------------------------
// 定数レジスタ宣言
//=============================================================================
float4x4 World;			// オブジェクトのワールド座標
float4x4 View;
float4x4 Projection;

float4x4 LightViewProjection;
float DepthBias = 0.005f;

float3 Light1Direction = normalize(float3(1, 1, -2));
float3 Light1Color = float3(0.9, 0.8, 0.7);

float3 Light2Direction = normalize(float3(-1, -1, 1));
float3 Light2Color = float3(0.1, 0.3, 0.8);

float3 AmbientColor = 0.2;

float3 MaterialDiffuse = 1.0;
float3 MaterialEmissive = 0.0;
float3 MaterialSpecular = 1.0;
float MaterialSpecularPower = 1.0;

string TechniqueName;	// モデルプロセッサから指定するためにこれは必要

texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
	Texture = (ShadowMap);
	MipFilter = Point;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

//-----------------------------------------------------------------------------
// 構造体宣言
//=============================================================================
// 頂点シェーダー入力構造体
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

// 頂点シェーダー出力構造体
struct VS_OUTPUT
{
    float4 Position		: POSITION0;
    float3 Lighting		: COLOR0;
    float2 TexCoord		: TEXCOORD0;
	float4 PositionWS	: TEXCOORD1;
};

//-----------------------------------------------------------------------------
// 頂点シェーダー
//=============================================================================
VS_OUTPUT SkinningVS(VS_INPUT input, uniform bool useMaterial)
{
    VS_OUTPUT output;
    
    // スキン変換行列の取得
    float4x4 skinTransform =
				CreateTransformFromBoneTexture( input.BoneIndices, input.BoneWeights );
			
	skinTransform = mul( skinTransform, World );
  
    // 頂点変換
    float4 position = mul(input.Position, skinTransform);
    output.Position = mul(mul(position, View), Projection);

    // 法線変換
    float3 normal = normalize( mul( input.Normal, skinTransform));
    
    float3 light1 = max(dot(normal, -Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, -Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2;
    
    if(useMaterial)
    {
    	output.Lighting *= MaterialDiffuse;
    }

    output.Lighting += AmbientColor;

    output.TexCoord = input.TexCoord;

	output.PositionWS = position;

	return output;
}

//-----------------------------------------------------------------------------
// ピクセルシェーダー
//=============================================================================
// ピクセルシェーダー入力構造体
struct PS_INPUT
{
    float3 Lighting		: COLOR0;
    float2 TexCoord		: TEXCOORD0;
	float4 PositionWS	: TEXCOORD1;
};

// ピクセルシェーダー
float4 SkinningPS(PS_INPUT input,
				  uniform bool useTexture,
				  uniform bool shadowEnabled) : COLOR0
{
    float4 color = 1.0;
    
    if(useTexture)
    {
    	color = tex2D(Sampler, input.TexCoord);
	}
    
    color.rgb *= input.Lighting;
    
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
			color *= float4(0.5, 0.5, 0.5, 1);
		};
	}

    return color;
}

//-----------------------------------------------------------------------------
// 描画手法の宣言
//=============================================================================
technique BasicTechnique
{
    pass SkinnedModelPass
    {
    	ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
        
        VertexShader = compile vs_3_0 SkinningVS(false);
        PixelShader = compile ps_3_0 SkinningPS(false, true);
    }
}

technique TextureTechnique
{
    pass SkinnedModelPass
    {
    	ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
        
        VertexShader = compile vs_3_0 SkinningVS(false);
        PixelShader = compile ps_3_0 SkinningPS(true, true);
    }
}

technique MaterialTechnique
{
    pass SkinnedModelPass
    {
    	ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
        
        VertexShader = compile vs_3_0 SkinningVS(true);
        PixelShader = compile ps_3_0 SkinningPS(false, true);
    }
}

technique MaterialTextureTechnique
{
    pass SkinnedModelPass
    {
    	ZEnable = TRUE;
		ZWriteEnable = TRUE;
		ZFunc = LESSEQUAL;
		StencilEnable = FALSE;
		AlphaBlendEnable = FALSE;
		CullMode = CCW;
        
        VertexShader = compile vs_3_0 SkinningVS(true);
        PixelShader = compile ps_3_0 SkinningPS(true, true);
    }
}
