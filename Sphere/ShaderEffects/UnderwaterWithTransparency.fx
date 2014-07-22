/// <description>Applies water defraction effect.</description>
// from Affirma Consulting Blog
// http://affirmaconsulting.wordpress.com/2010/12/30/tool-for-developing-hlsl-pixel-shaders-for-wpf-and-silverlight/

sampler2D inputSampler : register(s0);

float Timer : register(C0);

/// <summary>Refraction Amount.</summary>
/// <minValue>20</minValue>
/// <maxValue>60</maxValue>
/// <defaultValue>50</defaultValue>
float Refracton : register(C1);

/// <summary>Vertical trough</summary>
/// <minValue>20</minValue>
/// <maxValue>30</maxValue>
/// <defaultValue>23</defaultValue>
float VerticalTroughWidth : register(C2);

/// <summary>Center X of the Zoom.</summary>
/// <minValue>20</minValue>
/// <maxValue>30</maxValue>
/// <defaultValue>23</defaultValue>
float Wobble2 : register(C3);

static const float2 poisson[12] =
{
				float2(-0.326212f, -0.40581f),
				float2(-0.840144f, -0.07358f),
				float2(-0.695914f, 0.457137f),
				float2(-0.203345f, 0.620716f),
				float2(0.96234f, -0.194983f),
				float2(0.473434f, -0.480026f),
				float2(0.519456f, 0.767022f),
				float2(0.185461f, -0.893124f),
				float2(0.507431f, 0.064425f),
				float2(0.89642f, 0.412458f),
				float2(-0.32194f, -0.932615f),
				float2(-0.791559f, -0.59771f)
};

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 Delta = { sin(Timer + uv.x * VerticalTroughWidth + uv.y * uv.y * Wobble2 ) * 0.02 , cos(Timer + uv.y * 32 + uv.x * uv.x * 13)*0.02 };

		float2 NewUV = uv + Delta;

	float4 Color = 0;
	for (int i = 0; i < 12; i++)
	{
		 float2 Coord = NewUV + (poisson[i] / Refracton);
			 Color += tex2D(inputSampler, Coord)/12.0;
		 }
		 Color += tex2D(inputSampler, uv)/4;
		 Color.a = tex2D( inputSampler, uv ).a;
		 
		 return Color;
}