using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Manager : MonoBehaviour
{
    public float speed = 1f;
    private Animator pacStudentAnimator;
    public GameObject pacStudent;
    private AudioSource moveAudioSource;

    private Tweener tweener;
    private int currIndex = 0;
    private Vector3[] points = new Vector3[]
    {
        new Vector3(-37.5f, 40.5f, 0),
        new Vector3(-22.5f, 40.5f, 0),
        new Vector3(-22.5f, 28.5f, 0),
        new Vector3(-37.5f, 28.5f, 0)
    };
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();

        pacStudentAnimator = pacStudent.GetComponent<Animator>();
        moveAudioSource = pacStudent.GetComponent<AudioSource>();

        transform.position = points[0];

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 48f;

        MovePacStudent();

    }

    // Update is called once per frame
    void Update()
    {
        if (tweener != null && tweener.IsTweenComplete())
        {
            MovePacStudent();
        }
            PlayMoveAnimation();
    }

    private void MovePacStudent()
    {
        currIndex = (currIndex + 1) % points.Length;
        Vector3 nextPos = points[currIndex];
        tweener.AddTween(pacStudent.transform, pacStudent.transform.position, nextPos, speed);
        lastPosition = pacStudent.transform.position;

        moveAudioSource.Play();

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
