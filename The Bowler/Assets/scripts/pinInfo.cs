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
        pinPos.pinNum = int.Parse(transform.name.Substring(name.Length - 2));

        //Debug.Log(transform.name.Substring(name.Length - 2));
        //Debug.Log(int.TryParse(transform.name.Substring(name.Length - 2), out int thing) + " : " + thing);

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
        if ((collision.transform.CompareTag("Ball") || collision.transform.CompareTag("Pin")) && !pinPos.isHit)
        {
            //Debug.Log(name + ": layer hit: " + collision.gameObject.layer);

            pinPos.isHit = true;
        }
    }
}
