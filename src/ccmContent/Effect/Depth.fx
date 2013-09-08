/*

% Description of my shader.
% Second line of description for my shader.

keywords: material classic

date: YYMMDD

*/

float4x4 World	: World;
float4x4 View	: View;
float4x4 Proj	: Projection;


struct VS_OUTPUT {
	float4 Position	: POSITION;
	float4 DepthTextureUV: TEXCOORD0;
};

VS_OUTPUT mainVS(float4 vPos : POSITION) {
	VS_OUTPUT Output;
	
	float4x4 mat;
	mat = mul(World, View);
	mat = mul(mat, Proj);
	
	Output.Position = mul(vPos, mat);
	
	Output.DepthTextureUV = Output.Position;
	
	return Output;
}

struct PS_OUTPUT {
	float4 RGBColor : COLOR0;
};

PS_OUTPUT mainPS(float4 DepthTextureUV : TEXCOORD0) {
	PS_OUTPUT Output;

	Output.RGBColor = DepthTextureUV.z / DepthTextureUV.w;

	return Output;
}

technique RenderDepth {
	pass p0 {
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 mainPS();
	}
}

