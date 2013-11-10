texture DiffuseLightMap;
sampler DiffuseLightMapSampler = sampler_state
{
	Texture = (DiffuseLightMap);
};

texture SpecularLightMap;
sampler SpecularLightMapSampler = sampler_state
{
	Texture = (SpecularLightMap);
};

float3 ToHDR(float4 ldr)
{
	return float3(ldr.rgb) / ldr.a;
}

void GetRadiance(out float3 diffuse, out float3 specular, float4 projPosition)
{
	diffuse = 0;
	specular = 0;
	
	// ライト マップでのこのピクセルの位置を見つける
	float2 LightTexCoord = 0.5 * projPosition.xy / projPosition.w + float2( 0.5, 0.5 );
	LightTexCoord.y = 1.0f - LightTexCoord.y;
	
	// ライト マップに格納された放射輝度を取得する
	diffuse = ToHDR(tex2D(DiffuseLightMapSampler, LightTexCoord));
	specular = ToHDR(tex2D(SpecularLightMapSampler, LightTexCoord));
}
