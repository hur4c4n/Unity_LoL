Shader "Player/Silhouette" {
Properties 
{
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _BumpMap ("Normalmap", 2D) = "bump" {}
 _SilhouetteColor ("Silhouette Color", Color) = (1,1,1,1)
}

SubShader {
 Tags { "LightMode"="Always" "Queue" = "Transparent" }
 LOD 250
 
 //code dessinant la silhouette
 Pass
 {
 Name "SILHOUETTE"
 ZTest Greater
 Offset 0 , -3000
 cull back
 ZWrite off
 
  CGPROGRAM
  #pragma fragment frag
  #pragma vertex vert
  #include "UnityCG.cginc"
  float4 _SilhouetteColor;
  struct v2f {
      float4 pos : SV_POSITION;
  };
  
  v2f vert (appdata_base v)
  {
      v2f o;
      o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
      return o;
  }
  
  half4 frag (v2f i) : COLOR
  {
      return _SilhouetteColor;
  }
  ENDCG 
 }
}

FallBack "Mobile/Diffuse"
}