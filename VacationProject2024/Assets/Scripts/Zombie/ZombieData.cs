using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ZombieData : ScriptableObject
{
    public float maxHealth;
    public float maxSpeed;
    public float angularSpeed;
    public float acceleration;
    public float baseDetectRange;
    public float attackrange;
    public float possibleAttackAngle;
    public float minDamage;
    public float maxDamage;
    public float maxActivation;
    public float pursuitTime;
}
