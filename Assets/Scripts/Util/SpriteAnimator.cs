using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For details on why this exists: https://youtu.be/3BsUZy_QCQo?list=PLH-tnk2RJMG6lwhDnfogKyoLusRYL8Og8&t=39
public class SpriteAnimator
{
    SpriteRenderer spriteRenderer;
    List<Sprite> frames;
    float frameRate;

    int currentFrame;
    float timer;

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.spriteRenderer = spriteRenderer;
        this.frames = frames;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;

        if(timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }

    public List<Sprite> Frames
    {
        get { return frames; }
    }
}
