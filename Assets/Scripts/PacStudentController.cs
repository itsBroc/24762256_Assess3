using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveDuration = 0.5f;
    private float tileSize = 1.0f;
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


    public int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };
    private int[,] totalLevelMap;



    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();

        pacStudentAnimator = pacStudent.GetComponent<Animator>();
        moveAudioSource = pacStudent.GetComponent<AudioSource>();

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 48f;
        
        totalLevelMap = GenerateFullGrid(levelMap);
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();

        if (tweener != null && tweener.IsTweenComplete())
        {
            TryMovePacStudent();
        }
        Debug.Log(lastInput);

        //PlayMoveAnimation();
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
        Vector3 direction = GetDirection(lastInput);
        Vector3 positionToGetTo = pacStudent.transform.position + direction;

        //if (IsMoveable(positionToGetTo))
        //{
            currentInput = lastInput;
            //bool isEatingPellet = CheckForPellet(positionToGetTo);
            StartMovement(positionToGetTo);
        //}
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

    private bool IsMoveable(Vector3 position)
    {
        int gridX = Mathf.RoundToInt(position.x / tileSize);
        int gridY = Mathf.RoundToInt(position.y / tileSize);

        if(gridX < 0 || gridX >= totalLevelMap.GetLength(0) || gridY < 0 || gridY >= totalLevelMap.GetLength(1))
        {
            return false;
        }
        int tileType = totalLevelMap[gridX, gridY];
        return tileType == 0 || tileType == 5 || tileType == 6;
    }

    private bool CheckForPellet(Vector3 position)
    {
        int gridX = Mathf.RoundToInt(position.x / tileSize);
        int gridY = Mathf.RoundToInt(position.y / tileSize);

        if (gridX < 0 || gridX >= totalLevelMap.GetLength(0) || gridY < 0 || gridY >= totalLevelMap.GetLength(1))
        {
            return false;
        }

        int tileType = totalLevelMap[gridX, gridY];
        if(tileType == 5 || tileType == 6)
        {
            return true;
        }
        return false;
    }

    private void StartMovement(Vector3 positionToGetTo)
    {
        tweener.AddTween(pacStudent.transform, pacStudent.transform.position, positionToGetTo, moveDuration);
        lastPosition = pacStudent.transform.position;
    }

    /*bool CheckValid(KeyCode input)
    {
        switch (input)
        {
            case KeyCode.W:
                return (totalLevelMap[currentX - 1, currentY] == 0 || totalLevelMap[currentX - 1, currentY] == 5 || totalLevelMap[currentX - 1, currentY] == 6);
            case KeyCode.A:
                return (totalLevelMap[currentX, currentY - 1] == 0 || totalLevelMap[currentX, currentY - 1] == 5 || totalLevelMap[currentX, currentY - 1] == 6);
            case KeyCode.S:
                return (totalLevelMap[currentX + 1, currentY] == 0 || totalLevelMap[currentX + 1, currentY] == 5 || totalLevelMap[currentX + 1, currentY] == 6);
            case KeyCode.D:
                return (totalLevelMap[currentX, currentY + 1] == 0 || totalLevelMap[currentX, currentY + 1] == 5 || totalLevelMap[currentX, currentY + 1] == 6);
            default:
                return false;
        }
    }*/

    /*private void PlayMoveAnimation()
    {
        Vector3 currentPosition = pacStudent.transform.position;
        Vector3 direction = (currentPosition - lastPosition).normalized;

        pacStudentAnimator.SetBool("PacStudent_Right", false);
        pacStudentAnimator.SetBool("PacStudent_Up", false);
        pacStudentAnimator.SetBool("PacStudent_Left", false);
        pacStudentAnimator.SetBool("PacStudent_Down", false);

        switch (currentInput)
        {
            case KeyCode.W:
                pacStudentAnimator.SetBool("PacStudent_Up", true);
                break;
            case KeyCode.A:
                pacStudentAnimator.SetBool("PacStudent_Left", true);
                break;
            case KeyCode.S:
                pacStudentAnimator.SetBool("PacStudent_Down", true);
                break;
            case KeyCode.D:
                pacStudentAnimator.SetBool("PacStudent_Right", true);
                break;
            default:
                break;

        }
        lastPosition = currentPosition;
    }*/

    private int[,] GenerateFullGrid(int[,] levelMap)
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);
        int totalRows = rows * 2 - 1;
        int totalCols = cols * 2;

        int[,] totalLevelMap = new int[totalRows, totalCols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int num = levelMap[x, y];

                totalLevelMap[x, y] = num;
                totalLevelMap[x, totalCols - 1 - y] = num;

                if (x != totalCols - 1)
                {
                    totalLevelMap[totalRows - 1 - x, y] = num;
                    totalLevelMap[totalRows - 1 - x, totalCols - 1 - y] = num;
                }
            }
        }
        return totalLevelMap;
    }

}
