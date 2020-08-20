using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState
{
    // Start is called before the first frame update
    public abstract void StateStart(BossController boss);

    // Update is called once per frame
    public abstract void StateUpdate(BossController boss);
}
