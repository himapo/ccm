//-----------------------------------------------------------------------------
// 定数レジスタ宣言
//=============================================================================
float4x4 LightViewProjection;
float DepthBias = 0.005f;

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

float4 CalcShadow(float4 positionWS)
{
	float4 output = 1.0;
	
	// ライト空間でのこのピクセルの位置を見つける
	float4 lightingPosition = mul(positionWS, LightViewProjection);
	
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
		output = float4(0.5, 0.5, 0.5, 1);
	}
	
	return output;
}
