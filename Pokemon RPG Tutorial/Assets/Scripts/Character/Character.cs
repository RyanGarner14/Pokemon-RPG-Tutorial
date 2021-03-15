using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;

    public bool IsMoving { get; private set; }

    CharacterAnimator animator;

    public CharacterAnimator Animator
    {
        get { return animator; }
    }

    void Awake()
    {
        animator = GetComponent<CharacterAnimator>();        
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!isWalkable(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // while the distance between the current pos and the target pos is greater than a very small number
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); // move a small amount towards target
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.Solid | GameLayers.i.Interactable) != null)
        {
            return false;
        }

        return true;
    }
}
