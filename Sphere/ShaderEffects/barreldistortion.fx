// Based on https://code.google.com/p/groovyarcade/source/browse/hlsl/pincushion.fx?repo=groovymame&r=3d99cbc3895ed05e98114f19b9f30cb3dc259ddc&spec=svn.groovymame.d7d6786fd4a866d79ee78cc87f0dc85ca67c9f90

// new HLSL shader
// modify the comment parameters to reflect your shader parameters

/// <summary>Explain the purpose of this variable.</summary>
/// <minValue>05/minValue>
/// <maxValue>10</maxValue>
/// <defaultValue>3.5</defaultValue>

uniform float PI = 3.14159265f;

/// <summary>Controls the barrel amount</summary>
/// <minValue>0/minValue>
/// <maxValue>1.0</maxValue>
/// <defaultValue>0.1</defaultValue>
float BarrelAmountX : register(C0);

/// <summary>Appears to do nothing but is still required for the shader to work, for some reason.</summary>
/// <minValue>0/minValue>
/// <maxValue>1.0</maxValue>
/// <defaultValue>0.1</defaultValue>
float BarrelAmountY : register(C1);

uniform float WidthRatio;
uniform float HeightRatio;

sampler DiffuseSampler = sampler_state
{
	Texture   = <Diffuse>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
	AddressW = CLAMP;
};

float4 main(float2 Input : TEXCOORD) : COLOR 
{
	float2 Ratios = float2(WidthRatio, HeightRatio);

	// -- Screen Barrel Calculation --
	float2 UnitCoord = Input * Ratios * 2.0f - 1.0f;

	float BarrelR2 = pow(length(UnitCoord),2.0f) / pow(length(Ratios), 2.0f);
	float2 BarrelCurve = UnitCoord * BarrelAmountX * BarrelR2;
	float2 BaseCoord = Input + BarrelCurve;

	// If BaseCoord refers to a coordinate outside the original texture, return a transparent pixel instead to avoid weird ghosting artifacts outside the image.
  if( BaseCoord.x < 0.0 ||
			BaseCoord.x > 1.0 ||
			BaseCoord.y < 0.0 ||
			BaseCoord.y > 1.0)
  {
  	return float4(0,0,0,0);
  } 
	else
 	{
 		return tex2D(DiffuseSampler, BaseCoord);
  }	
}