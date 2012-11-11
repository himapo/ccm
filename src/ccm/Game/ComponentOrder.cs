
namespace ccm
{
    enum UpdateOrderLabel
    {
        DEBUG_FONT = -10,

        RESOURCE = -2,  // リソースは使用するクラスより先に読み込む
        INPUT = -1, // 入力を最初に更新

        DEFAULT = 0,

        UPDATER,

        // キャラ、オブジェクト
        ALLY,
        ENEMY,
        MAP,
        PLAYER,
        ITEM,
        DECO,
        PARTICLE,
        UI,

        COLLISION,  // コリジョンはキャラの位置が確定してから

        CAMERA, // カメラはキャラに依存

        SCENE,  // MMDXを使うシーンはカメラ依存
        STAGE,

        RENDER, // 描画はキャラとカメラに依存するので最後
    }

    enum DrawOrderLabel
    {
        DEFAULT = 0,

        ALLY,
        ENEMY,
        MAP,
        PLAYER,

        EFFECT,

        RENDER,

        RENDER_MAP_CUBE,
        RENDER_CUBE_BASIC,
        RENDER_CUBE,
        RENDER_MIKU,
        RENDER_BILLBOARD,   // パーティクルなど

        COLLISION,

        RENDER_UI,  // ここで深度バッファがクリアされる
        
        DEBUG_UI,
        DEBUG_FONT, // デバッグフォントは最前面に書く
    }
}
