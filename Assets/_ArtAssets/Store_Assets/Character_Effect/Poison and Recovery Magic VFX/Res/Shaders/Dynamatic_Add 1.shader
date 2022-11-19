Shader "Simple/Dynamatic_Add 1" {
    Properties {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _MainTexBrightness ("Main Tex Brightness", Float ) = 1
        _MainTexPower ("Main Tex Power", Float ) = 1
        _TurbulenceTex ("Turbulence Tex", 2D) = "bump" {}
        _TurbulenceTexPannerX ("Turbulence Tex Panner X", Float ) = 0
        _TurbulenceTexPannerY ("Turbulence Tex Panner Y", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		[Enum(On,1,Off,0)] _ZWrite("ZWrite", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+100"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
 
            Blend One One
            Cull Off
			ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
			#pragma skip_variants DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON LIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SCREEN SHADOWS_SHADOWMASK VERTEXLIGHT_ON LIGHTPROBE_SH DIRECTIONAL
              sampler2D _MainTex;   float4 _MainTex_ST;
              float _MainTexBrightness;
              sampler2D _TurbulenceTex;   float4 _TurbulenceTex_ST;
              float _TurbulenceTexPannerX;
              float _TurbulenceTexPannerY;
              float4 _MainColor;
              float _MainTexPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;

				float2 node_1145 = (o.uv0 + (float2(_TurbulenceTexPannerX, _TurbulenceTexPannerY)*_Time.g));
				o.uv2 = TRANSFORM_TEX(node_1145, _TurbulenceTex);

                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {

               
                float4 _TurbulenceTex_var = tex2D(_TurbulenceTex,i.uv2);
                clip((_TurbulenceTex_var.r+(1.0-i.uv1.b)) - 0.5);

                float2 node_407 = ((_TurbulenceTex_var.r*i.uv1.a)+(i.uv0+float2(i.uv1.r,i.uv1.g)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_407, _MainTex));
                float4 emissive = (_MainColor*(pow((_MainTexBrightness*_MainTex_var),_MainTexPower)*i.vertexColor));
                float4 finalColor = emissive;

				return (finalColor*_MainColor.a*i.vertexColor.a*_MainTex_var.a);
            }
            ENDCG
        }
    }
}
