Shader "Universal Render Pipeline/LookingGlass"
{
    Properties
    {
        _VirtualTexture ("Virtual Camera Texture", 2D) = "white" {}
        _SceneDepthTexture ("Scene Depth Texture", 2D) = "white" {}
        _LookingGlassDepthScalar ("Depth Scalar", Float) = 1.0
        _LookingGlassSunSelector ("Sun Selector", Float) = 0.5
        _GlassColor ("Glass Tint", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 0.8
        _RefractionStrength ("Refraction Strength", Range(0,1)) = 0.1
        _FresnelPower ("Fresnel Power", Range(0,5)) = 1.0
        _EdgeSoftness ("Edge Softness", Range(0,1)) = 0.1
        
        [Header(Distortion)]
        _DistortionStrength ("Distortion Strength", Range(0,1)) = 0.05
        _DistortionScale ("Distortion Scale", Range(0,10)) = 1.0
        _DistortionSpeed ("Distortion Speed", Range(0,5)) = 1.0
        
        [Header(Rendering)]
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 10
        [Enum(Off,0,On,1)] _ZWrite ("Z Write", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Z Test", Float) = 4
    }
    
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        Pass
        {
            Name "LookingGlassForward"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Cull [_CullMode]
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
                float2 uv : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float3 viewDirWS : TEXCOORD6;
                float eyeDepth : TEXCOORD7;
            };
            
            // Textures and Samplers
            TEXTURE2D(_VirtualTexture);
            SAMPLER(sampler_VirtualTexture);
            
            TEXTURE2D(_SceneDepthTexture);
            SAMPLER(sampler_SceneDepthTexture);
            
            // Properties
            CBUFFER_START(UnityPerMaterial)
                float4 _VirtualTexture_ST;
                float4 _GlassColor;
                float _LookingGlassDepthScalar;
                float _LookingGlassSunSelector;
                float _Transparency;
                float _RefractionStrength;
                float _FresnelPower;
                float _EdgeSoftness;
                float _DistortionStrength;
                float _DistortionScale;
                float _DistortionSpeed;
            CBUFFER_END
            
            // Global properties set by controller
            float4 _MainCameraPos;
            float4 _VirtualCameraPos;
            float4x4 _GlassWorldMatrix;
            // float4 _ProjectionParams;
            
            // Noise function for distortion
            float2 GradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float2 u = fp * fp * (3.0 - 2.0 * fp);
                
                float a = dot(sin(ip * 43758.5453), float2(1, 57));
                float b = dot(sin((ip + float2(1, 0)) * 43758.5453), float2(1, 57));
                float c = dot(sin((ip + float2(0, 1)) * 43758.5453), float2(1, 57));
                float d = dot(sin((ip + float2(1, 1)) * 43758.5453), float2(1, 57));
                
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS = normalInputs.normalWS;
                output.tangentWS = normalInputs.tangentWS;
                output.bitangentWS = normalInputs.bitangentWS;
                output.uv = TRANSFORM_TEX(input.uv, _VirtualTexture);
                output.screenPos = ComputeScreenPos(positionInputs.positionCS);
                output.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);
                output.eyeDepth = -TransformWorldToView(positionInputs.positionWS).z;
                
                return output;
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                // Screen space coordinates
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                
                // Sample scene depth
                float sceneDepth = SampleSceneDepth(screenUV);
                float linearSceneDepth = LinearEyeDepth(sceneDepth, _ZBufferParams);
                
                // Calculate depth difference for masking
                float depthDifference = input.eyeDepth - linearSceneDepth;
                float depthMask = _LookingGlassDepthScalar * depthDifference;
                
                // Discard pixels that are behind other objects
                clip(-depthMask);
                
                // Calculate view direction and normal
                float3 viewDir = normalize(input.viewDirWS);
                float3 normal = normalize(input.normalWS);
                
                // Fresnel calculation
                float fresnel = pow(1.0 - saturate(dot(viewDir, normal)), _FresnelPower);
                
                // Distortion calculation
                float2 distortionUV = input.uv * _DistortionScale + _Time.y * _DistortionSpeed;
                float2 distortion = GradientNoise(distortionUV) * _DistortionStrength;
                
                // Calculate refraction offset
                float2 refractionOffset = normal.xy * _RefractionStrength + distortion;
                
                // Sample virtual camera texture with refraction and distortion
                float2 virtualUV = input.uv + refractionOffset;
                float4 virtualColor = SAMPLE_TEXTURE2D(_VirtualTexture, sampler_VirtualTexture, virtualUV);
                
                // Edge softness based on depth difference
                float edgeSoftness = saturate(abs(depthMask) / _EdgeSoftness);
                
                // Combine colors
                float4 finalColor = virtualColor * _GlassColor;
                finalColor.a *= _Transparency * edgeSoftness;
                
                // Apply fresnel effect
                finalColor.rgb = lerp(finalColor.rgb, finalColor.rgb * fresnel, 0.3);
                
                // Sun selector effect (simulates different lighting conditions)
                float sunEffect = lerp(1.0, _LookingGlassSunSelector, 0.2);
                finalColor.rgb *= sunEffect;
                
                return finalColor;
            }
            ENDHLSL
        }
        
        // Depth pass for proper depth testing
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull [_CullMode]
            
            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            #pragma target 3.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
}
}
        
