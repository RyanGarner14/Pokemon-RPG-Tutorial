using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] string name;

    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;
    [SerializeField] Dialog dialog;
    
    // Properties

    public Sprite Sprite  {   get { return sprite; }  }
    public string Name    {   get { return name; }    }

    Character character;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    void Start()
    {
        setFovRotation(character.Animator.DefaultFacing);
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        // Show Exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // Trainer moves towards player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // Show Dialog
        yield return DialogManager.Instance.ShowDialog(dialog, () =>
        {
            GameController.Instance.StartTrainerBattle(this);
        });
    }

    public void setFovRotation(FacingDirection dir)
    {
        float angle = 0;
        if (dir == FacingDirection.Right)
            angle = 90;
        else if (dir == FacingDirection.Up)
            angle = 180;
        else if (dir == FacingDirection.Left)
            angle = 270;
        else if (dir == FacingDirection.Down)
            angle = 0;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
