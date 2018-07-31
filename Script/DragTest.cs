using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour {

    private List<DragMe> dragItemList = new List<DragMe>();
	// Use this for initialization
	void Awake () {
        DragMe dragMe = transform.Find("drag").GetComponent<DragMe>();
        DragMe dragMe1 = transform.Find("drag1").GetComponent<DragMe>();
        DragMe dragMe2 = transform.Find("drop").GetComponent<DragMe>();
        dragItemList.Add(dragMe);
        dragItemList.Add(dragMe1);
        dragItemList.Add(dragMe2);
        setCallBack();
	}

    void setCallBack()
    {
        for (int i = 0; i < dragItemList.Count; i++)
        {
           testDragCallBack dd = new testDragCallBack();
            dd.index = i;
            dragItemList[i].dragCallBack = dd;
        } 
     }
	
	// Update is called once per frame
	void Update () {
		
	}
}
