Shader "Custom/OjosShader"
{
    Properties
    {
        //_EyeColor("Eye Color", Color) = (1,1,1,1)
        //_BackColor("Background Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
        //_offsetX("Eye Movement in X", Range(-1,1)) = 0
        //_offsetY("Eye Movement in Y", Range(-1,1)) = 0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass{
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            //#pragma multi_compile_fog;
            
            #include "UnityCG.cginc"
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv:TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Maintex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            fixed4 frag(v2f i) : SV_TARGET
            {
                fixed col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
         }

    }
}
    
