using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ccm
{
    public interface IUpdater
    {
        void Update(float elapsedMilliSeconds);

        /// <summary>
        /// 終了フラグ
        /// </summary>
        bool Finish { get; set; }
    }
}
