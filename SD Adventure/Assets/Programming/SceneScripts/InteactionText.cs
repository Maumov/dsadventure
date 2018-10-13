using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InteactionText : InteractionObject
{
    public ConversationData WarnigText;

    public override void Action()
    {
        base.Action();
        MobileControlRig.Hide();
        ConversationUI.ShowText(WarnigText, MobileControlRig.Show);
    }
}
