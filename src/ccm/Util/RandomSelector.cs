using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    /// <summary>
    /// 指定した確率分布でランダムに選択する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class RandomSelector<T>
    {
        /// <summary>
        /// 確率境界値と対象オブジェクトの辞書
        /// </summary>
        Dictionary<int, T> BorderDic { get; set; }

        int totalProbability;

        IRand rand;

        public RandomSelector(IRand rand)
        {
            BorderDic = new Dictionary<int, T>();
            totalProbability = 0;
            this.rand = rand;
        }

        public void Add(int probability, T obj)
        {
            if (probability <= 0)
                return;

            totalProbability += probability;

            BorderDic[totalProbability] = obj;
        }

        public void Clear()
        {
            BorderDic.Clear();
            totalProbability = 0;
        }

        public T Get()
        {
            var sample = rand.Next(totalProbability);

            foreach (var border in BorderDic.Keys)
            {
                if (sample < border)
                {
                    return BorderDic[border];
                }
            }

            return default(T);
        }
    }
}
