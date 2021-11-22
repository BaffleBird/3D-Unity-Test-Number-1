Shader "Grass/BladeGrass"
{
    Properties
    {
        _BaseColor("Base color", Color) = (0, 0.5, 0, 1) // Color of the lower layer
        _TipColor("Tip color", Color) = (0, 1, 0, 1) // Color of the highest layer
        _RandomJitterRadius("Random jitter radius", Float) = 0.1
        _WindTexture("Wind texture", 2D) = "white" {}
        _WindFrequency("Wind Frequency", Float) = 1
        _WindAmplitude("Wind strength", Float) = 1
    }
    SubShader
    {
        // URP Compatibility
        Tags { "RenderType"="Opaque" "RenderPipline" = "UniversalPipeline" "IgnoreProjector" = "True"}

        // Forward Lit Pass
        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode" = "UniversalForward"}
            Cull Off

            HLSLPROGRAM
            // Signal this shader requires a compute buffer
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            
            // Lighting and Shadows Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment

            // Include logic file
            #include "BladeGrass.hlsl"

            ENDHLSL
        }
    }
}
