using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapNode : MonoBehaviour //holds Node info and handles visuals
{
    //Node type
    public enum NodeType { Empty, DamageItemGet, HealItemGet, BattleEncounter }
    public NodeType nodeType;
    public string popupText;
    public ItemData itemToGive;
    public string sceneToLoad = "";
    //Node general
    public List<MapNode> connectedNodes = new List<MapNode>();
    public bool isVisited = false;
    public bool isReachable = false;
    public Color reachableColor = Color.white;
    public Color unreachableColor = new Color(0.4f, 0.4f, 0.4f, 1f);
    private SpriteRenderer spriteRenderer;

    void Start() //accesess the SpriteRenderer component and updates the visual representation of the node based on its reachability status
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();

        foreach (MapNode connected in connectedNodes) // adds the lines to the nodes more dynamically (do i don't have to redraw them every time if i move them)
        {
            GameObject lineObj = new GameObject("Line_" + gameObject.name + "_to_" + connected.gameObject.name);
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();

            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, connected.transform.position);
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.startColor = Color.red; 
            lr.endColor = Color.red;
            lr.sortingOrder = -1;
        }
    }

    public void SetReachable(bool value) //sets the reachability status of the node and updates its visual representation accordingly
    {
        isReachable = value;
        UpdateVisual();
    }

    void UpdateVisual() //handles the colour change stuff indicating if reachalbe or not 
    {
        if (spriteRenderer == null) { return; }
        if (isReachable)
        {
            spriteRenderer.color = reachableColor;

        }
        else
        {
            spriteRenderer.color = unreachableColor;

        }
    }
}
