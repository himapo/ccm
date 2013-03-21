using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm.Util
{
    /// <summary>
    /// ランダムに1個ずつ抜けていくくじ引き型リスト
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ShuffleList<T>
    {
        public List<T> RemainList { get; private set; }

        public List<T> DrawnList { get; private set; }

        HimaLib.Math.IRand rand;

        public int RemainCount
        {
            get { return RemainList.Count; }
        }

        public ShuffleList(HimaLib.Math.IRand rand)
        {
            RemainList = new List<T>();
            DrawnList = new List<T>();
            this.rand = rand;
        }

        public void Add(T n)
        {
            RemainList.Add(n);
        }

        public void AddRange(List<T> list)
        {
            RemainList.AddRange(list);
        }

        /// <summary>
        /// 描画ではなく、くじ引きを引く方のDraw
        /// </summary>
        /// <returns>引いたもの</returns>
        public T Draw()
        {
            var result = RemainList[rand.Next(RemainList.Count)];

            RemainList.Remove(result);
            DrawnList.Add(result);

            return result;
        }

        /// <summary>
        /// 複数回まとめて引く
        /// </summary>
        /// <param name="count">何回引くか</param>
        public void Draw(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Draw();
            }
        }

        public void Reset()
        {
            RemainList.AddRange(DrawnList);
            DrawnList.Clear();
        }
    }
}
