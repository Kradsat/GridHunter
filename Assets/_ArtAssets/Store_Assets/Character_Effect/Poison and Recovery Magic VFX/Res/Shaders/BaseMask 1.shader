Shader "Simple/BaseMask 1" {
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

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
			ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
			#pragma skip_variants DIRLIGHTMAP_COMBINED DYNAMICLIGHTMAP_ON LIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SCREEN SHADOWS_SHADOWMASK VERTEXLIGHT_ON LIGHTPROBE_SH DIRECTIONAL

            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _MainTexPannerX;
            uniform float _MainTexPannerY;
            uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
            uniform float _MainTexBrightness;
            uniform float _MainTexContrast;
            uniform sampler2D _TurbulenceTex; uniform float4 _TurbulenceTex_ST;
            uniform float _UVEffectPower;
            uniform float _NormalTexPannerX;
            uniform float _NormalTexPannerY;
            uniform float _MaskTexPannerX;
            uniform float _MaskTexPannerY;
            uniform float4 _MainColor;
            uniform float _Dis;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 node_7882 = _Time;
                float2 node_1145 = (i.uv0+(float2(_NormalTexPannerX,_NormalTexPannerY)*node_7882.g));
                float4 _TurbulenceTex_var = tex2D(_TurbulenceTex,TRANSFORM_TEX(node_1145, _TurbulenceTex));
                clip((_TurbulenceTex_var.r+_Dis) - 0.5);
                float4 node_2160 = _Time;
                float2 node_407 = ((_TurbulenceTex_var.r*_UVEffectPower)+(i.uv0+(float2(_MainTexPannerX,_MainTexPannerY)*node_2160.g)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_407, _MainTex));
                float3 emissive = (_MainColor.rgb*(pow((_MainTexBrightness*_MainTex_var.rgb),_MainTexContrast)*i.vertexColor.rgb));
                float3 finalColor = emissive;
                float4 node_6409 = _Time;
                float2 node_514 = (i.uv0+(float2(_MaskTexPannerX,_MaskTexPannerY)*node_6409.g));
                float4 _MaskTex_var = tex2D(_MaskTex,TRANSFORM_TEX(node_514, _MaskTex));
                return fixed4(finalColor,(_MainColor.a*(i.vertexColor.a*(_MainTex_var.a*_MaskTex_var.r))));
            }
            ENDCG
        }
    }
}
