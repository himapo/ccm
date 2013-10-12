/*

% Description of my shader.
% Second line of description for my shader.

keywords: material classic

date: YYMMDD

*/

#include "BoneTexture.fxh"

float4x4 World		: World;
float4x4 View		: View;
float4x4 Projection	: Projection;


struct VSInput
{
    float4 Position : POSITION0;
    float3 Normal	: NORMAL0;
    float2 TexCoord : TEXCOORD0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

struct VSOutput {
	float4 Position		: POSITION;
	float4 PositionPS	: TEXCOORD0;
};

struct PSOutput {
	float4 RGBColor : COLOR0;
};

VSOutput VSStatic(float4 vPos : POSITION) {
	VSOutput output;
	
	float4x4 mat;
	mat = mul(World, View);
	mat = mul(mat, Projection);
	
	output.Position = mul(vPos, mat);
	
	output.PositionPS = output.Position;
	
	return output;
}

//-----------------------------------------------------------------------------
// 頂点シェーダー
//=============================================================================
VSOutput VSSkinning(VSInput input)
{
	VSOutput output = (VSOutput)0;
	
	// スキン変換行列の取得
	float4x4 skinTransform =
				CreateTransformFromBoneTexture( input.BoneIndices, input.BoneWeights );
	
	skinTransform = mul( skinTransform, World );
	
	// 頂点変換
	float4 position = mul(input.Position, skinTransform);
	output.Position = mul(mul(position, View), Projection);
	
	output.PositionPS = output.Position;
	
	return output;
}

PSOutput PSCommon(VSOutput input) {
	PSOutput output = (PSOutput)0;
	
	output.RGBColor.r = input.PositionPS.z / input.PositionPS.w;
	
	return output;
}

technique RenderDepthStatic {
	pass p0 {
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 VSStatic();
		PixelShader = compile ps_2_0 PSCommon();
	}
}

technique RenderDepthSkinning {
	pass p0 {
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_3_0 VSSkinning();
		PixelShader = compile ps_3_0 PSCommon();
	}
}

