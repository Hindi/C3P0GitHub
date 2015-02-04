using UnityEngine;
using System.Collections;

public class PipesManager : MonoBehaviour {

    [SerializeField]
    private GameObject bigPipe;

    [SerializeField]
    private GameObject smallPipe;

    Vector3 bigPipeObjPos;
    Vector3 smallPipeObjPos;

    Vector3 bigPipeStartPos;
    Vector3 smallPipeStartPos;

    void Start()
    {
        bigPipeObjPos = bigPipe.transform.position;
        smallPipeObjPos = smallPipe.transform.position;
        bigPipeStartPos = bigPipeObjPos;
        smallPipeStartPos = smallPipeObjPos;
    }

    public void restart()
    {
        bigPipe.transform.position = bigPipeStartPos;
        smallPipe.transform.position = smallPipeStartPos;
    }

    void Update()
    {
        bigPipe.transform.position = Vector3.MoveTowards(bigPipe.transform.position, bigPipeObjPos, 0.01f);
        smallPipe.transform.position = Vector3.MoveTowards(smallPipe.transform.position, smallPipeObjPos, 0.01f);
    }

    public void moveBigPipe(float deltayPositionY)
    {
        //bigPipe.transform.Translate(new Vector3(0, deltayPositionY, 0));
        bigPipeObjPos = new Vector3(bigPipe.transform.position.x, 
            bigPipe.transform.position.y + deltayPositionY, bigPipe.transform.position.z);
    }

    public void moveSmallPipe(float deltayPositionY)
    {
        smallPipeObjPos = new Vector3(smallPipe.transform.position.x,
            smallPipe.transform.position.y + deltayPositionY, smallPipe.transform.position.z);
    }
}
