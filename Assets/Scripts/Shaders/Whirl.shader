Shader "Custom/Whirl"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Rotation("Rotation", float) = 0
        _Number("Number", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
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

            float _Number;

            float2 Rotate(float2 pos, float rotation) {
                pos -= .5f;
                rotation *= 1 / length(pos);
                float AngleFromCenter = atan2(pos.y, pos.x);
                AngleFromCenter += rotation;
                float2 newPos = length(pos);
                newPos.x *= cos(AngleFromCenter);
                newPos.y *= sin(AngleFromCenter);
                newPos += .5f;

                newPos %= 1;
                if (newPos.x < 0)
                    newPos.x += 1;
                if (newPos.y < 0)
                    newPos.y += 1;

                return newPos;
            }

            sampler2D _MainTex;
            float _Rotation;

            fixed4 frag(v2f i) : SV_Target
            {
                i.uv = Rotate(i.uv, _Rotation * 0.0174533);
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
