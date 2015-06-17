using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;

public class LoLInputManager : MonoBehaviour
{
    GUIStyle style = new GUIStyle();
    List<LogMessage> logMessages = new List<LogMessage>();
    bool isPaused;
    
    void Start()
    {
        isPaused = false;
        Time.timeScale = 1.0f;

        Logger.OnLogMessage += logMessage => logMessages.Add( logMessage );

        //		InputManager.HideDevicesWithProfile( typeof( Xbox360MacProfile ) );
        //		InputManager.InvertYAxis = true;
        //		InputManager.EnableXInput = true;

        InputManager.Setup();

        InputManager.OnDeviceAttached += inputDevice => Debug.Log( "Attached: " + inputDevice.Name );
        InputManager.OnDeviceDetached += inputDevice => Debug.Log( "Detached: " + inputDevice.Name );
        InputManager.OnActiveDeviceChanged += inputDevice => Debug.Log( "Active device changed to: " + inputDevice.Name );

        InputManager.AttachDevice( new UnityInputDevice( new FPSProfile() ) );

        Debug.Log( "InControl (version " + InputManager.Version + ")" );
    }


    void FixedUpdate()
    {
        InputManager.Update();
        CheckForPauseButton();

        if ( InputManager.ActiveDevice.Action1.WasPressed )
        {
            Debug.Log( "BOOM!" );
        }
    }


    void Update()
    {
        if ( isPaused )
        {
            InputManager.Update();
            CheckForPauseButton();
        }
    }

    void CheckForPauseButton()
    {
        if ( Input.GetKeyDown( KeyCode.P ) || InputManager.MenuWasPressed )
        {
            Time.timeScale = isPaused ? 1.0f : 0.0f;
            isPaused = !isPaused;
        }
    }

}
