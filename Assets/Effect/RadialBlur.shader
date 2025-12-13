Shader "Hidden/Custom/RadialBlur"
{
    Properties { }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalRenderPipeline" 
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "RadialBlur"
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ===== Input Texture (From RenderPass) =====
            TEXTURE2D(_SourceTex);
            SAMPLER(sampler_SourceTex);

            // ===== Parameters =====
            float _Intensity;
            float2 _Center;
            float _Radius;
            int _SampleCount;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_Position;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 Frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dir = uv - _Center;

                // distance from center
                float dist = length(dir);

                // intensity fade outward
                float blurAmount = saturate(dist / _Radius) * _Intensity;

                // tiny samples = better performance
                float2 stepUV = dir * blurAmount / max(_SampleCount, 1);

                float4 col = 0;
                float2 curUV = uv;

                // Multi-sample radial blur
                [loop]
                for (int s = 0; s < _SampleCount; s++)
                {
                    col += SAMPLE_TEXTURE2D(_SourceTex, sampler_SourceTex, curUV);
                    curUV -= stepUV;
                }

                col /= _SampleCount;

                return col;
            }

            ENDHLSL
        }
    }
}
