using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public int playerNum = 0;
    [HideInInspector]
    public KeyCode forward, backward, left, right, jump, fire, taunt, interact;

    private void Awake()
    {
        DetermineInputs();
    }

    public void DetermineInputs()
    {
        switch(playerNum)
        {
            case 1:
                forward = KeyCode.W;
                backward = KeyCode.S;
                left = KeyCode.A;
                right = KeyCode.D;
                jump = KeyCode.Space;
                interact = KeyCode.E;
                fire = KeyCode.Mouse0;
                break;
            case 2:
                forward = KeyCode.Keypad8;
                backward = KeyCode.Keypad2;
                left = KeyCode.Keypad4;
                right = KeyCode.Keypad6;
                jump = KeyCode.Keypad0;
                interact = KeyCode.Keypad9;
                fire = KeyCode.Keypad7;
                break;
        }
    }
}
