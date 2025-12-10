Shader "Trashy Outlines/Outline Mask"
{
    Properties
    {
        [Enum(SilhouetteOFF, 0, SilhouetteON, 3)] _ZTestMode("ZTest Mode", Float) = 3
        // Matching stencil refs will interact with each other. If you want overlapping outlines for multiple objects, consider giving them different refs in both Mask and Outline.
        _StencilRef("Stencil Ref (Match Outline)", int) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent+100"
        }
        LOD 100

        // Hides any outline that may render on top of the object.
        Stencil
        {
            Ref [_StencilRef]
            Pass Replace
        }

        Pass
        {
            Name "Mask"
            Cull Off
            ZTest [_ZTestMode]
            ZWrite Off
            ColorMask 0
        }
    }
}
