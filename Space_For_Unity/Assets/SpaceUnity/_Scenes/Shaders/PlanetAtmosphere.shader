// SpaceUnity: Shader from Unity wiki

Shader "SpaceUnity/PlanetAtmosphere"
{
    Properties
    {
        //_Color("Color", Color) = (0, 0, 0, 1)
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
        _Size("Size", Float) = 0.1
        _Falloff("Falloff", Float) = 5
        _Transparency("Transparency", Float) = 15       
    }
   
	SubShader
    {  
		Tags {
            	"LightMode" = "Always"            	
	    		"Queue" = "Transparent+1"
        		"RenderType" = "Transparent"
		}
 		Pass {
            Name "AtmosphereBase"

            Cull Front
            Blend SrcAlpha One
           
            CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
				#pragma exclude_renderers gles
                #pragma vertex vert
                #pragma fragment frag
               
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
               
                //uniform float4 _Color;
                uniform float4 _AtmoColor;
                uniform float _Size;
                uniform float _Falloff;
                uniform float _Transparency;
               
                struct v2f {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                };

                v2f vert(appdata_base v) {
                    v2f o;
                   
                    v.vertex.xyz += v.normal*_Size;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.normal = v.normal;
                    o.worldvertpos = mul(_Object2World, v.vertex);
                   
                    return o;
                }
              
                float4 frag(v2f i) : COLOR {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(i.worldvertpos-_WorldSpaceCameraPos);                   
                    float4 color = _AtmoColor;
                    color.a = pow(saturate(dot(viewdir, i.normal)), _Falloff);
                    //color.a *= _Transparency*_Color*dot(normalize(i.worldvertpos+_WorldSpaceLightPos0), i.normal);
                    color.a *= _Transparency*dot(normalize(i.worldvertpos+_WorldSpaceLightPos0), i.normal);
                    return color;
                }
            ENDCG
        }
    }
   
    FallBack "Diffuse"
}