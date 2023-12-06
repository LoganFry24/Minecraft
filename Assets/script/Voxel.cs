using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Voxel
{
    public static readonly int ChunkWidth = 16;
	public static readonly int ChunkHeight = 16;
    //texture atlas paramteres
    //atlassize in block (16*16)
    public static readonly int AtlasSize = 16;
    public static float GetTextureSize(){
        //get 1 block's texturesize in float 
        return 1f / (float)AtlasSize; 
    }
    //Vector3 (float)(x,y,z) == one vertex(point)
    public static readonly Vector3[] voxelVerts = new Vector3[8] {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
    };
    //Triangle (Vector3)(vertex1,vertex2,vertex3) == one triangle
    //6 sides 2 triangle/side 3 vertex/triangle
	public static readonly int[,,] voxelMesh = new int[6,2,3] {
		//south
        {{0, 3, 1}, //triangle
        { 1, 3, 2},},//side
        //north
		{{5, 6, 4},
        {4, 6, 7},},
        //top
		{{3, 7, 2},
        {2, 7, 6},},
        //bottom
		{{1, 5, 0}, 
        {0, 5, 4},},
        //west
		{{4, 7, 0}, 
        {0, 7, 3},},
        //east
		{{1, 2, 5},
        {5, 2, 6}} 
	};
    public static readonly Vector2[] textures = new Vector2[6] {
        new Vector2 (0.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 1.0f)
    };
    //for optimization (don't draw unneceserry sides)
    //define each side's visibilty vector
    public static readonly Vector3[] visibilityChecker = new Vector3[6] {
		new Vector3(0.0f, 0.0f, -1.0f), //check south side
		new Vector3(0.0f, 0.0f, 1.0f), //north
		new Vector3(0.0f, 1.0f, 0.0f), //top
		new Vector3(0.0f, -1.0f, 0.0f),//bottom
		new Vector3(-1.0f, 0.0f, 0.0f),//west
		new Vector3(1.0f, 0.0f, 0.0f)//east
	};
}
