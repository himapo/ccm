float4 ToLDR(float3 hdr)
{
	float L = max(1.0f, max(hdr.r, max(hdr.g, hdr.b)));

	return float4(hdr / L, 1.0f / L);
}

float3 ToHDR(float4 ldr)
{
	return float3(ldr.rgb) / ldr.a;
}
