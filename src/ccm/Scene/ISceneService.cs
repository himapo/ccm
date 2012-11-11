
namespace ccm
{
    public enum SceneLabel
    {
        NULL_SCENE,

        TITLE_SCENE,
        GAME_SCENE,

        BOOT_SCENE,
        MODEL_VIEWER,
        MAP_VIEWER,
    }

    interface ISceneService
    {
        void ChangeScene(SceneLabel label);
    }
}
