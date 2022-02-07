Shader "Custom/VertexAnimation"
{
    Properties
    {
       _MainTex("Base (RGB)", 2D) = "white" {}
	   _tintAmount("Tint Amount", Range(0,1)) = 0.5
	   _ColorA("Color A", Color) = (1,1,1,1)
	   _ColorB("Color B", Color) = (1,1,1,1)
	   _Speed("Wave Speed", Range(0.1, 80)) = 5
	   _Frequency ("Wave Frequency", Range(0,5)) = 2
	   _Amplitude("Wave Amplitude", Range(-1,1)) = 1
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        

        struct Input
        {
            float2 uv_MainTex;
			float3 vertexColor;
        };

        sampler2D _MainTex;
		float4 _ColorA;
		float4 _ColorB;
		float _tintAmount;
		float _Frequency;
		float _Amplitude;
		float _Speed;
		float _OffsetVal;


		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float time = _Time * _Speed;
			float waveValueA = sin(time + v.vertex.x * _Frequency) * _Amplitude;

			v.vertex.xyz = float3(v.vertex.x, v.vertex.y + waveValueA, v.vertex.z);
			v.normal = normalize(float3(v.normal.x + waveValueA, v.normal.y,v.normal.z));
			o.vertexColor = float3(waveValueA,waveValueA,waveValueA);
		}
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            float3 tintColor = lerp(_ColorA, _ColorB, IN.vertexColor).rgb;
            // Metallic and smoothness come from slider variables
            o.Albedo = c.rgb *(tintColor *_tintAmount);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
