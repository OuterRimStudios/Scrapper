using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.UI.ControlMapper;

public class FindInputManager : MonoBehaviour
{
    ControlMapper controlMapper;
    Rewired.InputManager inputManager;

    private void Start()
    {
        controlMapper = GetComponent<ControlMapper>();
        inputManager = GameObject.Find("Rewired Input Manager").GetComponent<Rewired.InputManager>();
        controlMapper.rewiredInputManager = inputManager;
    }
}
