using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSkill : MonoBehaviour
{
    private Animator skillAnimator;

    private void Awake()
    {
        skillAnimator = GetComponent<Animator>();
    }

    public void UseChargeSkill()
    {
        Debug.Log("UseChargeSkill");
    }
}
