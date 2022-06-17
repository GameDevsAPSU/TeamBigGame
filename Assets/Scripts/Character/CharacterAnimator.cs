using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection { Up, Down, Left, Right }
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down;

    //Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    //States
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;
    bool wasMoving;

    //References
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);

        SetFacingDirection(defaultDirection);
        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        
        if(MoveX == 1)
        {
            currentAnim = walkRightAnim;
        }
        else if (MoveX == -1)
        {
            currentAnim = walkLeftAnim;
        }
        else if(MoveY == 1)
        {
            currentAnim = walkUpAnim;
        }
        else if(MoveY == -1)
        {
            currentAnim = walkDownAnim;
        }

        if(currentAnim != prevAnim || IsMoving != wasMoving)
        {
            currentAnim.Start();
        }

        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnim.Frames[0];
        }

        //wasMoving fixes bug of player sometimes appearing to slide
        wasMoving = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.Right)
        {
            MoveX = 1;
        }
        else if (dir == FacingDirection.Left)
        {
            MoveX = -1;
        }
        else if (dir == FacingDirection.Up)
        {
            MoveY = 1;
        }
        else if (dir == FacingDirection.Down)
        {
            MoveY = -1;
        }
    }

    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    } 
}
