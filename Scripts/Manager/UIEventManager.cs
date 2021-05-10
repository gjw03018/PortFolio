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

    #region - �� ��ȯ -
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

    #region - ���� -
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

    #region - ���� -
    #endregion

    #region - ���� -
    #endregion

    #region - ���� -
    public void ClickBuyButton(GoodsType type, int count)
    {
        /*
         * ���� �ݾ� ����
         */
        Debug.Log("Buy Item" +
            "" +
            "");
    }
    #endregion

    #region - ĳ���� -
    #endregion

    #region - ������ -
    #endregion

}
