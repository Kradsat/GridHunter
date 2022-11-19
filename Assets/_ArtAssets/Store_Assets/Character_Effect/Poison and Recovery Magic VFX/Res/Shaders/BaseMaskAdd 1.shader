Shader "Simple/BaseMaskAdd 1" {
    Properties {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _MainTexBrightness ("Main Tex Brightness", Float ) = 1
        _MainTexPannerX ("Main Tex Panner X", Float ) = 0
        _MainTexPannerY ("Main Tex Panner Y", Float ) = 0
        _MainTexContrast ("Main Tex Contrast", Float ) = 1
        _MaskTex ("Mask Tex", 2D) = "white" {}
        _MaskTexPannerX ("Mask Tex Panner X", Float ) = 0
        _MaskTexPannerY ("Mask Tex Panner Y", Float ) = 0
        _TurbulenceTex ("Turbulence Tex", 2D) = "bump" {}
        _UVEffectPower ("UV Effect Power", Float ) = 0
        _NormalTexPannerX ("Normal Tex Panner X", Float ) = 0
        _NormalTexPannerY ("Normal Tex Panner Y", Float ) = 0
        _Dis ("Dis", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
             sampler2D _MainTex;  float4 _MainTex_ST;
             float _MainTexPannerX;
             float _MainTexPannerY;
             sampler2D _MaskTex;  float4 _MaskTex_ST;
             float _MainTexBrightness;
             float _MainTexContrast;
             sampler2D _TurbulenceTex;  float4 _TurbulenceTex_ST;
             float _UVEffectPower;
             float _NormalTexPannerX;
             float _NormalTexPannerY;
             float _MaskTexPannerX;
             float _MaskTexPannerY;
             float4 _MainColor;
             float _Dis;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;

				float2 node_1145 = (o.uv0 + (float2(_NormalTexPannerX, _NormalTexPannerY)*_Time.g));
				o.uv1 = TRANSFORM_TEX(node_1145, _TurbulenceTex);
				float2 node_514 = (o.uv0 + (float2(_MaskTexPannerX, _MaskTexPannerY)*_Time.g));
				o.uv2 = TRANSFORM_TEX(node_514, _MaskTex);

                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
     
                float4 _TurbulenceTex_var = tex2D(_TurbulenceTex,i.uv1);
                clip((_TurbulenceTex_var.r+_Dis) - 0.5);

                float2 node_407 = ((_TurbulenceTex_var.r*_UVEffectPower)+(i.uv0+(float2(_MainTexPannerX,_MainTexPannerY)*_Time.g)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_407, _MainTex));
                float4 emissive = (_MainColor*(pow((_MainTexBrightness*_MainTex_var),_MainTexContrast)*i.vertexColor));
                float4 finalColor = emissive;
               
                float4 _MaskTex_var = tex2D(_MaskTex,i.uv2);
				return (finalColor*_MainColor.a*i.vertexColor*i.vertexColor.a*_MainTex_var.a*_MaskTex_var.r);
            }
            ENDCG
        }
    }
}
