using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBlock : Block {

	// Because the IBlock doesnt have a middle block to rotate around we have to arrange the children differently and compensate for their offset here.
	protected override void Start () {
        base.Start();
        transform.Translate((Vector2.left+Vector2.up) / 2);
    }
}
