using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class CollisionManager
    {
        static readonly CollisionManager instance = new CollisionManager();
        public static CollisionManager Instance { get { return instance; } set { } }

        List<CollisionInfo> infoList = new List<CollisionInfo>();

        // 衝突判定をもつものに対して1個発行
        int objectIDCount;
        
        List<int> availableIDList = new List<int>();
        
        Dictionary<int, Dictionary<int, int>> collisionMatrix = new Dictionary<int,Dictionary<int,int>>();

        List<KeyValuePair<int, int>> groupPairList = new List<KeyValuePair<int, int>>();

        // 衝突が起きると1個発行
        int collisionIDCount;

        CollisionDetectorFactory DetectorFactory = new CollisionDetectorFactory();

        CollisionManager()
        {
        }

        public void Add(CollisionInfo info)
        {
            // 未登録ならID発行
            if (info.ID == 0)
            {
                if (availableIDList.Count == 0)
                {
                    // 新ID発行
                    info.ID = ++objectIDCount;
                }
                else
                {
                    // IDストックから発行
                    info.ID = availableIDList[availableIDList.Count - 1];
                    availableIDList.RemoveAt(availableIDList.Count - 1);
                }
                infoList.Add(info);
                if (!collisionMatrix.ContainsKey(info.ID))
                {
                    collisionMatrix[info.ID] = new Dictionary<int, int>();
                }
            }
        }

        public void Remove(CollisionInfo info)
        {
            if (infoList.Remove(info))
            {
                // ヒット回数をリセット
                ResetCollisionCount(info.ID);
                // IDをストックに返す
                availableIDList.Add(info.ID);
                info.ID = 0;
            }
        }

        public void ResetCollisionCount(int ID)
        {
            if (ID > 0)
            {
                // TODO : コスト高そう
                collisionMatrix[ID].ToList().ForEach(pair => collisionMatrix[ID][pair.Key] = 0);
            }
        }

        public void Detect()
        {
            collisionIDCount = 0;

            foreach (var groupPair in groupPairList)
            {
                Detect(groupPair.Key, groupPair.Value);
            }
        }

        public void Draw(ICollisionDrawer drawer)
        {
            var activeQuery = GetActiveQuery();

            foreach (var info in activeQuery)
            {
                foreach (var primitive in info.Primitives)
                {
                    primitive.Draw(drawer, true);
                }
            }
        }

        void Detect(int alpha, int beta)
        {
            // 衝突判定が有効なものだけ取り出すクエリ
            var activeQuery = GetActiveQuery();

            var alphaQuery = GetActiveGroupQuery(activeQuery, alpha);

            var betaQuery = GetActiveGroupQuery(activeQuery, beta);

            foreach (var a in alphaQuery)
            {
                foreach (var b in betaQuery)
                {
                    if (Detect(a, b))
                    {
                        var collisionCount = UpdateCollisionMatrix(a, b);
                        var collisionID = ++collisionIDCount;
                        a.Reaction(collisionID, collisionCount);
                        b.Reaction(collisionID, collisionCount);
                    }
                }
            }
        }

        IEnumerable<CollisionInfo> GetActiveQuery()
        {
            var activeQuery =
                from info in infoList
                where info.Active()
                select info;
            return activeQuery;
        }

        IEnumerable<CollisionInfo> GetActiveGroupQuery(IEnumerable<CollisionInfo> activeQuery, int groupID)
        {
            var groupQuery =
                from active in activeQuery
                where active.Group() == groupID
                select active;
            return groupQuery;
        }

        bool Detect(CollisionInfo a, CollisionInfo b)
        {
            foreach (var primitiveA in a.Primitives)
            {
                foreach (var primitiveB in b.Primitives)
                {
                    if (DetectorFactory.Create(primitiveA, primitiveB).Detect())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        int UpdateCollisionMatrix(CollisionInfo a, CollisionInfo b)
        {
            if (!collisionMatrix[a.ID].ContainsKey(b.ID))
            {
                collisionMatrix[a.ID][b.ID] = 0;
            }
            if (!collisionMatrix[b.ID].ContainsKey(a.ID))
            {
                collisionMatrix[b.ID][a.ID] = 0;
            }

            var ab = ++collisionMatrix[a.ID][b.ID];
            var ba = ++collisionMatrix[b.ID][a.ID];

            if (ab < ba)
            {
                collisionMatrix[b.ID][a.ID] = ba = ab;
            }
            if (ba < ab)
            {
                collisionMatrix[a.ID][b.ID] = ab = ba;
            }

            return ab;
        }
    }
}
