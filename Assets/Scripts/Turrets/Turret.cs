using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected GameManager manager;

    public void Init(GameManager manager)
    {
        this.manager = manager;
    }
}
