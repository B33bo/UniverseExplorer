Shader "Hidden/Aurora"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplacementA("Displacement", 2D) = "white" {}
        _DisplacementB("Displacement 2", 2D) = "white" {}

        _DisplacementASpeedX("Displacement A Speed X", float) = 1
        _DisplacementASpeedY("Displacement A Speed Y", float) = 1

        _DisplacementBSpeedX("Displacement B Speed X", float) = 1
        _DisplacementBspeedY("Displacement B Speed Y", float) = 1

        _ColorA("Color A", Color) = (1, 1, 1, 1)
        _ColorB("Color B", Color) = (1, 1, 1, 1)

        _Rainbow("Is Rainbow", int) = 0

        _Alpha("Alpha", float) = 1
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

            sampler2D _MainTex;
            sampler2D _DisplacementA;
            float4 _ColorA;
            float4 _ColorB;
            float _DisplacementASpeedX;
            float _DisplacementASpeedY;
            float _Alpha;

            sampler2D _DisplacementB;
            float _DisplacementBSpeedX;
            float _DisplacementBSpeedY;

            int _Rainbow;

            float4 Lerp(float4 a, float4 b, float t) {
                return (b - a) * t + a;
            }

            float3 GetDisplacement(float x, float y, float xSpeed, float ySpeed, sampler2D tex) {
                x += _Time * xSpeed;
                y += _Time * ySpeed;

                x %= 1;
                y %= 1;
                if (x < 0)
                    x += 1;
                if (y < 0)
                    y += 1;

                float4 color = tex2D(tex, float2(x, y));
                return float3(color.r, color.g, color.b);
            }

            float3 Hue(float H)
            {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R, G, B));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float3 change = GetDisplacement(i.uv.x, i.uv.y, _DisplacementASpeedX, _DisplacementASpeedY, _DisplacementA);
                float alphaMultiplier = change.b;

                uv += float2(change.x, change.y);

                change = GetDisplacement(i.uv.x, i.uv.y, _DisplacementBSpeedX, _DisplacementBSpeedY, _DisplacementB);
                uv += float2(change.x, change.y);

                alphaMultiplier += change.b;
                float lerpAmount = (uv.x + _Time) % 2;

                if (lerpAmount > 1)
                    lerpAmount = 2 - lerpAmount;

                fixed4 col = tex2D(_MainTex, i.uv); // for the alpha

                if (_Rainbow > 0)
                    col.rgb = Hue(lerpAmount);
                else
                    col.rgb = Lerp(_ColorA, _ColorB, lerpAmount);
                col.a *= alphaMultiplier;

                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
