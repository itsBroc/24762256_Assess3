using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveDuration = 0.5f;
    private float tileSize = 3f;
    private Animator pacStudentAnimator;
    public GameObject pacStudent;
    private Tweener tweener;

    private AudioSource moveAudioSource;
    private AudioClip eatPelletSound;
    private AudioClip moveSound;


    private KeyCode lastInput;
    private KeyCode currentInput;
    private Vector3 lastPosition;
    //private int currentX = 1;
    //private int currentY = 1;



    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();

        pacStudentAnimator = pacStudent.GetComponent<Animator>();
        moveAudioSource = pacStudent.GetComponent<AudioSource>();

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 48f;
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();

        if (tweener != null && tweener.IsTweenComplete())
        {
            TryMovePacStudent();
        }

        PlayMoveAnimation();
    }

    private void HandleKey()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
    }

    private void TryMovePacStudent()
    {
        Vector3 direction = GetDirection(lastInput) * 3;
        Vector3 positionToGetTo = pacStudent.transform.position + direction;

        if (IsMoveable(direction))
        {
            currentInput = lastInput;
            //bool isEatingPellet = CheckForPellet(positionToGetTo);
            StartMovement(positionToGetTo);
        }
        else
        {
            direction = GetDirection(currentInput) * 3;
            positionToGetTo = pacStudent.transform.position + direction;

            if (IsMoveable(direction))
            {
                StartMovement(positionToGetTo);
            }
        }
    }

    private Vector3 GetDirection(KeyCode input)
    {
        switch (input)
        {
            case KeyCode.W: return Vector3.up;
            case KeyCode.A: return Vector3.left;
            case KeyCode.S: return Vector3.down;  
            case KeyCode.D: return Vector3.right; 
            default: return Vector3.zero;
        }
    }

    private bool IsMoveable(Vector3 direction)
    {
        Vector3 rayOrigin = pacStudent.transform.position;
        Vector2 boxSize = new Vector2(2.0f, 2.0f);

        RaycastHit2D hit = Physics2D.BoxCast(rayOrigin, boxSize, 0f, direction, 2.0f);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return false;
            }
        }

        return true;
    }

    /*private bool CheckForPellet(Vector3 position)
    {

    }*/

    private void StartMovement(Vector3 positionToGetTo)
    {
        tweener.AddTween(pacStudent.transform, pacStudent.transform.position, positionToGetTo, moveDuration);
        lastPosition = pacStudent.transform.position;
    }



    private void PlayMoveAnimation()
    {

        Vector3 currentPosition = pacStudent.transform.position;
        Vector3 direction = (currentPosition - lastPosition).normalized;

        pacStudentAnimator.SetBool("PacStudent_Right", false);
        pacStudentAnimator.SetBool("PacStudent_Up", false);
        pacStudentAnimator.SetBool("PacStudent_Left", false);
        pacStudentAnimator.SetBool("PacStudent_Down", false);

        if (direction.x > 0)
        {
            pacStudentAnimator.SetBool("PacStudent_Right", true);
        }
        else if (direction.x < 0)
        {
            pacStudentAnimator.SetBool("PacStudent_Left", true);
        }
        else if (direction.y > 0)
        {
            pacStudentAnimator.SetBool("PacStudent_Up", true);
        }
        else if (direction.y < 0)
        {
            pacStudentAnimator.SetBool("PacStudent_Down", true);
        }


        lastPosition = currentPosition;
    }



}
