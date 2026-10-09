Shader "Kavkaz Epos/Climbing/Surface Highlight"
{
    Properties
    {
        [HDR] _HighlightColor ("Highlight Color", Color) = (0.3, 0.75, 1, 1)
        _Intensity ("Intensity", Range(0, 5)) = 0.65
        _BaseGlow ("Base Glow", Range(0, 1)) = 0.2
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 0.8
        _BandSpacing ("Band Spacing (Metres)", Range(0.05, 2)) = 0.35
        _BandSpeed ("Band Speed (Metres / Second)", Range(-2, 2)) = 0.15
        _BandWidth ("Band Width", Range(0.01, 0.5)) = 0.12
        _EdgeFeather ("Volume Edge Feather", Range(0.001, 0.5)) = 0.15
        _FadeStart ("Distance Fade Start", Float) = 15
        _FadeEnd ("Distance Fade End", Float) = 25
        [HideInInspector] _VolumeCenter ("Volume Center", Vector) = (0, 0, 0, 0)
        [HideInInspector] _VolumeSize ("Volume Size", Vector) = (1, 1, 1, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent-10"
            "DisableBatching" = "True"
        }

        Pass
        {
            Name "ClimbingSurfaceHighlight"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend SrcAlpha One
            ColorMask RGB
            ZWrite Off
            ZTest Always
            Cull Front

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _HighlightColor;
                float4 _VolumeCenter;
                float4 _VolumeSize;
                float _Intensity;
                float _BaseGlow;
                float _PulseSpeed;
                float _BandSpacing;
                float _BandSpeed;
                float _BandWidth;
                float _EdgeFeather;
                float _FadeStart;
                float _FadeEnd;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                // The unit cube is only the rasterization volume, not a visible surface.
                float3 positionOS = _VolumeCenter.xyz + input.positionOS.xyz * _VolumeSize.xyz;
                output.positionCS = TransformObjectToHClip(positionOS);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.positionCS.xy / _ScaledScreenParams.xy;
                float rawDepth = SampleSceneDepth(uv);

                // Sky / empty depth must never receive the projection.
                #if UNITY_REVERSED_Z
                    clip(rawDepth - 0.000001);
                    float depth = rawDepth;
                #else
                    clip(0.999999 - rawDepth);
                    float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1.0, rawDepth);
                #endif

                float3 positionWS = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
                float3 positionOS = TransformWorldToObject(positionWS);
                float3 volumePosition = (positionOS - _VolumeCenter.xyz) / max(_VolumeSize.xyz, 0.0001);
                float3 edgeDistance = 0.5 - abs(volumePosition);
                clip(min(edgeDistance.x, min(edgeDistance.y, edgeDistance.z)));

                float3 edgeFade = smoothstep(0.0, max(_EdgeFeather, 0.001), edgeDistance);
                float volumeFade = edgeFade.x * edgeFade.y * edgeFade.z;
                float distanceFade = 1.0 - smoothstep(
                    max(_FadeStart, 0.0), max(_FadeEnd, _FadeStart + 0.01),
                    distance(positionWS, GetCameraPositionWS()));

                // World-space height keeps band spacing consistent on scaled triggers.
                float phase = (positionWS.y - _Time.y * _BandSpeed) / max(_BandSpacing, 0.001);
                float bandDistance = abs(frac(phase + 0.5) - 0.5);
                float aa = max(fwidth(phase), 0.001);
                float band = 1.0 - smoothstep(_BandWidth, _BandWidth + aa, bandDistance);
                float pulse = 0.75 + 0.25 * sin(_Time.y * _PulseSpeed * TWO_PI);
                float glow = lerp(_BaseGlow, 1.0, band) * pulse;
                float alpha = saturate(_HighlightColor.a) * volumeFade * distanceFade;
                return half4(_HighlightColor.rgb * (_Intensity * glow), alpha);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
