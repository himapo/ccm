using System;
using System.Collections.Generic;
using System.Linq;

namespace HimaLib.Updater
{
    public class UpdaterManager : IUpdaterManager
    {
        // グローバルインスタンスもあるけど個別にインスタンス化もできる
        static readonly UpdaterManager instance = new UpdaterManager();

        public static UpdaterManager Instance { get { return instance; } private set { } }

        List<IUpdater> updaterList = new List<IUpdater>();

        public UpdaterManager()
        {
        }

        public void Update(float elapsedMilliSeconds)
        {
            updaterList.ForEach(x => x.Update(elapsedMilliSeconds));

            updaterList.RemoveAll(x => x.Finish);
        }

        public void Add(IUpdater updater)
        {
            updaterList.Add(updater);
        }
    }
}
