texture DiffuseLightMap;
sampler DiffuseLightMapSampler = sampler_state
{
	Texture = (DiffuseLightMap);
	MipFilter = Linear;
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

float3 GetDiffuseRadiance(float4 projPosition)
{
	float3 output = 0;
	
	// ���C�g �}�b�v�ł̂��̃s�N�Z���̈ʒu��������
	float2 LightTexCoord = 0.5 * projPosition.xy / projPosition.w + float2( 0.5, 0.5 );
	LightTexCoord.y = 1.0f - LightTexCoord.y;
	
	// ���C�g �}�b�v�Ɋi�[���ꂽ���ˋP�x���擾����
	output = tex2D(DiffuseLightMapSampler, LightTexCoord).rgb;
	
	return output;
}
