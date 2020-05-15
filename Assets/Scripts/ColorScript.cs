using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScript : NetworkBehaviour
{

    private Color colour;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        colour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        ChangeColor(colour);
        if (isServer)
        {
            RpcSelectColor(colour);
        }
        else
        {
            CmdSelectColor(colour);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        ChangeColor(colour);
        if (hasAuthority)
        {
            RpcSelectColor(colour);
        }
        else
        {
            CmdSelectColor(colour);
        }
    }

    [Command]
    void CmdSelectColor(Color color)
    {
        ChangeColor(color);
        RpcSelectColor(color);
    }

    [ClientRpc]
    void RpcSelectColor(Color color)
    {
        if (isLocalPlayer)
        {
            return;
        }
        ChangeColor(color);
    }

    public void ChangeColor(Color color)
    {
        gameObject.transform.Find("Marker").GetComponent<Renderer>().material.color = color;
    }
}
