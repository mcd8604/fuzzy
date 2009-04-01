float4x4 World;
float4x4 View;
float4x4 Projection;

float3 lightPos;
float4 lightColor;
float4 EyePosition;

float4 AmbientLightColor;

float DiffusePower;
float SpecularPower;
float exponent;

Texture BasicTexture;

sampler TextureSampler = sampler_state 
{ 
	texture = <BasicTexture>; 
	magfilter = LINEAR; 
	minfilter = LINEAR; 
	mipfilter = LINEAR; 
	AddressU = WRAP; 
	AddressV = WRAP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal	: NORMAL0;
    float4 Color    : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION;
    float2 Texcoord : TEXCOORD0;
    float3 Normal	: TEXCOORD1;
    float3 Light	: TEXCOORD2;
    float3 Reflected: TEXCOORD3;
    float3 View		: TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    	
    float4x4 wvp = mul(mul(World,View),Projection);
    output.Position = mul(input.Position, wvp);
    
    output.Texcoord = input.Texcoord;
    
    float3 WPosition = mul(input.Position, World).xyz;
    
	output.Normal = mul(input.Normal, World);
    
    output.Light = normalize(lightPos - WPosition);
    output.Reflected = normalize(reflect(output.Light, output.Normal));
    output.View = normalize(EyePosition - WPosition);
        
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{    

    float4 textureColor = tex2D(TextureSampler, input.Texcoord);
    float4 ambient = AmbientLightColor * textureColor;
    float4 surfaceColor = lightColor * textureColor;
    float4 diffuse = DiffusePower * surfaceColor * dot(input.Light, input.Normal); 
    
    float4 specular;
    float dot = dot(input.View, input.Reflected);
    
    if(dot < 0) {
		specular = 0;
	} else {   
		specular = SpecularPower * lightColor * pow(dot, exponent);
	}
	
    return ambient + diffuse + specular;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
