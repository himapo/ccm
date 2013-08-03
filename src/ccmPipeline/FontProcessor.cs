using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
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
    [ContentProcessor(DisplayName = "非ASCII用フォントプロセッサ")]
    public class FontProcessor : FontDescriptionProcessor
    {
        [DefaultValue("FontCharacters.txt")]
        [DisplayName("Message File")]
        [Description("The characters in this file will be automatically added to the font.")]
        public string MessageFile
        {
            get { return messageFile; }
            set { messageFile = value; }
        }
        private string messageFile = "FontCharacters.txt";

        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            var fullPath = Path.GetFullPath(MessageFile);

            context.AddDependency(fullPath);

            var letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);

            foreach (char c in letters)
            {
                if (c == '\r' || c == '\n')
                {
                    continue;
                }
                input.Characters.Add(c);
            }

            return base.Process(input, context);
        }
    }
}
