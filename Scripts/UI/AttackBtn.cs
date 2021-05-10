using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;

public class AttackBtn : MonoBehaviour
{
    public Image coolTime;
    Button btn;
    private void Start()
    {
        btn = GetComponent<Button>();
    }
    public void StartCollTime(float time) { coolTime.fillAmount = 1; StartCoroutine("CoolTime",time);  }

    //쿨타임체크
    IEnumerator CoolTime(float time=1)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //print("CoolTime : " + sw.ElapsedMilliseconds + "ms");
        float second = 0;
        while (true)
        {
            if(coolTime.fillAmount > 0)
            {
                second += 0.01f;
                coolTime.fillAmount = 1 - second/time;
                btn.enabled = false;
            }
            else
            {
                sw.Stop();
                //print("CoolTime : " + sw.ElapsedMilliseconds + "ms");
                btn.enabled = true;
                break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
