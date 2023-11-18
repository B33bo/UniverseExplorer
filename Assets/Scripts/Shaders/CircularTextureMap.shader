Shader "Hidden/CircularTextureMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha("Alpha", float) = 1
        _Ring("Ring", float) = 0
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

            float2 ToPolarCoordinates(float2 vec) {
                vec.x -= .5f;
                vec.y -= .5f;
                float magnitude = sqrt(vec.x * vec.x + vec.y * vec.y);
                float argument = atan2(vec.y, vec.x);
                return float2(magnitude * 2, argument);
            }

            sampler2D _MainTex;
            float _Alpha;
            float _Ring;

            float stretchRing(float magnitude) {
                return (magnitude - _Ring) / (1 - _Ring);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float pi = 3.141592653589793238462;
                float2 polar = ToPolarCoordinates(i.uv);
                float argAsPercent = (polar.y + pi) / (2 * pi);

                if (polar.x > 1 || polar.x < _Ring)
                    return fixed4(0, 0, 0, 0);

                fixed4 col = tex2D(_MainTex, float2(argAsPercent, 1- stretchRing(polar.x)));
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
