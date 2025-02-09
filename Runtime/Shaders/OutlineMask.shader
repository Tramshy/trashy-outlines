Shader "Trashy Outlines/Outline Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            ZTest LEqual
            ZWrite Off
            ColorMask 0
        }
    }
}
