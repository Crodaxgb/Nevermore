Shader "Custom/WireFrameShader"
{
    Properties
    {
        _Color ("Wire Color", Color) = (1,1,1,1) // white default
    }
    CGINCLUDE
    float4 _Color;
    ENDCG
   SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Pass
        {
            Cull Off          // see through the cube
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma target 4.5                // geometry shader
            #pragma vertex   vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; };
            struct v2g     { float4 pos : POSITION; };
            struct g2f     { float4 pos : SV_POSITION; float4 col : COLOR; };

            v2g vert (appdata v)
            {
                v2g o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(24)]
            void geom(triangle v2g IN[3], inout LineStream<g2f> LS)
            {
                for (int i = 0; i < 3; i++)
                {
                    int j = (i + 1) % 3;
                    g2f p1; p1.pos = IN[i].pos; p1.col = _Color;
                    g2f p2; p2.pos = IN[j].pos; p2.col = _Color;
                    LS.Append(p1); LS.Append(p2);
                }
            }

            fixed4 frag(g2f i) : SV_Target { return _Color; }
            ENDCG
        }
    }
}
