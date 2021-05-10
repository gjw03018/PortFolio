using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelAnimationController : MonoBehaviour
{
    PlayObjectClass pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent < PlayObjectClass > ();
    }
    public void AttackEvent(float angle)
    {
        pc.AttackEvent(angle);
    }

    
    
}
