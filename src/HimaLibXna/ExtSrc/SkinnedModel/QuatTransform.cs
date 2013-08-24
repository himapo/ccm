#region File Description
//-----------------------------------------------------------------------------
// QuatTransform.cs
//-----------------------------------------------------------------------------
#endregion
#region Using ステートメント

using System;
using Microsoft.Xna.Framework;

#endregion

namespace SkinnedModel
{
    /// <summary>
    /// クォータニオンを使った回転と平行移動を表すことのできる頂点変換用構造体
    /// 通常の行列の半分以下のメモリ使用量になる
    /// </summary>
    public struct QuatTransform
    {
        #region フィールド

        // 回転
        public Quaternion   Rotation;

        // 平行移動
        public Vector3      Translation;

        #endregion

        #region 初期化

        /// <summary>
        /// クォータニオンと平行移動を指定して生成する
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="translation"></param>
        public QuatTransform(Quaternion rotation, Vector3 translation)
        {
            Rotation = rotation;
            Translation = translation;
        }

        /// <summary>
        /// 指定された行列から生成する
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static QuatTransform FromMatrix( Matrix matrix )
        {
            // 行列の分解
            Quaternion rotation;
            Vector3 translation;
            Vector3 scale;
            matrix.Decompose( out scale, out rotation, out translation );

            // 一意のスケールか？
            if ( !CloseEnough( scale.X, scale.Y ) || !CloseEnough( scale.X, scale.Z ) )
            {
                throw new InvalidOperationException(
                    "一意のスケール(X,Y,Zが同じスケール値)ではありません" );
            }

            if ( !CloseEnough( scale.X, 1.0f ) )
                throw new InvalidOperationException( "スケール値が1以外です" );

            return new QuatTransform( rotation, translation );
        }

        static bool CloseEnough( float a, float b )
        {
            return ( Math.Abs( a - b ) < 1e-4f );
        }

        #endregion

        /// <summary>
        /// QuatTransformの結合
        /// </summary>
        public static QuatTransform operator *(QuatTransform value1, QuatTransform value2)
        {
            // 平行移動の算出
            Vector3 newTranslation;
            Vector3.Transform(ref value1.Translation, ref value2.Rotation,
                                out newTranslation);

            newTranslation.X += value2.Translation.X;
            newTranslation.Y += value2.Translation.Y;
            newTranslation.Z += value2.Translation.Z;

            // 回転部分の結合
            QuatTransform result;
            Quaternion.Concatenate(ref value1.Rotation, ref value2.Rotation,
                                        out result.Rotation);

            result.Translation = newTranslation;

            return result;
        }

        public Matrix ToMatrix()
        {
            return Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateTranslation(Translation);
        }
    }
}
