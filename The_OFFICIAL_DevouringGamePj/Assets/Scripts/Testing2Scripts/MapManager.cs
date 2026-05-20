using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance; //makes mapmanager findable without needing to make a new variable each script (i believe)
    public MapNode startNode;
    public GameObject playerPiece;
    public float moveSpeed = 5f;
    private MapNode currentNode;
    private bool isMoving = false;

    void Awake()
    {
        Instance = this; //sets the instance variable to the current instance of MapManager when the script is loaded
    }

    void Start() 
    {
        // restores position on map when coming back form another scene
        string savedNodeName = PlayerPrefs.GetString("CurrentNode", ""); //retrieves the name of the last visited node from PlayerPrefs, defaulting to the startNode if no saved data is found, "" is the default
        if (savedNodeName != "")
        {
            foreach (MapNode node in FindObjectsByType<MapNode>(FindObjectsSortMode.None)) //looks through all MapNode objects in the scene to find the one that matches the saved node name
            {
                if (node.gameObject.name == savedNodeName)
                {
                    currentNode = node; //sets the currentNode to the found node
                    break; // just stops the loop once the node is found, since we don't need to keep looking
                }
                
            }
        }
        // if nothing is saved, go to start node
        if (currentNode == null)
        {
            currentNode = startNode;
        }
        // move player piece to the current node's position
        playerPiece.transform.position = currentNode.transform.position;
        currentNode.isVisited = true; //marks the current node as visited
        UnlockConnectedNodes(currentNode); 
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);
            if (hit != null)
            {
                MapNode node = hit.GetComponent<MapNode>();
                if (node != null)
                {
                    TryToMoveToNode(node);
                }
            }
        }
    }

    public void TryToMoveToNode(MapNode target)
    {
        if (isMoving) { return; }
        if (!target.isReachable) { return; }
        StartCoroutine(MoveToNode(target));
    }

    IEnumerator MoveToNode(MapNode target)
    {
        isMoving = true;
        LockAllNodes();
        while (Vector3.Distance(playerPiece.transform.position, target.transform.position) > 0.01f)
        {
            playerPiece.transform.position = Vector3.MoveTowards(playerPiece.transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        //snaps exactly to node
        playerPiece.transform.position = target.transform.position;
        //updates state
        currentNode = target;
        currentNode.isVisited = true;
        isMoving = false;
        //saves Postion
        PlayerPrefs.SetString("currentNode", currentNode.gameObject.name);
        PlayerPrefs.Save();
        //unlocks the next nodes
        UnlockConnectedNodes(currentNode);
    }

    void UnlockConnectedNodes(MapNode node)
    {
        foreach (MapNode connected in node.connectedNodes)
        {
            if(!connected.isVisited)
            {
                connected.SetReachable(true);
            }
        }
    }

    void LockAllNodes()
    {
        foreach (MapNode node in FindObjectsByType<MapNode>(FindObjectsSortMode.None))
        {
            node.SetReachable(false);
        }
    }

}
