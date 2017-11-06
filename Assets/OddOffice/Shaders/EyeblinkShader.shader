Shader "Unlit/EyeblinkShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// fixed4 col = tex2D(_MainTex, i.uv);
				/*
				float2 uv = i.uv - float2(-0.1,0.5);
				float d = 0.45 * length(uv);

				float rate = 5.0;
				float eyeWidthInv = 0.5 * cos((_Time-3.14)*rate) + 0.5;

				if (_Time > 3.0 && _Time < 4.5)
				{
					eyeWidthInv = 1.0;
				}
				else if (_Time > 5.0)
				{
					eyeWidthInv = 0.0;
				}

				float redComp = 0.0;
				if (eyeWidthInv < 0.7)
				{
					float angle = atan(200.0 * (eyeWidthInv*eyeWidthInv*eyeWidthInv) * uv.y, uv.x);
					float redAngle = 1.0 - abs(angle);
				}
				else
				{
					redComp = 0.0;
				}
				float4 col = float4(redComp,0.0,0.0,1.0);
				*/
				float4 col = float4(1.0,0.0,0.0,1.0);
				return col;
			}
			ENDCG
		}
	}
}
