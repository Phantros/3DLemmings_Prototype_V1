using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bunny : MonoBehaviour
{
    private StateMachine _stateMachine;

    private GameObject currentCrate;

    public Rigidbody rb;

    public BunnySpawner bunnySpawner;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    private Color color;

    [HideInInspector]
    public GameObject currentJobModel;

    //Inspector variables
    [Header("Bunnies")]
    public float speed = 0.003f;
    public bool goingForward = true;
    public float jumpForce = 45;

    //Inspector objects
    [Header("Feedback Objects")]
    public TextMeshProUGUI bombCounter;
    public Pickaxe aPickaxeObjbase;
    public BouncePad aBouncePadObjbase;
    public Arrow anArrowObjbase;

    //Variables only used in this class
    private const int MAX_CRATE_DESTROY_COUNTDOWN = 2100;
    private const float MAX_SPEED = 0.006f;
    private string walkDirection = "";
    private bool startBombCountdown = false;
    private float bombCountdown = 5;

    //Variables used outside of this class, but not needed in inspector
    [HideInInspector]
    public string newJob;
    [HideInInspector]
    public bool turnAround = false;
    [HideInInspector]
    public bool hittingCrate = false;
    [HideInInspector]
    public bool pickaxeState = false;
    [HideInInspector]
    public int crateDestroyCountdown = 0;

    //initialize a bunny with a position and no starting job 
    public void Init(Vector3 startPosition)
    {
        this.transform.position = startPosition;
        newJob = null;
    }

    //Set a reference to the necessary components
    //State machine switches with At() function
    private void Awake()
    {
        _stateMachine = new StateMachine();

        rb = GetComponent<Rigidbody>();

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        color = skinnedMeshRenderer.material.color;

        //The different behaviours (jobs) the bunnies can do
        var walkBehviour = new WalkBehaviour(this);
        var pickaxeBehaviour = new PickaxeBehaviour(this);
        var bombBehaviour = new BombBehaviour(this);
        var bounceBehaviour = new BounceBehaviour(this);
        var turnAroundBehaviour = new TurnAroundBehaviour(this);
        var goRightBehaviour = new GoRightBehaviour(this);
        var goLeftBehaviour = new GoLeftBehaviour(this);

        //Using the At() function to:
        //set what state to transition from, 
        //what state to transition to,
        //based on a bool function
        At(walkBehviour, bombBehaviour, TimeToDie());
        At(walkBehviour, pickaxeBehaviour, TimeToHackAway());
        At(pickaxeBehaviour, bombBehaviour, TimeToDie());
        At(walkBehviour, bounceBehaviour, TimeToJump());
        At(bounceBehaviour, bombBehaviour, TimeToDie());
        At(walkBehviour, turnAroundBehaviour, TimeToTurnThemAround());
        At(turnAroundBehaviour, bombBehaviour, TimeToDie());
        At(walkBehviour, goRightBehaviour, TimeToGoRight());
        At(goRightBehaviour, bombBehaviour, TimeToDie());
        At(walkBehviour, goLeftBehaviour, TimeToGoLeft());
        At(goLeftBehaviour, bombBehaviour, TimeToDie());

        //Starting state is the normal walking behaviour. 
        _stateMachine.SetState(walkBehviour);

        //The function that handles the statemachine transitions
        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        //A set of bool functions that are used to decide if the statemachine should transition
        Func<bool> TimeToHackAway() => () => newJob == "Pickaxe";
        Func<bool> TimeToDie() => () => newJob == "Bomb";
        Func<bool> TimeToJump() => () => newJob == "Bounce";
        Func<bool> TimeToTurnThemAround() => () => newJob == "TurnAround";
        Func<bool> TimeToGoRight() => () => newJob == "GoRight";
        Func<bool> TimeToGoLeft() => () => newJob == "GoLeft";
    }

    private void Update()
    {
        _stateMachine.Tick();

        CalculateMovementDirection();

        //When the bunny reaches a destroyable crate, a countdown starts to destroy the crate and the pickaxe model above the bunny in the scene. 
        if (hittingCrate)
        {
            crateDestroyCountdown++;
            if (crateDestroyCountdown >= MAX_CRATE_DESTROY_COUNTDOWN)
            {
                Destroy(currentCrate);
                Destroy(currentJobModel);
                crateDestroyCountdown = 0;
                crateDestroyCountdown = 0;
            }
        }

        //when the bunny gets destroyed, a counter appears over the bunny, facing the screen.
        if (startBombCountdown)
        {
            bombCountdown -= 1 * Time.deltaTime;

            bombCounter.text = ((int)bombCountdown).ToString();

            bombCounter.transform.LookAt(this.transform.position + Camera.main.transform.forward);

        }
    }

    //Turns the bunny around, and sets the bool back. 
    private void FixedUpdate()
    {
        if (turnAround)
        {
            speed = -speed;
            turnAround = false;
        }
    }

    //To get what direction the bunny is walking in
    private void CalculateMovementDirection()
    {
        if (goingForward && speed > 0)
        {
            walkDirection = "North";
        }

        if (goingForward && speed < 0)
        {
            walkDirection = "South";
        }

        if (!goingForward && speed > 0)
        {
            walkDirection = "East";
        }

        if (!goingForward && speed < 0)
        {
            walkDirection = "West";
        }
    }

    //Either going forward, or side to side
    public void Move()
    {
        if (goingForward)
        {
            rb.transform.position += new Vector3(0, 0, speed);
        }
        else if (!goingForward)
        {
            rb.transform.position += new Vector3(speed, 0, 0);
        }
    }

    //Sets the bunny color to blue, for visual confirmation
    private void OnMouseOver()
    {
        skinnedMeshRenderer.material.color = Color.blue;
    }

    //Sets the bunny color back to the original color
    private void OnMouseExit()
    {
        skinnedMeshRenderer.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(currentJobModel);
            Destroy(this.gameObject);
        }
    }

    //Logic for when the bunny hits different objects or bunnies with jobs. 
    private void OnCollisionEnter(Collision collision)
    {
        //To make the bunny turn around when touching a crate, unless the pickaxe job is active. 
        if (collision.gameObject.CompareTag("Crate"))
        {
            if (!pickaxeState)
            {
                this.transform.Rotate(0, 180, 0);
                turnAround = true;
            }
            else if (pickaxeState)
            {
                hittingCrate = true;
                currentCrate = collision.gameObject;
            }
        }

        if (collision.gameObject.CompareTag("Bounce"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (collision.collider.CompareTag("Death"))
        {
            Destroy(currentJobModel);
            Destroy(this.gameObject);
        }

        if (collision.collider.CompareTag("Wall"))
        {
            this.transform.Rotate(0, 180, 0);
            turnAround = true;
        }

        if (collision.collider.CompareTag("GoRight"))
        {
            this.transform.Rotate(0, 90, 0);

            if (goingForward)
            {
                goingForward = false;
            }
            else if (!goingForward)
            {
                goingForward = true;
                turnAround = true;
            }
        }

        if (collision.collider.CompareTag("GoLeft"))
        {
            this.transform.Rotate(0, -90, 0);

            if (goingForward)
            {
                turnAround = true;
                goingForward = false;
            }
            else if (!goingForward)
            {
                goingForward = true;
            }
        }
    }

    //Logic for when the speed up element is used. 
    public void SpeedUp()
    {
        if (speed >= 0)
        {
            speed = MAX_SPEED;
        }
        else if (speed < 0)
        {
            speed = -MAX_SPEED;
        }
    }

    public void InitPickaxe()
    {
        Pickaxe aPickaxe = Instantiate(aPickaxeObjbase);
        aPickaxe.Init(new Vector3(
            this.transform.position.x,
            this.transform.position.y + 1,
            this.transform.position.z));

        if (walkDirection == "East" || walkDirection == "West")
        {
            aPickaxe.transform.Rotate(new Vector3(0, 0, 90));
        }

        currentJobModel = aPickaxe.gameObject;
    }

    public void InitArrowRight()
    {
        Arrow anArrow = Instantiate(anArrowObjbase);

        anArrow.Init(new Vector3(
            this.transform.position.x,
            this.transform.position.y + 1,
            this.transform.position.z));

        switch (walkDirection)
        {
            case "North":
                break;

            case "South":
                anArrow.transform.Rotate(new Vector3(0, 0, 180));
                break;

            case "East":
                anArrow.transform.Rotate(new Vector3(0, 0, 90));
                break;

            case "West":
                anArrow.transform.Rotate(new Vector3(0, 0, 270));
                break;
        }

        currentJobModel = anArrow.gameObject;
    }

    public void InitArrowLeft()
    {
        Arrow anArrow = Instantiate(anArrowObjbase);

        anArrow.Init(new Vector3(
            this.transform.position.x,
            this.transform.position.y + 1,
            this.transform.position.z));

        switch (walkDirection)
        {
            case "North":
                anArrow.transform.Rotate(new Vector3(0, 0, 180));
                break;

            case "South":
                break;

            case "East":
                anArrow.transform.Rotate(new Vector3(0, 0, 270));
                break;

            case "West":
                anArrow.transform.Rotate(new Vector3(0, 0, 90));
                break;
        }

        currentJobModel = anArrow.gameObject;
    }

    public void InitArrowTurnAround()
    {
        Arrow anArrow = Instantiate(anArrowObjbase);

        anArrow.Init(new Vector3(
            this.transform.position.x,
            this.transform.position.y + 1,
            this.transform.position.z));

        switch (walkDirection)
        {
            case "North":
                anArrow.transform.Rotate(new Vector3(0, 0, 90));
                break;

            case "South":
                anArrow.transform.Rotate(new Vector3(0, 0, 270));
                break;

            case "East":
                anArrow.transform.Rotate(new Vector3(0, 0, 180));
                break;

            case "West":
                break;
        }

        currentJobModel = anArrow.gameObject;
    }

    public void InitBouncePad()
    {
        BouncePad aBouncePad = Instantiate(aBouncePadObjbase);

        aBouncePad.Init(new Vector3(
            this.transform.position.x,
            this.transform.position.y - 0.4f,
            this.transform.position.z));

        currentJobModel = aBouncePad.gameObject;
    }

    public void InitBombCountDown()
    {
        startBombCountdown = true;
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject, 5f);
        Destroy(currentJobModel);
    }
}
