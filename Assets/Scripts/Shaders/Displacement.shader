Shader "Custom/Displacement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Displacement ("Displacement", 2D) = "bump" {}
        _Displacement2 ("Displacement", 2D) = "bump" {}
        _Multiplier("Multiplier", float) = 1
        _ShowDisplacement2("Show Displacement 2", int) = 0
        _SpeedX("X Speed", float) = 0
        _SpeedY("Y Speed", float) = 0
        _Speed2X("X Speed", float) = 0
        _Speed2Y("Y Speed", float) = 0
    }
    SubShader
    {
        // No culling or depth
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

            sampler2D _MainTex;
            sampler2D _Displacement;
            sampler2D _Displacement2;
            float _Multiplier;
            float _SpeedX;
            float _SpeedY;
            float _Speed2X;
            float _Speed2Y;
            bool _ShowDisplacement2;

            float mod(float a, float b) {
                a %= b;
                if (a < 0)
                    a += b;
                return a;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 displacementPos = float2(mod(i.uv.x + _Time * _SpeedX,1), mod(i.uv.y + _Time * _SpeedY,1));
                fixed4 displacement = tex2D(_Displacement, displacementPos);

                if (_ShowDisplacement2) {
                    displacementPos = float2(mod(i.uv.x + _Time * _Speed2X, 1), mod(i.uv.y + _Time * _Speed2Y, 1));
                    displacement += tex2D(_Displacement2, displacementPos);
                }
                else {
                    displacement *= 2;
                }

                i.uv.x += (displacement.r - 1) * _Multiplier;
                i.uv.y += (displacement.g - 1) * _Multiplier;

                i.uv.x = mod(i.uv.x, 1);
                i.uv.y = mod(i.uv.y, 1);

                fixed4 col = tex2D(_MainTex, i.uv);
                
                // just invert the colors
                col.rgb = col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
