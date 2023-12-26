Shader "Custom/ColorShift"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _ColorC("Color C", Color) = (1,1,1,1)
        _Offset("Offset", float) = 0
        _Speed("Speed", float) = 0
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            fixed4 _ColorA;
            fixed4 _ColorB;
            fixed4 _ColorC;
            float _Offset;
            float _Speed;

            fixed4 lerp(fixed4 a, fixed4 b, float t) {
                return (b - a) * t + a;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = (_Time * _Speed) + _Offset + i.uv.x;
                time %= 1;
                if (time < 0) time += 1;

                if (time < .35)
                    return lerp(_ColorA, _ColorB, time / .35);
                if (time < .62)
                    return lerp(_ColorB, _ColorC, (time - .35) / .27);
                return lerp(_ColorC, _ColorA, (time - .62) / .38);
            }
            ENDCG
        }
    }
}
