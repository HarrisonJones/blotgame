Shader "Valerie/PaintTexture"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		OldPaintUv("OldPaintUv", VECTOR) = (0,0,0,0)
		PaintUv("PaintUv", VECTOR) = (0,0,0,0)
		PaintBrushSize("PaintBrushSize", Range(0,0.1) ) = 0.1
		PaintBrushColour("PaintBrushColour", Color ) = (0,0,0,0)
		OldCharacterSpeedFactor("OldCharacterSpeedFactor", float) = 0
		CharacterSpeedFactor("CharacterSpeedFactor", float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float PaintBrushSize;
			float2 OldPaintUv;
			float2 PaintUv;
			float4 PaintBrushColour;

			float CharacterSpeedFactor;
			float OldCharacterSpeedFactor;
	
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			// This function tests every point on the map and determines if we're going to fill it in
			fixed4 frag (v2f hitpos) : SV_Target
			{
				
				// Without any other testing, this will draw a dot on the character's current position
				// We're guaranteed to have space between dots though, so it doesn't look smooth unless we do something fancy
				if ( distance( hitpos.uv, PaintUv ) < PaintBrushSize * CharacterSpeedFactor )
					return float4( PaintBrushColour );

				// Calculate the direction from our last position to our current position
				float2 dirToCurrentPos = normalize(PaintUv - OldPaintUv); 

				// We can get a perpindicular line by (1, -1) of the direction we just calculated
				// Using that, we can create a bounding box that encapsulates our last dot and our current dot!
				float2 leftPos = PaintUv + (float2(dirToCurrentPos.y, -dirToCurrentPos.x) * -PaintBrushSize * CharacterSpeedFactor);
				float2 oldLeftPos = OldPaintUv + (float2(dirToCurrentPos.y, -dirToCurrentPos.x) * -PaintBrushSize * OldCharacterSpeedFactor);
				float2 rightPos = PaintUv + (float2(dirToCurrentPos.y, -dirToCurrentPos.x) * PaintBrushSize * CharacterSpeedFactor);
				float2 oldRightPos = OldPaintUv + (float2(dirToCurrentPos.y, -dirToCurrentPos.x) * PaintBrushSize * OldCharacterSpeedFactor);

				float2 polyPoints[4] = {
					leftPos,  oldLeftPos, oldRightPos, rightPos
				};

				float2 testCoordinate = hitpos.uv;

				// This is a standard bounding box test that tells us if we're inside the bounds we just calculated
				// https://en.wikipedia.org/wiki/Point_in_polygon if you're interested in the theory behind it
				int i, j= 0;
				int c = 0;
				for (i = 0, j = polyPoints.Length-1; i < polyPoints.Length; j = i++) {
					if ( ((polyPoints[i].y>testCoordinate.y) != (polyPoints[j].y>testCoordinate.y)) &&
						(testCoordinate.x < (polyPoints[j].x-polyPoints[i].x) * (testCoordinate.y-polyPoints[i].y) / (polyPoints[j].y-polyPoints[i].y) + polyPoints[i].x) )
						c = !c;
				}

				if(c)
					return float4( PaintBrushColour );

				return tex2D( _MainTex, testCoordinate );
			}
			ENDCG
		}
	}
}
