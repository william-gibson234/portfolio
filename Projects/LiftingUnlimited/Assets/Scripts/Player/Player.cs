using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class to control the Player, which is what the user controls
//Implements the interface IDataPersistence in order to load and save Game Data, such as totals, awards, and others
public class Player : MonoBehaviour, IDataPersistence
{
    public event EventHandler<OnEnergyChangeEventArgs> OnEnergyChange;
    public class OnEnergyChangeEventArgs
    {
        public float energyNormalized;
    }

    public event EventHandler<OnInjuryChangeEventArgs> OnInjuryChange;
    public class OnInjuryChangeEventArgs
    {
        public bool isPlayerInjured;
    }
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float MOVE_SPEED = 10f;
    [SerializeField] private PlayerVisualManager playerVisualManager;
    [SerializeField] private InjuryManager injuryManager;
    [SerializeField] private CompetitionManager cM;

    [SerializeField] private CompetitionSymbol cS;

    [SerializeField] private Transform hatHoldPointVisual;

    private const float ROTATE_SPEED = 10f;
    private const float PLAYER_RADIUS = 1f;
    private const float PLAYER_HEIGHT = 2f;
    private const float PLAYER_INTERACT_DISTANCE = 5f;

    private float benchProgression;
    private float deadliftProgression;
    private float squatProgression;

    private float energy = 1;

    private float ENERGY_CONST = 0.01f;

    private int currDeadliftCount = 0;
    private int currBenchCount = 0;
    private int currSquatCount = 0;

    private bool isWalking;
    private bool isLifting;
    private bool isInjured;
    private bool isCompeting;
    private bool windowOpen;

    private InteractableObject selectedObject;

    private Vector3 lastMoveDir;

    //Functions to load and save data when the game is opened/closed
    public void LoadData(GameData data)
    {
        this.isInjured = data.injuryStatus;
        this.benchProgression = data.benchProgression;
        this.squatProgression = data.squatProgression;
        this.deadliftProgression = data.deadliftProgression;
    }
    public void SaveData(ref GameData data)
    {
        data.injuryStatus = this.isInjured;
        data.benchProgression = this.benchProgression;
        data.squatProgression = this.squatProgression;
        data.deadliftProgression = this.deadliftProgression;
    }
    private void Awake()
    {
        gameInput.OnInteractAndEnterAction += GameInput_OnInteractAndEnterAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

        cM.OnCompetitionFinish += CompetitionManager_OnCompetitionFinish;

        Equipment.PlayerCheckInjury += Equipment_PlayerCheckInjury;

        cS.OnCompetitionStart += CompetitionSymbol_OnCompetitionStart;


        Equipment.OnAnyLiftComplete += Equipment_OnAnyLiftComplete;


        isWalking = false;
        isLifting = false;
        isCompeting = false;
        windowOpen = false;

    }

    private void Equipment_OnAnyLiftComplete(object sender, Equipment.OnAnyLiftCompleteEventArgs e)
    {
        if(!isCompeting) {
            ShowHat();
        }
    }
    private void CompetitionManager_OnCompetitionFinish(object sender, CompetitionManager.OnCompetitionFinishEventArgs e)
    {
        isCompeting = false;
        ShowHat();
    }
    private void CompetitionSymbol_OnCompetitionStart(object sender, CompetitionSymbol.OnCompetitionStartEventArgs e)
    {
        isCompeting = true;
        HideHat();
    }

    //Function to display the correct hat
    public void SetHat(GameObject hatPrefab)
    {
        for(int i = 0; i < hatHoldPointVisual.childCount; i++)
        {
            GameObject.Destroy(hatHoldPointVisual.GetChild(i).gameObject);
        }

        Transform newHatHoldPointVisualTransform = Instantiate(hatPrefab.transform, hatHoldPointVisual);
    }
    private void Equipment_PlayerCheckInjury(object sender, EventArgs e)
    {
        if (injuryManager.GetInjuryRisk() > 98f)
        {
            SetInjuryStatus(true);
        }
    }

