Shader "Custom/GaussianBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurSize("Blur Size", Range(0.0, 10.0)) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

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

                sampler2D _MainTex;
                float _BlurSize;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 color = tex2D(_MainTex, i.uv);
                    fixed4 blurredColor = fixed4(0.0, 0.0, 0.0, 0.0);
                    float blurSize = _BlurSize;
                    float2 texelSize = 1.0 / _ScreenParams.xy;

                    // Gaussian blur kernel
                    fixed weights[9] = { 0.05, 0.09, 0.12, 0.15, 0.16, 0.15, 0.12, 0.09, 0.05 };

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            float2 offset = float2(x, y) * texelSize * blurSize;
                            blurredColor += tex2D(_MainTex, i.uv + offset) * weights[(x + 1) + (y + 1) * 3];
                        }
                    }

                    return blurredColor;
                }
                ENDCG
            }
        }
}