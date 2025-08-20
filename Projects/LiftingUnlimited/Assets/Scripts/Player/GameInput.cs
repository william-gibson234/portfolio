using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to handle the input from the PlayerInputActions input controller
public class GameInput : MonoBehaviour
{
    [SerializeField] private PauseScreenUI pSUI;

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAndEnterAction;
    public event EventHandler OnInteractAlternateAction;
    
    public event EventHandler OnEnterTextAction;

    public event EventHandler OnShopOpenAction;

    public event EventHandler OnPauseAction;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAndEnter.performed += InteractAndEnter_performed;
        playerInputActions.Player.InteractAlt.performed += InteractAlt_performed;
        playerInputActions.Player.EnterText.performed += EnterText_performed;

        playerInputActions.Player.OpenShop.performed += OpenShop_performed;

        playerInputActions.Player.Pause.performed += Pause_performed;

        pSUI.OnMainMenuOpen += PauseScreenUI_OnMainMenuOpen;
    }

    private void PauseScreenUI_OnMainMenuOpen(object sender, EventArgs e)
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAndEnter.performed -= InteractAndEnter_performed;
        playerInputActions.Player.InteractAlt.performed -= InteractAlt_performed;
        playerInputActions.Player.EnterText.performed -= EnterText_performed;

        playerInputActions.Player.OpenShop.performed -= OpenShop_performed;

        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction.Invoke(this, EventArgs.Empty);
    }
    private void OpenShop_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShopOpenAction.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction.Invoke(this, EventArgs.Empty);
    }
    private void EnterText_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEnterTextAction.Invoke(this, EventArgs.Empty);
    }
    private void InteractAlt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction.Invoke(this, EventArgs.Empty);
    }
    private void InteractAndEnter_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAndEnterAction?.Invoke(this, EventArgs.Empty);
    }
    //Function to normalize the movement vector, so diagonal movement is not faster than orthogonal movement
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