    //Event Listener to trigger whenever anything is interacted with
    //Checks to make sure Player has energy to lift and that Player is not hurt
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedObject != null&&energy>0)
        {
            if (selectedObject is Equipment)
            {
                if (!isInjured)
                {

                    selectedObject.Interact(this);
                    HideHat();
                    LiftingGameManager.isLifting = true;
                }
            }
            else
            {
                selectedObject.Interact(this);
            }
        }
        if(selectedObject is ProteinCounter&&energy==0)
        {
            selectedObject.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedObject != null && !isInjured && selectedObject is Equipment)
        {
            Equipment selectedEquipment = (Equipment)selectedObject;
            selectedEquipment.InteractAlternate(this);
        }
    }
    private void GameInput_OnInteractAndEnterAction(object sender, EventArgs e)
    {
        if (selectedObject != null && energy > 0)
        {
            if (selectedObject is Equipment)
            {
                if (!isInjured)
                {

                    selectedObject.InteractAndEnter(this);
                    HideHat();
                    LiftingGameManager.isLifting = true;
                }
            }
            else
            {
                selectedObject.InteractAndEnter(this);
            }
        }

        if (selectedObject is ProteinCounter && energy == 0)
        {
            selectedObject.InteractAndEnter(this);
        }
    }

    private void Update()
    {
        playerVisualManager.UpdateVisual(isLifting);
        Movement();

        //Code to restrict Player from leaving gym area
        if(transform.position.z < -50f)
        { 
            transform.position = new Vector3(transform.position.x, transform.position.y, -50f);
        }
        Interactions();
            
        if (isLifting && !isInjured && !isCompeting&&!LiftingGameManager.isWindowOpen)
        {
            if (energy > 0)
            { 
                    //Decrements energy consistently
                energy -= Time.deltaTime * ENERGY_CONST;
                OnEnergyChange?.Invoke(this, new OnEnergyChangeEventArgs
                {
                    energyNormalized = energy
                });
            }
            else
            {
                if (energy < 0)
                {
                    energy = 0f;
                }
            }
        }
    }
    private void HideHat()
    {
        if (hatHoldPointVisual.childCount > 0)
        {
            hatHoldPointVisual.GetChild(0).gameObject.SetActive(false);
        }
    }
    public Vector3 GetMoveDir()
    {
        return lastMoveDir;
    }
    private void ShowHat()
    {
        if (hatHoldPointVisual == null)
        {
            Debug.LogError("hat hold point null");
        }
        else
        {
            if (hatHoldPointVisual.childCount > 0)
            {
                hatHoldPointVisual.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    //Function hanldes movement, ensures Player is not lifting, competing, has a UI window open, or had something in front of them
    private void Movement()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = MOVE_SPEED * Time.deltaTime;

        //Utilizes a Raycast to see if there is anything in front of the Player
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, moveDir, moveDistance);

        if (!LiftingGameManager.isCompeting && !LiftingGameManager.isLifting && !LiftingGameManager.isWindowOpen)
        {
            if (canMove)
            {
                transform.position += moveDir * moveDistance;
            }
            //Changes player direction based on where they are moving
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * ROTATE_SPEED);
            isWalking = moveDir != Vector3.zero;
        }
        else
        {
            isWalking = false;
        }
    }

    //Function to handle the interactions with the player
    private void Interactions()
    {
        if (!isCompeting)
        {
            //Finds the Player's direction
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDir != Vector3.zero)
            {
                lastMoveDir = moveDir;
            }

            if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, PLAYER_INTERACT_DISTANCE))
            {
                //Tries to cast the object in front of it to an interactableObject, meaning the Player can interact with it
                if (raycastHit.transform.TryGetComponent(out InteractableObject interactableObject))
                {
                    if (selectedObject != interactableObject)
                    {
                        selectedObject = interactableObject;
                    }
                }
                else
                {
                    if (!isLifting)
                    {
                        selectedObject = null;
                    }
                }
            }
            else
            {
                if (!isLifting)
                {
                    selectedObject = null;
                }
            }
        }
    }

    //Various setter, getter, and increment functions for different variables
    public void SetIsLifting(bool IsLifting)
    {
        isLifting = IsLifting;
    }
    public bool IsLifting()
    {
        return isLifting;
    }
    public void SetIsWalking(bool IsWalking)
    {
        isWalking = IsWalking;
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    public float GetBenchProgression()
    {
        return benchProgression;
    }
    public void SetBenchProgression(float bp)
    {
        benchProgression = bp;
    }

    public float GetSquatProgression()
    {
        return squatProgression;
    }
    public void SetSquatProgression(float sp)
    {
        squatProgression = sp;
    }

    public float GetDeadliftProgression()
    {
        return deadliftProgression;
    }
    public void SetDeadliftProgression(float dlp)
    {
        deadliftProgression = dlp;
    }

    public void SetDeadliftCount(int dc)
    {
        currDeadliftCount = dc;
    }
    public int GetDeadliftCount()
    {
        return currDeadliftCount;
    }
    public void IncrementDeadliftCount()
    {
        currDeadliftCount++;
    }

    public void SetBenchCount(int bc)
    {
        currBenchCount = bc;
    }
    public int GetBenchCount()
    {
        return currBenchCount;
    }
    public void IncrementBenchCount()
    {
        currBenchCount++;
    }

    public void SetSquatCount(int sc)
    {
        currSquatCount = sc;
    }
    public int GetSquatCount()
    {
        return currSquatCount;
    }
    public void IncrementSquatCount()
    {
        currSquatCount++;
    }
    public bool GetInjuryStatus()
    {
        return isInjured;
    }
    public void SetInjuryStatus(bool iI)
    {
        isInjured = iI;
        OnInjuryChange?.Invoke(this, new OnInjuryChangeEventArgs
        {
            isPlayerInjured = iI
        });
    }

    public void SetIsCompeting(bool iC)
    {
        isCompeting = iC;
    }
    public bool GetIsCompeting()
    {
        return isCompeting;
    }

    public float GetEnergy()
    {
        return energy;
    }
    public void SetEnergyToOne()
    {
        energy = 1;
    }

    public void SetSelectedObject(Equipment e)
    {
        selectedObject = e;
    }
    public void SetWindowOpen(bool isOpen)
    {
        windowOpen = isOpen;
    }
}
