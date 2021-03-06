using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] string name;

    public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainerView;

    private Vector2 input;

    private Character character;

    // Properties

    public Sprite Sprite { get { return sprite; } }
    public string Name { get { return name; } }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        if(!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");


            // Disable Diagonal Movement
            if (input.x != 0) input.y = 0;

           
            if(input != Vector2.zero) // If input detected
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
            Interact();
    }

    void Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.Interactable);
        if(collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }    

    private void OnMoveOver()
    {
        checkForEncounters();
        checkForTrainers();
    }

    private void checkForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.Grass) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                character.Animator.IsMoving = false;
                OnEncountered();
            }
        }        
    }
    private void checkForTrainers()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.Fov);
        if (collider != null)
        {
            Debug.Log("In Trainers View");
            character.Animator.IsMoving = false;
            OnEnterTrainerView?.Invoke(collider);
        }        
    }

}
