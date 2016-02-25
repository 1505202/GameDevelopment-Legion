// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33515,y:32838,varname:node_3138,prsc:2|normal-4102-RGB,emission-1954-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32377,y:32394,ptovrint:False,ptlb:Block Color,ptin:_BlockColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:99,x:32377,y:32586,ptovrint:False,ptlb:BlockMask,ptin:_BlockMask,varname:node_99,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9919,x:32608,y:32495,varname:node_9919,prsc:2|A-7241-RGB,B-99-RGB;n:type:ShaderForge.SFN_Color,id:597,x:32393,y:33082,ptovrint:False,ptlb:EmissiveColor,ptin:_EmissiveColor,varname:node_597,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:566,x:32393,y:33250,ptovrint:False,ptlb:EmissiveMask,ptin:_EmissiveMask,varname:node_566,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:850,x:32611,y:33082,varname:node_850,prsc:2|A-597-RGB,B-566-RGB;n:type:ShaderForge.SFN_Add,id:1954,x:33186,y:32913,varname:node_1954,prsc:2|A-8068-OUT,B-850-OUT;n:type:ShaderForge.SFN_Add,id:8068,x:32988,y:32792,varname:node_8068,prsc:2|A-9919-OUT,B-7925-OUT;n:type:ShaderForge.SFN_Tex2d,id:9911,x:32377,y:32802,ptovrint:False,ptlb:Shade,ptin:_Shade,varname:node_9911,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7925,x:32792,y:32842,varname:node_7925,prsc:2|A-9911-RGB,B-6303-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:6303,x:32576,y:32915,varname:node_6303,prsc:2,min:-1,max:1|IN-4676-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4676,x:32377,y:32985,ptovrint:False,ptlb:ShadeIntensity,ptin:_ShadeIntensity,varname:node_4676,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:4102,x:33186,y:32704,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_4102,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;proporder:7241-99-597-566-9911-4676-4102;pass:END;sub:END;*/

Shader "Legion/UnlitEmisive" {
    Properties {
        _BlockColor ("Block Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _BlockMask ("BlockMask", 2D) = "white" {}
        _EmissiveColor ("EmissiveColor", Color) = (0.5,0.5,0.5,1)
        _EmissiveMask ("EmissiveMask", 2D) = "white" {}
        _Shade ("Shade", 2D) = "white" {}
        _ShadeIntensity ("ShadeIntensity", Float ) = 0
        _NormalMap ("NormalMap", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _BlockColor;
            uniform sampler2D _BlockMask; uniform float4 _BlockMask_ST;
            uniform float4 _EmissiveColor;
            uniform sampler2D _EmissiveMask; uniform float4 _EmissiveMask_ST;
            uniform sampler2D _Shade; uniform float4 _Shade_ST;
            uniform float _ShadeIntensity;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
////// Lighting:
////// Emissive:
                float4 _BlockMask_var = tex2D(_BlockMask,TRANSFORM_TEX(i.uv0, _BlockMask));
                float4 _Shade_var = tex2D(_Shade,TRANSFORM_TEX(i.uv0, _Shade));
                float4 _EmissiveMask_var = tex2D(_EmissiveMask,TRANSFORM_TEX(i.uv0, _EmissiveMask));
                float3 emissive = (((_BlockColor.rgb*_BlockMask_var.rgb)+(_Shade_var.rgb*clamp(_ShadeIntensity,-1,1)))+(_EmissiveColor.rgb*_EmissiveMask_var.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
