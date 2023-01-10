using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    BoxArea _currentArea;
    BoxArea CurrentArea
    {
        get { return _currentArea; }
        set
        {
            _currentArea = value;
            Transform moveTo = _currentArea.GetComponent<Transform>();
            SetPosition(moveTo);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentArea = GameObject.Find("BoxArea 5").GetComponent<BoxArea>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
            Debug.Log("Click!");
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
            Debug.Log("Click!");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
            Debug.Log("Click!");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
            Debug.Log("Click!");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

    }
    void Attack()
    {
        Transform attack = transform.GetChild(0);
        attack.GetComponent<PlayerAttack>().Attacking();
        
    }
    bool MoveUp()
    {
        if(CurrentArea.Up != null && CurrentArea.Up.Moveable)
        {
            CurrentArea = CurrentArea.Up;
            return true;
        }
        return false;
    }
    bool MoveDown()
    {
        if (CurrentArea.Down != null && CurrentArea.Down.Moveable)
        {
            CurrentArea = CurrentArea.Down;
            return true;
        }
        return false;
    }
    bool MoveLeft()
    {
        if (CurrentArea.Left != null && CurrentArea.Left.Moveable)
        {
            CurrentArea = CurrentArea.Left;
            return true;
        }
        return false;
    }
    bool MoveRight()
    {
        if (CurrentArea.Right != null && CurrentArea.Right.Moveable)
        {
            CurrentArea = CurrentArea.Right;
            return true;
        }
        return false;
    }
    void SetPosition(Transform moveTo)
    {
        transform.position = new Vector3(moveTo.position.x, moveTo.position.y, 0) ;
    }
}
