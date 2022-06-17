using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog, Cutscene }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start()
    {
        playerController.OnEnterTrainersView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();

            if(trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () => //When this triggers do a function that does {FUNCTION STUFF HERE}
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () => //You could make a standalone function and achieve the same thing
        {
            if(state == GameState.Dialog)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(state == GameState.Battle)//Undecided if/how combat will be implemented. This method would be good for turn based combat
        {
            //battleSystem.HandleUpdate();
        }
        else if(state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
