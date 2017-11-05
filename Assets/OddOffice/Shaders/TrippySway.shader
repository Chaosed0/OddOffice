Shader "Custom/TrippySway" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Top("Top", Float) = 1
		_Bottom("Bottom", Float) = 1
		_ObjHeight("ObjHeight", Float) = 1
		_MidPoint("MidPoint", Float) = 1
		_RandomOffset("RandomOffset", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Top;
		float _Bottom;
		float _ObjHeight;
		float _ObjWidth;
		float _MidPoint;
		float _Modifier;
		float _RandomOffset;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void vert(inout appdata_full v){
			float timeScale = 35;
			float heightDividend = 1.5;

			float height = v.vertex.y - (_MidPoint - _ObjHeight / heightDividend);
			float num = height / _ObjHeight;

			float addend = _Modifier * sin(_Time * timeScale + _RandomOffset) * num * num * (_ObjWidth + _ObjHeight) / 3;
			float heightAddend = _Modifier * sin((_Time * timeScale + _RandomOffset + 3.14) * 2) * .64;

			float temp = max(0, (_Top - (_Top - v.vertex.y)) / _ObjHeight);
			heightAddend *= temp;

			if (v.vertex.y >= (_MidPoint - _ObjHeight / heightDividend)) {
				v.vertex.z += addend;
			}
			v.vertex.y += heightAddend;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = c.rgb * .75;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
