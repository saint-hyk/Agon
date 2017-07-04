Shader "Custom/SelectedShader" {
   Properties {
      _Colour ("Main Colour", Color) = (0.5, 0.5, 0.5, 1)
      _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
      _OutlineWidth ("Outline width", Range (0.002, 0.05)) = 0.005
      _Threshold ("Threshold", Range (0.001, 1.00)) = 0.001
   }

   SubShader {
      Tags { "RenderType"="Opaque" }
      Pass {
         Cull Back
         ZWrite On
         Blend SrcAlpha OneMinusSrcAlpha

         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         #include "UnityCG.cginc"

         struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 uv     : TEXCOORD0;
         };

         struct v2f {
            float4 pos        : SV_POSITION;
            float3 normal     : NORMAL;
            half4 screencoord : TEXCOORD0;
         };

         uniform sampler2D _CameraDepthNormalsTexture;
         uniform float4 _CameraDepthNormalsTexture_TexelSize;

         uniform float4 _OutlineColor;
         uniform float  _OutlineWidth;

         v2f vert(appdata v) {
            v2f o;

            o.pos = UnityObjectToClipPos(v.vertex);

            o.normal = v.normal;

            o.screencoord = ComputeScreenPos(o.pos);

            return o;
         }

         uniform float4 _Colour;
         uniform float _Threshold;

         fixed4 frag(v2f i) : SV_Target {
            half2 persp_fix_cord = i.screencoord.xy / i.screencoord.w;

            float3 centre = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, persp_fix_cord));
            float3 up     = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, persp_fix_cord + fixed2(0, _CameraDepthNormalsTexture_TexelSize.y)));
            float3 down   = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, persp_fix_cord - fixed2(0, _CameraDepthNormalsTexture_TexelSize.y)));
            float3 left   = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, persp_fix_cord - fixed2(_CameraDepthNormalsTexture_TexelSize.x, 0)));
            float3 right  = DecodeViewNormalStereo(tex2D(_CameraDepthNormalsTexture, persp_fix_cord + fixed2(_CameraDepthNormalsTexture_TexelSize.x, 0)));

            float3 up_diff    = abs(centre - up);
            float3 down_diff  = abs(centre - down);
            float3 left_diff  = abs(centre - left);
            float3 right_diff = abs(centre - right);

            half edge = length(up_diff + down_diff + left_diff + right_diff) > _Threshold;

            return (_Colour * (1 - edge)) + (_OutlineColor * edge);
         }

         ENDCG
      }
   }
}
