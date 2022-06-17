using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<Collider2D> OnEnterTrainersView;//These are used in something called Observer pattern, useful for triggering a function without storing a reference to the function's object

    private Vector2 input;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(input.x != 0)//Stop diagonal movement
            {
                input.y = 0;
            }

            if(input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        
        Debug.DrawLine(transform.position, interactPos, Color.cyan, 0.5f);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);//The ? means if you can do it, do it. Without that if the object lacked Interact() it would throw an error
        }
    }

    private void OnMoveOver()
    {
        CheckIfInTrainersView();
    }

    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FOVLayer);

        if (collider != null)
        {
            character.Animator.IsMoving = false;
            OnEnterTrainersView?.Invoke(collider);
        }
    }
}
