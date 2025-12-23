Shader "UI/MarkerStylized"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Pulse ("Pulse", Range(0,1)) = 0

        _BaseRadius ("Base Radius", Range(0,0.5)) = 0.18
        _RingWidth ("Ring Width", Range(0,0.2)) = 0.025
        _Softness ("Softness", Range(0,0.1)) = 0.02

        _NoiseStrength ("Noise Strength", Range(0,0.05)) = 0.02
        _Glow ("Glow", Range(0,1)) = 0.4
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
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
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            fixed4 _Color;
            float _Pulse;
            float _BaseRadius;
            float _RingWidth;
            float _Softness;
            float _NoiseStrength;
            float _Glow;

            // Petit bruit procédural
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed circleMask(float dist, float radius, float softness)
            {
                return 1.0 - smoothstep(radius, radius + softness, dist);
            }

            fixed ringMask(float dist, float radius, float width, float softness)
            {
                float inner = smoothstep(radius - width, radius - width + softness, dist);
                float outer = smoothstep(radius, radius + softness, dist);
                return saturate(inner - outer);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV centrées
                float2 uv = i.uv - 0.5;
                float dist = length(uv);

                // Bruit léger pour styliser
                float noise = hash21(uv * 30.0) * _NoiseStrength;

                // Point central plus petit + pulsation
                float pointPulse = 0.01 * sin(_Pulse * 6.28);
                float center = 1.0 - smoothstep(0.02 + pointPulse + noise, 0.04 + pointPulse + noise, dist);

                // Pulse global pour l’anneau
                float pulse = smoothstep(0,1,_Pulse);

                // Rayon animé de l’anneau
                float radius = _BaseRadius + pulse * 0.15 + noise;

                // Anneau stylisé
                float ring = ringMask(dist, radius, _RingWidth, _Softness);
                ring *= (1.0 - pulse); // légère dissolution progressive

                // Alpha final
                float alpha = saturate(center + ring);

                // Glow léger
                float glow = ring * _Glow;

                fixed4 col = _Color;
                col.rgb += glow;
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}
