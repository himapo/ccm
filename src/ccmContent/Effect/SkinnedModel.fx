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

//-----------------------------------------------------------------------------
// 定数レジスタ宣言
//=============================================================================
float4x4 World;			// オブジェクトのワールド座標
float4x4 View;
float4x4 Projection;

float3 Light1Direction = normalize(float3(1, 1, -2));
float3 Light1Color = float3(0.9, 0.8, 0.7);

float3 Light2Direction = normalize(float3(-1, -1, 1));
float3 Light2Color = float3(0.1, 0.3, 0.8);

float3 AmbientColor = 0.2;

float3 MaterialDiffuse = 1.0;
float3 MaterialEmissive = 0.0;
float3 MaterialSpecular = 1.0;
float MaterialSpecularPower = 1.0;

string TechniqueName;

texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
};

//-----------------------------------------------------------------------------
// 頂点テクスチャ用の定数レジスタ宣言
//=============================================================================
float2 BoneTextureSize;	// ボーン用頂点テクスチャのサイズ

// ボーン用頂点テクスチャサンプラー宣言
texture BoneRotationTexture;

sampler BoneRotationSampler : register(vs,s0) = sampler_state
{
	Texture = (BoneRotationTexture);
	// 殆どのGPUでは以下のようなステート設定にしないと
	// 頂点テクスチャのフェッチがうまくいかない
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
	AddressU = Clamp;
	AddressV = Clamp;
};

texture BoneTranslationTexture;

sampler BoneTranslationSampler : register(vs,s1) = sampler_state
{
	Texture = (BoneTranslationTexture);
	// 殆どのGPUでは以下のようなステート設定にしないと
	// 頂点テクスチャのフェッチがうまくいかない
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
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
    float4 Position : POSITION0;
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

//-----------------------------------------------------------------------------
// クォータニオンヘルパーメソッド
//=============================================================================
// クォータニオンと平行移動から行列に変換する
float4x4 CreateTransformFromQuaternionTransform( float4 quaternion, float3 translation )
{
	float4 q = quaternion;
	float ww = q.w * q.w - 0.5f;
	float3 v00 = float3( ww       , q.x * q.y, q.x * q.z );
	float3 v01 = float3( q.x * q.x, q.w * q.z,-q.w * q.y );
	float3 v10 = float3( q.x * q.y, ww,        q.y * q.z );
	float3 v11 = float3(-q.w * q.z, q.y * q.y, q.w * q.x );
	float3 v20 = float3( q.x * q.z, q.y * q.z, ww        );
	float3 v21 = float3( q.w * q.y,-q.w * q.x, q.z * q.z );
	
	return float4x4(
		2.0f * ( v00 + v01 ), 0,
		2.0f * ( v10 + v11 ), 0, 
		2.0f * ( v20 + v21 ), 0,
		translation, 1
	);
}

// 複数のクォータニオン+平行移動のブレンディング処理
float4x4 BlendQuaternionTransforms(
		float4 q1, float3 t1,
		float4 q2, float3 t2,
		float4 q3, float3 t3,
		float4 q4, float3 t4,
		float4 weights )
{
	return
		CreateTransformFromQuaternionTransform(q1, t1) * weights.x +
		CreateTransformFromQuaternionTransform(q2, t2) * weights.y +
		CreateTransformFromQuaternionTransform(q3, t3) * weights.z +
		CreateTransformFromQuaternionTransform(q4, t4) * weights.w;
}

//-----------------------------------------------------------------------------
// 頂点テクスチャからボーン情報のフェッチ
//=============================================================================
float4x4 CreateTransformFromBoneTexture( float4 boneIndices, float4 boneWeights )
{
	float2 uv = 1.0f / BoneTextureSize;
	uv.y *= 0.5f;
	float4 texCoord0 = float4( ( 0.5f + boneIndices.x ) * uv.x, uv.y, 0, 1 );
	float4 texCoord1 = float4( ( 0.5f + boneIndices.y ) * uv.x, uv.y, 0, 1 );
	float4 texCoord2 = float4( ( 0.5f + boneIndices.z ) * uv.x, uv.y, 0, 1 );
	float4 texCoord3 = float4( ( 0.5f + boneIndices.w ) * uv.x, uv.y, 0, 1 );

	// 回転部分のフェッチ
	float4 q1 = tex2Dlod( BoneRotationSampler, texCoord0 );
	float4 q2 = tex2Dlod( BoneRotationSampler, texCoord1 );
	float4 q3 = tex2Dlod( BoneRotationSampler, texCoord2 );
	float4 q4 = tex2Dlod( BoneRotationSampler, texCoord3 );

	// 平行移動部分のフェッチ
	float4 t1 = tex2Dlod( BoneTranslationSampler, texCoord0 );
	float4 t2 = tex2Dlod( BoneTranslationSampler, texCoord1 );
	float4 t3 = tex2Dlod( BoneTranslationSampler, texCoord2 );
	float4 t4 = tex2Dlod( BoneTranslationSampler, texCoord3 );
	
	return BlendQuaternionTransforms(
					q1, t1,
					q2, t2,
					q3, t3,
					q4, t4,
					boneWeights );
}

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
    
    float3 light1 = max(dot(normal, Light1Direction), 0) * Light1Color;
    float3 light2 = max(dot(normal, Light2Direction), 0) * Light2Color;

    output.Lighting = light1 + light2;
    
    if(useMaterial)
    {
    	output.Lighting *= MaterialDiffuse;
    }

    output.Lighting += AmbientColor;

    output.TexCoord = input.TexCoord;
    
    return output;
}

//-----------------------------------------------------------------------------
// ピクセルシェーダー
//=============================================================================
// ピクセルシェーダー入力構造体
struct PS_INPUT
{
    float3 Lighting : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

// ピクセルシェーダー
float4 SkinningPS(PS_INPUT input, uniform bool useTexture) : COLOR0
{
    float4 color = 1.0;
    
    if(useTexture)
    {
    	color = tex2D(Sampler, input.TexCoord);
	}
	
    color.rgb *= input.Lighting;
    
    return color;
}

//-----------------------------------------------------------------------------
// 描画手法の宣言
//=============================================================================
technique BasicTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 SkinningVS(false);
        PixelShader = compile ps_3_0 SkinningPS(false);
    }
}

technique TextureTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 SkinningVS(false);
        PixelShader = compile ps_3_0 SkinningPS(true);
    }
}

technique MaterialTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 SkinningVS(true);
        PixelShader = compile ps_3_0 SkinningPS(false);
    }
}

technique MaterialTextureTechnique
{
    pass SkinnedModelPass
    {
        VertexShader = compile vs_3_0 SkinningVS(true);
        PixelShader = compile ps_3_0 SkinningPS(true);
    }
}
