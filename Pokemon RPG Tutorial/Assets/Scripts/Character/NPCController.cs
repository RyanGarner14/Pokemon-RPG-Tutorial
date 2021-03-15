using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;

    NPCState state;
    float idleTimer = 0;
    int currentPattern;

    Character character;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact()
    {
        if(state == NPCState.Idle)
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

    private void Update()
    {
        if (DialogManager.Instance.IsShowing)
            return;

        character.HandleUpdate();
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer > timeBetweenPattern)
            {
                idleTimer = 0;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        yield return character.Move(movementPattern[currentPattern]);
        currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }
}

public enum NPCState { Idle, Walking }
