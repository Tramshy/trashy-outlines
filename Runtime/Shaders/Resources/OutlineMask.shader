Shader "Trashy Outlines/Outline Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Enum(SilhouetteOFF, 0, SilhouetteON, 3)] _ZTestMode("ZTest Mode", Float) = 3
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent+100"
        }
        LOD 100

        Stencil
        {
            Ref 1
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
