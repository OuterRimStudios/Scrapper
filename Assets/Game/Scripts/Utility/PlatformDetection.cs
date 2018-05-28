using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class PlatformDetection : MonoBehaviour {

    public static bool useGamepad;

    GameObject prevSelectedObj;
    EventSystem es;
    Player player;

    private void Awake()
    {
        es = EventSystem.current;
        player = ReInput.players.GetPlayer(0);
    }

    void OnEnable()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    void OnDisable()
    {
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        es.sendNavigationEvents = true;
        useGamepad = true;
    }

    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        es.sendNavigationEvents = false;
        useGamepad = false;
    }

    void Update()
    {
        if(player.controllers.Keyboard.GetAnyButton() || player.controllers.Mouse.GetAnyButton() || player.controllers.Mouse.GetAxis(0) > 0 || player.controllers.Mouse.GetAxis(1) > 0)
        {
            useGamepad = false;
            es.sendNavigationEvents = false;
        }
        else if(player.controllers.GetLastActiveController(ControllerType.Joystick) != null && player.controllers.GetLastActiveController(ControllerType.Joystick).GetAnyButton() || player.GetAxis("LeftStickX") > 0 || player.GetAxis("LeftStickY") > 0 || player.GetAxis("RightStickX") > 0|| player.GetAxis("RightStickY") > 0)
        {
            useGamepad = true;
            es.sendNavigationEvents = true;
        }
    }
}