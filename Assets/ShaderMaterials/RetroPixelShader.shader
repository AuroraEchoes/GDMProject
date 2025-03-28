Shader "Custom/RetroPixelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(1, 64)) = 4
        _ColorDepth ("Color Depth", Range(1, 32)) = 16
        _DitheringStrength ("Dithering Strength", Range(0, 1)) = 0.5
        _EdgeStrength ("Edge Strength", Range(0, 1)) = 0.5
        _ScanlineStrength ("Scanline Strength", Range(0, 1)) = 0.15
        _ScanlineSize ("Scanline Size", Range(1, 10)) = 3
        _Contrast ("Contrast", Range(0.5, 2)) = 1.2
        _Brightness ("Brightness", Range(0.5, 1.5)) = 1.0
        _PaletteMap ("Palette Map", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off Cull Off

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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _PaletteMap;
            float _PixelSize;
            float _ColorDepth;
            float _DitheringStrength;
            float _EdgeStrength;
            float _ScanlineStrength;
            float _ScanlineSize;
            float _Contrast;
            float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            // dither matrix
            static const float bayerMatrix[16] = {
                0.0/16.0, 8.0/16.0, 2.0/16.0, 10.0/16.0,
                12.0/16.0, 4.0/16.0, 14.0/16.0, 6.0/16.0,
                3.0/16.0, 11.0/16.0, 1.0/16.0, 9.0/16.0,
                15.0/16.0, 7.0/16.0, 13.0/16.0, 5.0/16.0
            };

            // Sobel operator
            float detectEdges(sampler2D tex, float2 uv, float2 texelSize) {
                float2 offsetX = float2(texelSize.x, 0);
                float2 offsetY = float2(0, texelSize.y);
                
                float3 pixL = tex2D(_MainTex, uv - offsetX).rgb;
                float3 pixR = tex2D(_MainTex, uv + offsetX).rgb;
                float3 pixU = tex2D(_MainTex, uv - offsetY).rgb;
                float3 pixD = tex2D(_MainTex, uv + offsetY).rgb;
                
                float3 gradX = pixR - pixL;
                float3 gradY = pixD - pixU;
                
                float edgeX = length(gradX);
                float edgeY = length(gradY);
                
                return (edgeX + edgeY) * 0.5;
            }

            // Small colour pallete
            float3 quantizeColor(float3 color, float levels) {
                return floor(color * levels) / levels;
            }

            // TODO add map to texture
            float3 mapToPalette(float3 color) {
                return tex2D(_PaletteMap, float2(color.r, 0.5)).rgb;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Pixelate
                float2 texelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                float2 pixelatedUV = floor(i.uv / (texelSize * _PixelSize)) * (texelSize * _PixelSize);
                float4 col = tex2D(_MainTex, pixelatedUV);
                
                // contrast & brightness
                col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;
                col.rgb *= _Brightness;
                
                float edge = detectEdges(_MainTex, pixelatedUV, texelSize * _PixelSize);
                col.rgb = lerp(col.rgb, col.rgb * (1.0 - edge), _EdgeStrength);
                
                // dithering
                uint x = (uint)fmod(i.screenPos.x / _PixelSize, 4);
                uint y = (uint)fmod(i.screenPos.y / _PixelSize, 4);
                float dither = bayerMatrix[y * 4 + x];
                
                // Colour dithering
                float3 quantizedColor = quantizeColor(col.rgb, _ColorDepth);
                col.rgb = lerp(quantizedColor, col.rgb, dither * _DitheringStrength);
                col.rgb = quantizeColor(col.rgb, _ColorDepth);
                
                // TODO fix scanlines
                float scanline = fmod(i.screenPos.y, _ScanlineSize * 2) < _ScanlineSize ? 1.0 - _ScanlineStrength : 1.0;
                col.rgb *= scanline;
                
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}