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
#include "LightMapping.fxh"
#include "Shadow.fxh"

//-----------------------------------------------------------------------------
// 定数レジスタ宣言
//=============================================================================
float4x4 World;			// オブジェクトのワールド座標
float4x4 View;
float4x4 Projection;

float3 AmbientColor = 0.2;

float3 DiffuseColor = 1.0;
float3 EmissiveColor = 0.0;
float3 SpecularColor = 1.0;
float SpecularPower = 1.0;

string TechniqueName;	// モデルプロセッサから指定するためにこれは必要

texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};

//-----------------------------------------------------------------------------
// 構造体宣言
//=============================================================================
// 頂点シェーダー入力構造体
struct VSInputSkinning
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

// 頂点シェーダー出力構造体
struct VSOutput
{
    float4 Position		: POSITION0;
    float2 TexCoord		: TEXCOORD0;
	float4 PositionWS	: TEXCOORD1;
};

//-----------------------------------------------------------------------------
// 頂点シェーダー
//=============================================================================
VSOutput SkinningVS(VSInputSkinning input, uniform bool useMaterial)
{
    VSOutput output;
    
    // スキン変換行列の取得
    float4x4 skinTransform =
				CreateTransformFromBoneTexture( input.BoneIndices, input.BoneWeights );
			
	skinTransform = mul( skinTransform, World );
  
    // 頂点変換
    float4 position = mul(input.Position, skinTransform);
    output.Position = mul(mul(position, View), Projection);

    output.TexCoord = input.TexCoord;

	output.PositionWS = position;

	return output;
}

//-----------------------------------------------------------------------------
// ピクセルシェーダー
//=============================================================================
float4 SkinningPS(VSOutput input,
				  uniform bool useTexture,
				  uniform bool shadowEnabled) : COLOR0
{
	float4 output = 1.0;
	
	// 射影空間でのこのピクセルの位置を見つける
	float4 projPosition = mul(mul(input.PositionWS, View), Projection);
	
	// ライト マップに格納された放射輝度を取得する
	float3 diffuse;
	float3 specular;
	GetRadiance(diffuse, specular, projPosition);
	
	// マテリアルカラー、アンビエントライトと合成する
	float3 light = saturate(DiffuseColor * diffuse + SpecularColor * specular + AmbientColor);
	
	output = float4(light.rgb, 1.0);
	
    if(useTexture)
    {
    	output *= tex2D(Sampler, input.TexCoord);
	}
    
	if(shadowEnabled)
	{
	    output *= CalcShadow(input.PositionWS);
	}

    return output;
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
