using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public bool IsMoving { get; private set; }

    CharacterAnimator animator;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
        {
            yield break;
        }

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)//While distance between player and target is larger than really small number, move towards it over time
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;//Once close enough set player position to target position

        IsMoving = false;
        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;//Info on normalizing vectors: https://youtu.be/-M_tDVLx9Qg?list=PLH-tnk2RJMG6lwhDnfogKyoLusRYL8Og8&t=480

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer))
        {
            return false;
        }

        return true;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidObjectsLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }

        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
        {
            Debug.Log("ERROR: There's no art budget for diagonal sprites! Please stick to making the NPC look in cardinal directions!");
        }
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}
