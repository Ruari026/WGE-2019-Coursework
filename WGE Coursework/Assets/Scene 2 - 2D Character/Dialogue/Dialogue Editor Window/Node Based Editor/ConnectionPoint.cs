using System;
using UnityEngine;
using UnityEditor;

public class ConnectionPoint
{
    public Rect rect;

    public ConnectionPointType type;

    public Node node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
    {
        //Setting Up ConnectionPoint
        this.node = node;
        this.type = type;
        rect = new Rect(0, 0, 10f, 20f);

        //Setting ConnectionPoint Styles
        if (type == ConnectionPointType.IN)
        {
            this.style = new GUIStyle();
            this.style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            this.style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            this.style.border = new RectOffset(4, 4, 12, 12);
        }
        else
        {
            this.style = new GUIStyle();
            this.style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            this.style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            this.style.border = new RectOffset(4, 4, 12, 12);
        }

        //Setting ConnectionPoint Actions
        this.OnClickConnectionPoint = OnClickConnectionPoint;
    }

    public void HandleConnectionPoint(Rect renderPos)
    {
        DrawConnectionPoint(renderPos);
    }

    public void DrawConnectionPoint()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch (type)
        {
            case ConnectionPointType.IN:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.OUT:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
    public void DrawConnectionPoint(Rect pos)
    {
        rect = pos;

        if (GUI.Button(pos, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}


public enum ConnectionPointType
{
    IN,
    OUT
}