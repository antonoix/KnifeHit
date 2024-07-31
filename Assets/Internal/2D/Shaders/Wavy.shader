Shader "Custom/Wavy"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor ("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor ("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _BlendOp ("Operation", Float) = 0
        
        _Color("Color", color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
        _RevealValue("Reveal", float) = 0
        _FeatherValue("Feather", float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Blend [_SrcFactor] [_DstFactor]
        BlendOp [_BlendOp]
        
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv: TEXCOORD0;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _RevealValue;
            float _FeatherValue;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
            
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex);

                return o;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, input.uv.xy);
                fixed4 mask = tex2D(_MaskTex, input.uv.zw);
                float revealAmount = step(mask.r, _RevealValue);
                float topRevealAmount = step(mask.r, _RevealValue + _FeatherValue);
                float bottomRevealAmount = step(mask.r, _RevealValue - _FeatherValue);
                float topAndBottomDifference = topRevealAmount - bottomRevealAmount;
                float3 finishColor = lerp(textureColor.rgb, float3(0, 0, 0), topAndBottomDifference);

                return fixed4(finishColor * _Color, textureColor.a * revealAmount);
            }
            ENDCG
        }
    }
}