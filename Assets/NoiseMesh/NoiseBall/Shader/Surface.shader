Shader "NoiseBall/Surface"
{
    Properties
    {
        _Color ("", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        #include "Common.cginc"

        struct Input { float dummy; float2 uv_MainTex; };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        sampler2D _MainTex;
       // float4 _MainTex_ST;

        void vert(inout appdata_full v)
        {
            float3 v1 = displace(v.vertex.xyz);
            float3 v2 = displace(v.texcoord.xyz);
            float3 v3 = displace(v.texcoord1.xyz);
            v.vertex.xyz = v1;
            v.normal = normalize(cross(v2 - v1, v3 - v1));
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
