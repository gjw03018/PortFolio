using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

using Variable;

public class UIEventManager : MonoBehaviour
{
    public void TestButton()
    {
        Debug.Log("Click Button");
    }

    #region - 씬 전환 -
    //StartScene -> MainGameScene
    public void ClickStartScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    //MainGameScene -> PlayGameScene
    public void ClickGameScene()
    {
        SceneManager.LoadScene("PlayGameScene");
    }
    #endregion

    #region - 설정 -
    public AudioMixer audioMixer;
    public void SetBackGroundVolume(float volume)
    {
        audioMixer.SetFloat("BGVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetQuality(int qulityIndex)
    {
        QualitySettings.SetQualityLevel(qulityIndex);
    }

    #endregion

    #region - 메일 -
    #endregion

    #region - 시작 -
    #endregion

    #region - 상점 -
    public void ClickBuyButton(GoodsType type, int count)
    {
        /*
         * 보유 금액 차감
         */
        Debug.Log("Buy Item" +
            "" +
            "");
    }
    #endregion

    #region - 캐릭터 -
    #endregion

    #region - 프로필 -
    #endregion

}
