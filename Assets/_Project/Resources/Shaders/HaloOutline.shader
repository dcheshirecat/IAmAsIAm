// The glow around things you can use (owner: "like a halo/outline, not a light from the middle").
// An inverted hull: the mesh drawn again a little bigger, back faces only, so only a rim shows around the thing's outline.
// It's pushed out from the thing's centre (not along its normals) so cube corners don't split apart. Additive, no depth write.
Shader "IAmAsIAm/HaloOutline"
{
    Properties
    {
        _Color ("Colour", Color) = (1, 0.9, 0.68, 0.8)
        _Width ("Width (metres)", Float) = 0.04
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "Halo"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite Off
            Blend SrcAlpha One

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Width;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; };

            Varyings vert (Attributes v)
            {
                Varyings o;
                float3 posWS = TransformObjectToWorld(v.positionOS.xyz);
                float3 centreWS = TransformObjectToWorld(float3(0, 0, 0));
                float3 away = posWS - centreWS;
                float len = length(away);
                float3 dir = len > 1e-4 ? away / len : TransformObjectToWorldNormal(v.normalOS);
                posWS += dir * _Width;
                o.positionCS = TransformWorldToHClip(posWS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}
