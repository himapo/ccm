/*

% Description of my shader.
% Second line of description for my shader.

keywords: material classic

date: YYMMDD

*/

float4x4 World		: World;
float4x4 View		: View;
float4x4 Projection	: Projection;


struct VS_OUTPUT {
	float4 Position	: POSITION;
	float Depth		: TEXCOORD0;
};

VS_OUTPUT mainVS(float4 vPos : POSITION) {
	VS_OUTPUT Output;
	
	float4x4 mat;
	mat = mul(World, View);
	mat = mul(mat, Projection);
	
	Output.Position = mul(vPos, mat);
	
	Output.Depth = Output.Position.z / Output.Position.w;
	
	return Output;
}

struct PS_OUTPUT {
	float4 RGBColor : COLOR0;
};

PS_OUTPUT mainPS(VS_OUTPUT input) {
	PS_OUTPUT Output = (PS_OUTPUT)0;

	Output.RGBColor.r = input.Depth;

	return Output;
}

technique RenderDepth {
	pass p0 {
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 mainPS();
	}
}

