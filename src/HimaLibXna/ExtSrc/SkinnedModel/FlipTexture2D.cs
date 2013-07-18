using System;

using Microsoft.Xna.Framework.Graphics;

namespace TexSkinningSample
{
    /// <summary>
    /// 連続書き込み用テクスチャクラス
    /// 
    /// 同一のテクスチャに連続でデータを書き込むとGPUとバッティングするので
    /// 複数のテクスチャを切り替える必要がある
    /// 
    /// 処理自体は簡単だが不要な変数やコードが増えるのでクラスとしてまとめている
    /// </summary>
    public class FlipTexture2D : IDisposable
    {
        #region プロパティ

        /// <summary>
        /// 現在のテクスチャの取得
        /// </summary>
        public Texture2D Texture { get { return textures[index]; } }

        #endregion

        #region フィールド

        Texture2D[] textures;
        int index;

        #endregion

        /// <summary>
        /// フリップテクスチャの生成
        /// Texture2Dのコンストラクタと引数は一緒で
        /// テクスチャ数を指定するnumberTexturesが追加されている
        /// </summary>
        /// <param name="numberOfTextures">フリップ用テクスチャ数</param>
        public FlipTexture2D(GraphicsDevice graphicsDeivce, int width, int height,
                    bool mipMap, SurfaceFormat format,
                    int numberTextures )
        {
            // テクスチャの初期化
            textures = new Texture2D[numberTextures];
            for ( int i = 0; i < textures.Length; ++i )
            {
                textures[i] = new Texture2D( graphicsDeivce, width, height, mipMap, format );
            }
        }

        /// <summary>
        /// フリップテクスチャの生成
        /// Texture2Dのコンストラクタと引数は一緒
        /// フリップ用のテクスチャは２つ作られる
        /// </summary>
        public FlipTexture2D( GraphicsDevice graphicsDeivce, int width, int height,
                    bool mipMap, SurfaceFormat format )
            : this( graphicsDeivce, width, height, mipMap, format, 2 )
        {
        }

        /// <summary>
        /// 使用するテクスチャを切り替え
        /// </summary>
        public void Flip()
        {
            if ( ++index >= textures.Length )
                index = 0;
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
         public void Dispose()
        {
            for ( int i = 0; i < textures.Length; ++i )
            {
                if ( textures[i] != null )
                {
                    textures[i].Dispose();
                    textures[i] = null;
                }
            }
        }

    }
}
