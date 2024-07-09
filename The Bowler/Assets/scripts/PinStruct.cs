using UnityEngine;

public enum pinNumber
{
    one = 1,
    two = 2,
    three = 3,
    four = 4,
    five = 5,
    six = 6,
    seven = 7,
    eight = 8,
    nine = 9,
    ten = 10
}

public struct pinPosition
{
    public pinNumber pinNum;
    public Vector3 pinPos;
    public Quaternion pinRot;
    public bool isHit;
}