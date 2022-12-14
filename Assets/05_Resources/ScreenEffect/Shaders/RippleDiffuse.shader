Shader "Hidden/RippleDiffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_WaveCount ("WaveCount", int) = 1

		_Scale ("Scale", float) = 1
		_Speed ("Speed", float) = 1
		_Frequency ("Frequency", float) = 1
		_ExternalRadio ("ExternalRadio", float) = .1

		_AspectRatio ("AspectRatio", float) = 1
		_CircleXScale ("CircleXScale", float) = 1
		_CircleYScale ("CircleYScale", float) = 1

		_InternalRadio0 ("InternalRadio0", float) = .1
		_InternalRadio1 ("InternalRadio1", float) = .1
		_InternalRadio2 ("InternalRadio2", float) = .1
		_InternalRadio3 ("InternalRadio3", float) = .1
		_InternalRadio4 ("InternalRadio4", float) = .1
		_InternalRadio5 ("InternalRadio5", float) = .1
		_InternalRadio6 ("InternalRadio6", float) = .1
		_InternalRadio7 ("InternalRadio7", float) = .1
		_InternalRadio8 ("InternalRadio8", float) = .1
		_InternalRadio9 ("InternalRadio9", float) = .1

		//WebGL doesn't support Vector
		_TargetPosX0 ("TargetPosX0", float) = .5
		_TargetPosX1 ("TargetPosX1", float) = .5
		_TargetPosX2 ("TargetPosX2", float) = .5
		_TargetPosX3 ("TargetPosX3", float) = .5
		_TargetPosX4 ("TargetPosX4", float) = .5
		_TargetPosX5 ("TargetPosX5", float) = .5
		_TargetPosX6 ("TargetPosX6", float) = .5
		_TargetPosX7 ("TargetPosX7", float) = .5
		_TargetPosX8 ("TargetPosX8", float) = .5
		_TargetPosX9 ("TargetPosX9", float) = .5

		_TargetPosY0 ("TargetPosY0", float) = .5
		_TargetPosY1 ("TargetPosY1", float) = .5
		_TargetPosY2 ("TargetPosY2", float) = .5
		_TargetPosY3 ("TargetPosY3", float) = .5
		_TargetPosY4 ("TargetPosY4", float) = .5
		_TargetPosY5 ("TargetPosY5", float) = .5
		_TargetPosY6 ("TargetPosY6", float) = .5
		_TargetPosY7 ("TargetPosY7", float) = .5
		_TargetPosY8 ("TargetPosY8", float) = .5
		_TargetPosY9 ("TargetPosY9", float) = .5

		_Amplitude0 ("Amplitude0", float) = 1
		_Amplitude1 ("Amplitude1", float) = 1
		_Amplitude2 ("Amplitude2", float) = 1
		_Amplitude3 ("Amplitude3", float) = 1
		_Amplitude4 ("Amplitude4", float) = 1
		_Amplitude5 ("Amplitude5", float) = 1
		_Amplitude6 ("Amplitude6", float) = 1
		_Amplitude7 ("Amplitude7", float) = 1
		_Amplitude8 ("Amplitude8", float) = 1
		_Amplitude9 ("Amplitude9", float) = 1

		_MaxValue0 ("MaxValue0", float) = 1
		_MaxValue1 ("MaxValue1", float) = 1
		_MaxValue2 ("MaxValue2", float) = 1
		_MaxValue3 ("MaxValue3", float) = 1
		_MaxValue4 ("MaxValue4", float) = 1
		_MaxValue5 ("MaxValue5", float) = 1
		_MaxValue6 ("MaxValue6", float) = 1
		_MaxValue7 ("MaxValue7", float) = 1
		_MaxValue8 ("MaxValue8", float) = 1
		_MaxValue9 ("MaxValue9", float) = 1
	}
	SubShader {

		Lighting Off
		ZTest Always
		Cull Off
		ZWrite Off
		Fog { Mode Off }

		Pass {
			CGPROGRAM
			#pragma exclude_renderers flash
	  		#pragma vertex vert_img
	  		#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;

			uniform float _WaveCount;
			uniform float _Scale, _Speed, _Frequency, _ExternalRadio;
			uniform float _AspectRatio, _CircleXScale, _CircleYScale;
			uniform float _InternalRadio0, _InternalRadio1, _InternalRadio2, _InternalRadio3, _InternalRadio4, _InternalRadio5, _InternalRadio6, _InternalRadio7, _InternalRadio8, _InternalRadio9;
			uniform float _Amplitude0, _Amplitude1, _Amplitude2, _Amplitude3, _Amplitude4, _Amplitude5, _Amplitude6, _Amplitude7, _Amplitude8, _Amplitude9;
			uniform float _MaxValue0, _MaxValue1, _MaxValue2, _MaxValue3, _MaxValue4, _MaxValue5, _MaxValue6, _MaxValue7, _MaxValue8, _MaxValue9; 
			uniform float _TargetPosX0, _TargetPosX1, _TargetPosX2, _TargetPosX3, _TargetPosX4, _TargetPosX5, _TargetPosX6, _TargetPosX7, _TargetPosX8, _TargetPosX9;
			uniform float _TargetPosY0, _TargetPosY1, _TargetPosY2, _TargetPosY3, _TargetPosY4, _TargetPosY5, _TargetPosY6, _TargetPosY7, _TargetPosY8, _TargetPosY9;

			float2 calculateCoordinatesOffset(float2 target, float2 coords, float amplitude, float internalRadio, float maxValue){
				float2 texCoordOffset = float2(.0, .0);
				float dir = coords - target;
				float dist = distance(float2(coords.x * _AspectRatio / _CircleXScale, coords.y / _CircleYScale), float2(target.x * _AspectRatio / _CircleXScale, target.y / _CircleYScale));

				if(dist <= _ExternalRadio + internalRadio) {
					float2 offset = (_Scale * dir * (sin(dist * 80. * _Frequency - _Time.w * 15. * _Speed) + .5) / 30.) * amplitude;

					maxValue = maxValue + internalRadio; //Set max wave value offset

					if(dist <= maxValue) { //Pixel between max value and center
						if(dist <= internalRadio) { //Pixel between end of InternalRadio and center => No more waves on the starting position
							//Do nothing
						}
						else { //Pixel between MaxValue and end of InternalRadio 
							float distFromCenter = (dist - internalRadio) / (maxValue - internalRadio);
							texCoordOffset = offset * distFromCenter;
						}
					}
					else { //Pixel between start of ExternalRadio + InternalRadio and MaxValue
						float distFromCenter = (dist - maxValue) / (_ExternalRadio + internalRadio - maxValue);
						texCoordOffset = offset - offset * distFromCenter;
					}
				}
				
				return texCoordOffset;
			}

			float4 frag(v2f_img i) : COLOR {
				float2 coords = i.uv; //Get uv coords
				float2 texCoord = coords;

				//*****0***** //Do calculations for each position
				if(_WaveCount > 0)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX0, _TargetPosY0), coords, _Amplitude0, _InternalRadio0, _MaxValue0);
				
				if(_WaveCount > 1)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX1, _TargetPosY1), coords, _Amplitude1, _InternalRadio1, _MaxValue1);

				if(_WaveCount > 2)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX2, _TargetPosY2), coords, _Amplitude2, _InternalRadio2, _MaxValue2);

				if(_WaveCount > 3)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX3, _TargetPosY3), coords, _Amplitude3, _InternalRadio3, _MaxValue3);

				if(_WaveCount > 4)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX4, _TargetPosY4), coords, _Amplitude4, _InternalRadio4, _MaxValue4);

				if(_WaveCount > 5)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX5, _TargetPosY5), coords, _Amplitude5, _InternalRadio5, _MaxValue5);

				if(_WaveCount > 6)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX6, _TargetPosY6), coords, _Amplitude6, _InternalRadio6, _MaxValue6);

				if(_WaveCount > 7)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX7, _TargetPosY7), coords, _Amplitude7, _InternalRadio7, _MaxValue7);

				if(_WaveCount > 8)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX8, _TargetPosY8), coords, _Amplitude8, _InternalRadio8, _MaxValue8);

				if(_WaveCount > 9)
					texCoord += calculateCoordinatesOffset(float2(_TargetPosX9, _TargetPosY9), coords, _Amplitude9, _InternalRadio9, _MaxValue9);

				//Timed
				float4 diffuse; //Target diffuse ripple color
				diffuse = tex2D(_MainTex, texCoord); //Pixel color result

				return diffuse;
			}
			ENDCG
		}
	}

	Fallback "Diffuse"
}