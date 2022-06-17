using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    Character character;
    bool isExclaimed = false;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFOVRotation(character.Animator.DefaultDirection);
    }

    public void Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!isExclaimed)
        {
            StartCoroutine(TriggerTrainerBattle(initiator?.GetComponent<PlayerController>()));
            BattleLost();
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfterBattle));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        //Show exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        //Move towards player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        //Show dialog
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => {
            //If we implement rpg-style combat this would be a good place to initiate that
            BattleLost();
        }));
    }

    public void BattleLost()//Temp name, kept for sake of tutorial video usage
    {
        isExclaimed = true;
        fov.gameObject.SetActive(false);
    }

    public void SetFOVRotation(FacingDirection dir)
    {
        float angle = 0f;

        if(dir == FacingDirection.Right)
        {
            angle = 90f;
        }
        else if(dir == FacingDirection.Up)
        {
            angle = 180f;
        }
        else if(dir == FacingDirection.Left)
        {
            angle = 270f;
        }

        fov.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
