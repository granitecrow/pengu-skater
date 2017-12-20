Shader "GraniteCrow/CurvedWorld"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Curvature("Curvature", Float) = 0.001
	}
		SubShader
		{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert vertex:vert addshadow


		uniform sampler2D _MainTex;
		uniform float _Curvature;
		
		struct Input
		{
			float2 uv_MainTex;
		};

		//manipulates geometry
		void vert(inout appdata_full v)
		{
			//get coordinate in world space
			float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
			//adjust based on camera
			worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
			worldSpace = float4(0.0f, (worldSpace.z * worldSpace.z) * -_Curvature, 0.0f, 0.0f);

			v.vertex += mul(unity_WorldToObject, worldSpace);
		}
		
		//manipulates color
		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rbg;
			o.Alpha = c.a;
		}
		
		ENDCG
		
	}
			FallBack "Mobile/Diffuse"
}
