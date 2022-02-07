Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		//_RampTex("Ramp",2D) = "white" {}
		_CelShadingLevels("Cel Shader Levels", Range(1,10)) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		fixed _CelShadingLevels;
		//sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
        };

       
        fixed4 _Color;


        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            
            o.Albedo =  tex2D (_MainTex, IN.uv_MainTex).rgb;
           
        }

		fixed4 LightingToon (SurfaceOutput s, fixed3 lightDir, half3 viewDir, half atten)
		{
			half NdotL = dot (s.Normal, lightDir);
			half cel = floor(NdotL * _CelShadingLevels) / (_CelShadingLevels - 0.5);
			//NdotL = tex2D(_RampTex, fixed2(NdotL, 0.5));
			half4 color;
			color.rgb = s.Albedo * _LightColor0.rgb * (cel * atten);
			color.a = s.Alpha;

			return color;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
