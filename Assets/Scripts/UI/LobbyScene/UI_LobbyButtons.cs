using Common;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Recorder.OutputPath;

namespace GameUI
{
    public class UI_LobbyButtons : UIWidget
    {
        [SerializeField] Button startBtn;
        [SerializeField] Button settingBtn;
        [SerializeField] Button eixtBtn;

        public override bool Init()
        {
            if (!base.Init()) return false;

            return true;
        }

        #region OnClick Event
        public void OnClickStartButton()
        {
            GameSystem.Init();

            Manager.UI.HideHUD();
            Instantiate(Resources.Load<GameObject>(nameof(CustomizingMenu)));
        }

        public void OnClickSettingButton()
        {
            //TODO:  설정창 팝업
            GameSystem.Init();
            Manager.Scene.LoadScene(Define.SceneType.TestFC_1);
        }
        public void OnClickExitButton()
        {
            Manager.UserData.SaveAllUserData();
            Debug.Log($"[{GetType()}] 게임 종료");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}