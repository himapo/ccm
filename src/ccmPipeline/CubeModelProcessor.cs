using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ccmPipeline
{
    /// <summary>
    /// このクラスは XNA Framework コンテンツ パイプラインによってインスタンス化され、
    /// 任意のコンテンツ データに対してカスタムな処理を行い、TInput 型を TOutput 型 に
    /// 変換します。データの型を変更せずにカスタム処理を行う場合は、入力と出力の型が
    /// 同じになることもあります。
    ///
    /// このクラスはコンテンツ パイプライン拡張ライブラリのプロジェクトに含めてください。
    ///
    /// TODO: ContentProcessor 属性を変更して、このプロセッサの正しい表示名を
    /// 指定します。
    /// </summary>
    [ContentProcessor(DisplayName = "CubeModelProcessor")]
    public class CubeModelProcessor : ModelProcessor
    {
        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            var myMaterial = new EffectMaterialContent();

            var effectPath = Path.GetFullPath("Effect/Phong.fx");
            myMaterial.Effect = new ExternalReference<EffectContent>(effectPath);

            // マテリアル名をエフェクトに渡す（TODO: これ渡せてないなあ・・・）
            Console.WriteLine("Material name : " + material.Name);
            myMaterial.Effect.Name = material.Name;

            if (material is BasicMaterialContent)
            {
                var basicMaterial = (BasicMaterialContent)material;

                myMaterial.OpaqueData.Add("DiffuseColor", basicMaterial.DiffuseColor);
                myMaterial.OpaqueData.Add("Alpha", basicMaterial.Alpha);
                myMaterial.OpaqueData.Add("EmissiveColor", basicMaterial.EmissiveColor);
                myMaterial.OpaqueData.Add("SpecularColor", basicMaterial.SpecularColor);
                myMaterial.OpaqueData.Add("SpecularPower", 4.0f);
            }
            else if (material is EffectMaterialContent)
            {
                var effectMaterial = (EffectMaterialContent)material;
            }
            else
            {
                throw new Exception("unknown material");
            }

            return base.ConvertMaterial(myMaterial, context);
        }
    }
}
