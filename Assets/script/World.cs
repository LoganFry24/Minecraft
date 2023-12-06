using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;
    public Block[] blocktypes;
}
[System.Serializable]//to edit in unity
public class Block
{
    public string Name;
    public bool isSolid;
    //set all texture sides of the block
    public int southTexture;
    public int northTexture;
    public int topTexture;
    public int bottomTexture;
    public int westTexture;
    public int eastTexture;
    //get actual side's textureId
    
    public int GetTextureId(int dir){
        switch (dir)
        {
            case 0:
                return southTexture;
            case 1:
                return northTexture;
            case 2:
                return topTexture;
            case 3:
                return bottomTexture;
            case 4:
                return westTexture;
            case 5:
                return eastTexture;
            default:
                Debug.Log("Invalid direction!");
                return 0;
        }
    }
}
