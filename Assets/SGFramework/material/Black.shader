Shader "UI/Black"     
{     
    Properties     
    {     
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}     
        //_Color ("Tint", Color) = (1,1,1,1)  
        _Color2 ("Black", Color) = (1,1,1,1)  
    }     
     
    SubShader     
    {     
        Tags     
        {      
            "Queue"="Transparent"      
            "IgnoreProjector"="True"      
            "RenderType"="Transparent"      
            "PreviewType"="Plane"     
            "CanUseSpriteAtlas"="True"     
        }     

        Blend SrcAlpha OneMinusSrcAlpha  
     
        Pass     
        {     
            CGPROGRAM     
            #pragma vertex vert     
            #pragma fragment frag     
            #include "UnityCG.cginc"     
                 
            struct appdata_t     
            {     
                float4 vertex   : POSITION;     
                float4 color    : COLOR;     
                float2 texcoord : TEXCOORD0;     
            };     
     
            struct v2f     
            {     
                float4 vertex   : SV_POSITION;     
                fixed4 color    : COLOR;     
                half2 texcoord  : TEXCOORD0;     
            };     
               
            sampler2D _MainTex;       
            fixed4 _Color2;     
     
            v2f vert(appdata_t IN)     
            {     
                v2f OUT;     
                OUT.vertex = UnityObjectToClipPos(IN.vertex);     
                OUT.texcoord = IN.texcoord;     
#				ifdef UNITY_HALF_TEXEL_OFFSET     
                OUT.vertex.xy -= (_ScreenParams.zw - 1.0);     
#				endif     
                OUT.color = IN.color ;//* _Color;     
                return OUT;  
            }  
     
            fixed4 frag(v2f IN) : SV_Target     
            {     
                // float maskAlpha = 0.9;
                // half4 maskColor = float4(0,0,0,1);
                // half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;     
                // float grey = dot(color.rgb, maskColor);   
                // half4 black = half4(grey,grey,grey,color.a * maskAlpha);  
                // return ( color * (1.0 - black.a) + black) * black.a ;

                // //ban tou ming de hei
                // half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;     
                // float grey = dot(color.rgb, fixed3(0.05, 0.05, 0.05));   
                // return half4(grey,grey,grey,color.a);         
                
                half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                _Color2.rgb *= _Color2.a;
                color.rgb *= _Color2.rgb;
                color.a *= _Color2.a;
                // color.a *= 1;
                return color;
            }     
            ENDCG     
        }     
    }     
}