// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Eyeblink"
{
	Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_StartTime ("StartTime", Float) = 0.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
			float _StartTime;

            float4 frag (v2f i) : COLOR
            {
				float elapsedTime = _Time[1] - _StartTime;

				float2 uv = i.uv - float2(-0.15,0.5);
				float d = 0.65 * length(uv);

				float rate = 6.0;
				float eyeWidthInv = 0.5 * cos((elapsedTime-3.14)*rate) + 0.5;

				bool afterPauseStart = elapsedTime > 3.0;
				bool beforePauseEnd = elapsedTime < 4.0;
				bool afterLastBlink = elapsedTime > 4.75;
				if (afterPauseStart && beforePauseEnd)
				{
					eyeWidthInv = 1.0;
				}
				else if (afterLastBlink)
				{
					eyeWidthInv = 0.0;
				}

				float redComp = 0.0;
				if (eyeWidthInv < 0.7)
				{
					float angle = atan2(200.0 * (eyeWidthInv*eyeWidthInv*eyeWidthInv) * uv.y, uv.x);
					float redAngle = 1.0 - abs(angle);
					redComp = smoothstep(redAngle, redAngle-0.002, d);
				}
				else
				{
					redComp = 0.0;
				}

				float4 col = float4(redComp,0.0,0.0,1.0);
                return col;
            }
            ENDCG
        }
    }
}
