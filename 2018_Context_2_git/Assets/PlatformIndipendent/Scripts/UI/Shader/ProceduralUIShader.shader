Shader "Custom/ProceduralUIShader"
{
	Properties
	{
	}
	SubShader
	{

		Cull Off
		//ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.5

			#include "UnityCG.cginc"

			StructuredBuffer<float3>	vertices;
			StructuredBuffer<float2>	uvs		;
			int vertexOffset = 0;
			sampler2D tex;

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

			v2f vert (uint id : SV_VertexID)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertices[id + vertexOffset]);
				o.uv = uvs[id + vertexOffset];
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(tex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
