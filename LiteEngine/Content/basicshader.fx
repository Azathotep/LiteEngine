
struct VsOut
{
    float4 Position   	: SV_POSITION;
    float4 Color	: COLOR;
    float2 TextureCoords: TEXCOORD0;
};

struct PsOut
{
    float4 Color : COLOR;
};

//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;
bool xShowNormals;
float3 xCamPos;
float3 xCamUp;
float xLighting;




//------- Texture Samplers --------

Texture2D xTexture;

Texture xTexture2;

SamplerState TextureSampler
{ 
  Texture = <xTexture>; 
  Filter = MIN_MAG_MIP_LINEAR;
  AddressU = mirror; AddressV = mirror;
};

//MIN_MAG_LINEAR_MIP_POINT;
//MIN_MAG_MIP_POINT
//MIN_MAG_MIN_LINEAR

//------- Technique: Transparent --------

VsOut BasicVS( float4 inPos : SV_POSITION, float4 inColor: COLOR, float2 inTexCoords: TEXCOORD0)
{	
	VsOut ret = (VsOut)0;
	float4x4 viewProjection = mul (xView, xProjection);
	float4x4 worldViewProjection = mul (xWorld, viewProjection);
    
	ret.Position = mul(inPos, worldViewProjection);	
	ret.TextureCoords = inTexCoords;
    ret.Color = inColor;
	return ret;    
}

PsOut BasicPS(VsOut v) 
{
	PsOut ret = (PsOut)0;
	ret.Color = tex2D(TextureSampler, v.TextureCoords);
	ret.Color *= v.Color;
	return ret;
}

technique Basic
{
	pass Pass0
	{   
		VertexShader = compile vs_4_0 BasicVS();
		PixelShader  = compile ps_4_0 BasicPS();
	}
}

