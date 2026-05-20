using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MapNode : MonoBehaviour //holds Node info and handles visuals
{
    //Node type
    public enum NodeType { Start, ItemGet, BattleEncounter }
    public NodeType nodeType;
    public string popupText;
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
