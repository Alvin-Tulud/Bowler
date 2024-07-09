using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinInfo : MonoBehaviour
{
    private pinPosition pinPos;

    // Start is called before the first frame update
    void Awake()
    {
        pinPos.pinNum = (pinNumber) int.Parse(transform.name.Substring(name.Length - 2));
        pinPos.pinPos = transform.position;
        pinPos.pinRot = transform.rotation;
        pinPos.isHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pinPos.pinPos != transform.position)
        {
            pinPos.pinPos = transform.position;
            pinPos.pinRot = transform.rotation;
        }
    }

    public pinPosition getPinPos()
    {
        return pinPos;
    }

    public void setPinPos(pinPosition pin)
    {
        pinPos = pin;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == gameObject.layer ||
            collision.gameObject.layer == 6)
        {
            pinPos.isHit = true;
        }
    }
}
