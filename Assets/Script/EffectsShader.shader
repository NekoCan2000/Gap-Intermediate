//シェーダプログラミング（Unity独自シェーダ：ShaderLab）
//このプログラムをMaterialに貼り付ける
//参考資料URL
//http://nn-hokuson.hatenablog.com/entry/2017/04/14/204822
//http://tsumikiseisaku.com/blog/shader-tutorial-001/

Shader "Custom/EffectShader" {
	Properties {
		_Color ("色", Color) = (1,1,1,1)
		_MainTex ("元画像 (RGB)", 2D) = "white" {}
		_DisolveTex ("反映画像 (RGB)", 2D) = "white" {}
		_Threshold("しきい値", Range(0,1))= 0.0
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		LOD 200 //描画タイミング
		CGPROGRAM //描画処理ここから

		//#pragma surface 関数名 ライティングモデル オプション
		#pragma surface surf Standard fullforwardshadows
		//使用バージョン（デフォルトは3）
		#pragma target 3.0

		sampler2D _MainTex;//表示画像
		sampler2D _DisolveTex;//エフェクト画像

		//後で使われるInputの構造体を宣言
		struct Input {
			float2 uv_MainTex;
		};
		half _Threshold; //しきい値
		fixed4 _Color; //fixe ×4→Vector4

		//struct SurfaceOutputStandard {
		//	fixed3 Albedo;      // 色（RGB） デフォルトは黒
		//	fixed3 Normal;      // 法線（XYZ） デフォルトは設定なし
		//	half3  Emission;    // 自己発光（RGB） デフォルトは黒
		//	half   Metallic;    // 金属かどうか（0 ～ 1） デフォルトは0
		//	half   Smoothness;  // ツルツル度合い（0 ～ 1） デフォルトは0
		//	half   Occlusion;   // オクルージョン・遮蔽（0 ～ 1） デフォルトは1
		//	fixed  Alpha;       // 不透明度（0 ～ 1） デフォルトは0
		//};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//uv_MainTexに_DisolveTexをかけてmに代入
			fixed4 m = tex2D (_DisolveTex, IN.uv_MainTex);

			half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
			if( g < _Threshold ){ //彩度がしきい値未満なら
				discard;//計算を破棄
			} 
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;//色を取得
			o.Albedo = c.rgb; //出力色に代入
			o.Emission = c.rgb;//発光（これがないと暗い）
			o.Alpha = c.a;//出力色に代入
		}
		ENDCG //描画処理終わり
	}
	//滑り止め（描画処理が対応できなかった時に行うシェーダ）
	FallBack "Diffuse"
}