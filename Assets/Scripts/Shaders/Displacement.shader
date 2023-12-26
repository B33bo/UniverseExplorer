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
        _SpeedRot("Rotate Speed", float) = 0
        _Speed2X("X Speed 2", float) = 0
        _Speed2Y("X Speed 2", float) = 0
        _Speed2Rot("Rotate Speed 2", float) = 0
        _Color("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Tags
        {
            "LightMode" = "Universal2D"
        }
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            sampler2D _Displacement;
            sampler2D _Displacement2;
            float _Multiplier;
            float _SpeedX;
            float _SpeedY;
            float _SpeedRot;
            float _Speed2X;
            float _Speed2Y;
            float _Speed2Rot;
            int _ShowDisplacement2;
            fixed4 _Color;

            float mod(float a, float b) {
                a %= b;
                if (a < 0)
                    a += b;
                return a;
            }

            float2 rotate(float2 pos, float amount) {
                pos.x -= .5f;
                pos.y -= .5f;

                float magnitude = sqrt(pos.x * pos.x + pos.y * pos.y);
                float rotation = atan2(pos.y, pos.x) + amount;

                pos.x = cos(rotation) * magnitude + .5;
                pos.y = sin(rotation) * magnitude + .5;

                return pos;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 displacementPos = float2(mod(i.uv.x + _Time * _SpeedX,1), mod(i.uv.y + _Time * _SpeedY,1));
                fixed4 displacement = tex2D(_Displacement, rotate(displacementPos, _SpeedRot * _Time));

                if (_ShowDisplacement2 > 0) {
                    displacementPos = float2(mod(i.uv.x + _Time * _Speed2X, 1), mod(i.uv.y + _Time * _Speed2Y, 1));
                    displacement += tex2D(_Displacement2, rotate(displacementPos, _Speed2Rot * _Time));
                }
                else {
                    displacement *= 2;
                }

                i.uv.x += (displacement.r - 1) * _Multiplier;
                i.uv.y += (displacement.g - 1) * _Multiplier;

                i.uv.x = mod(i.uv.x, 1);
                i.uv.y = mod(i.uv.y, 1);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
