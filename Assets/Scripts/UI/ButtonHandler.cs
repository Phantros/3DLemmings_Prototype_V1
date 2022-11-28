using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button _bombButton;
    public Button _bounceButton;
    public Button _pickaxeButton;
    public Button _goLeftButton;
    public Button _goRightButton;
    public Button _turnAroundButton;
    public Button _speedUpButton;
    public Button _nukeButton;

    public string possibleJob;

    private void Start()
    {
        possibleJob = null;
    }

    //Get the UI button components and add an AddListener
    void Awake()
    {
        Button bombBtn = _bombButton.GetComponent<Button>();
        Button bounceBtn = _bounceButton.GetComponent<Button>();
        Button pickaxeBtn = _pickaxeButton.GetComponent<Button>();
        Button goLeftBtn = _goLeftButton.GetComponent<Button>();
        Button goRightBtn = _goRightButton.GetComponent<Button>();
        Button turnAroundBtn = _turnAroundButton.GetComponent<Button>();    
        Button speedUpBtn = _speedUpButton.GetComponent<Button>();
        Button nukeBtn = _nukeButton.GetComponent<Button>();

        bombBtn.onClick.AddListener(BombTask);
        bounceBtn.onClick.AddListener(BounceTask);
        pickaxeBtn.onClick.AddListener(PickaxeTask);
        goLeftBtn.onClick.AddListener(GoLeftTask);
        goRightBtn.onClick.AddListener(GoRightTask);
        turnAroundBtn.onClick.AddListener(TurnAroundTask);
        speedUpBtn.onClick.AddListener(SpeedUpTask);
        nukeBtn.onClick.AddListener(NukeTask);
    }

    //Set the possible job based on what button whas clicked. 
    //This variable is used in the BunnyChangeController to give to a certain bunny if clicked
    void BombTask() => possibleJob = "Bomb";
    
    void BounceTask() => possibleJob = "Bounce";
    
    void PickaxeTask() => possibleJob = "Pickaxe";
    
    void GoLeftTask() => possibleJob = "GoLeft";
    
    void GoRightTask() => possibleJob = "GoRight";

    void TurnAroundTask() => possibleJob = "TurnAround";

    void SpeedUpTask() => possibleJob = "SpeedUp";

    void NukeTask() => possibleJob = "Nuke";
}
