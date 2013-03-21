using System;
using System.Collections.Generic;

namespace ccm.Util
{
    /// <summary>
    /// ゲームと無関係な汎用ユーティリティ
    /// </summary>
    static class GeneralUtil
    {
        // Listの要素入れ替え
        public static void Swap<T>(List<T> list, int x, int y)
        {
            T temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }

        public static List<int> Range(int start, int end)
        {
            var result = new List<int>();
            for (var i = start; i < end; ++i)
            {
                result.Add(i);
            }
            return result;
        }

        public static List<int> Range(int count)
        {
            return Range(0, count);
        }
    }
}
