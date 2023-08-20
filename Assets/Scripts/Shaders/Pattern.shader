Shader "Custom/Pattern"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorA("Red", Color) = (0,0,0,1)
        _ColorB("Green", Color) = (0,0,0,1)
        _ColorC("Blue", Color) = (0,0,0,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                float4 newColor = float4(0, 0, 0, 0);
                newColor += _ColorA * col.r;
                newColor += _ColorB * col.g;
                newColor += _ColorC * col.b;
                newColor.a = col.a * (_ColorA.a * col.r + _ColorB.a * col.g + _ColorC.a * col.b);
                return newColor;
            }
            ENDCG
        }
    }
}
