using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    /*
	todo:
	ráér
	rendet rakni texture mappába
	//zölditeni a tilesetet
	blockid és textuárk listája
	kell:
	player és caamera movement
	block törlés és lerakás
	világ genrálás és biomok
	*/
	//model object of the mesh
    public MeshRenderer meshRenderer;
	//view object of the mesh
    public MeshFilter meshFilter;
	List<Vector3> vertices = new List<Vector3>(); 
	List<int> triangles = new List<int>(); //store the trinagle index
	//set texture
	List<Vector2> textures = new List<Vector2>();
	int vertexIndex = 0;
	//is there a block in the postion or just air
	byte[,,] voxelMap = new byte[Voxel.ChunkWidth, Voxel.ChunkHeight, Voxel.ChunkWidth];
	World world;
	void FillVoxelMap(){
		for (int y = 0; y < Voxel.ChunkHeight; y++) {
			for (int x = 0; x < Voxel.ChunkWidth; x++) {
				for (int z = 0; z < Voxel.ChunkWidth; z++) {
					//voxelMap[x, y, z] = 0;
					if (y < 1)
                        voxelMap[x, y, z] = 4;//bedrock
                    else if (y == Voxel.ChunkHeight - 1)
                        voxelMap[x, y, z] = 3; //grass
					else if (y == Voxel.ChunkHeight - 1)
						voxelMap[x, y, z] = 2; //dirt
                    else
                        voxelMap [x, y, z] = 1; //stone
    
					
				}
			}
		}
	}
	bool CheckVoxel(Vector3 position){
		//convert player's position to integer
		int x = Mathf.FloorToInt(position.x);
		int y = Mathf.FloorToInt(position.y);
		int z = Mathf.FloorToInt(position.z);
		//are we in the current chunk?
		if (x < 0 || x > Voxel.ChunkWidth - 1 || y < 0 || y > Voxel.ChunkHeight - 1 || z < 0 || z > Voxel.ChunkWidth - 1)
			return false;//no
		//yes
		return world.blocktypes[voxelMap[x, y, z]].isSolid;
	}
	void SetTexture(int texture){
		//calculate the topleft corner position of the given texture
		float y= texture/Voxel.AtlasSize;//get actual line index
		float x= texture-(y*Voxel.AtlasSize); //get actual colum
		//convert to pixels
		x*=Voxel.GetTextureSize(); 
		y*=Voxel.GetTextureSize();
		//invert y to start indexing from the topleft texture
		y = 1f - y - Voxel.GetTextureSize();
		//set texture's corner coords
		textures.Add(new Vector2(x,y)); //top left
		textures.Add(new Vector2(x,y+Voxel.GetTextureSize())); //bottom left
		textures.Add(new Vector2(x+Voxel.GetTextureSize(),y)); //top right
		textures.Add(new Vector2(x+Voxel.GetTextureSize(),y)); //top right
		textures.Add(new Vector2(x,y+Voxel.GetTextureSize())); //second bottom left
		textures.Add(new Vector2(x+Voxel.GetTextureSize(),y+Voxel.GetTextureSize())); //bottom right 
	}
	void CreateVoxelModel(Vector3 position){
		//6 sides 2 triangle/side
		//iterate sides
		for (int side = 0; side < 6; side++){
			//check side's visibilty
			if(CheckVoxel(position+Voxel.visibilityChecker[side])==false){
				byte blockID = voxelMap[(int)position.x, (int)position.y, (int)position.z];
				
				for (int tri = 0; tri < 2; tri++){ //triangles of the side
					for(int p=0;p<3;p++){ //iterate the vertexes(points) of the triangle
						//save actual vertex
						vertices.Add(position+Voxel.voxelVerts[Voxel.voxelMesh[side,tri, p]]);
						triangles.Add(vertexIndex); //save index of the actual triangle
						//SetTexture(0); //set texture Voxel.textures[side]
						vertexIndex++;
					}
				}
				SetTexture(world.blocktypes[blockID].GetTextureId(side));
			}//skip side
		}
	}
	void CreateChunkModel(){
		for (int y = 0; y < Voxel.ChunkHeight; y++) {
			for (int x = 0; x < Voxel.ChunkWidth; x++) {
				for (int z = 0; z < Voxel.ChunkWidth; z++) {
					CreateVoxelModel(new Vector3(x, y, z));
				}
			}
		}
	}
	//create unity mesh
	void CreateMesh(){
		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		// Assign UV coordinates
        if (textures.Count == vertices.Count)
        {
            mesh.uv = textures.ToArray();
        }
        else
        {
            Debug.LogError("UV array size does not match the vertices array size.");
        }
		mesh.RecalculateNormals();//take normal vectors
		//set mesh filter
		meshFilter.mesh = mesh;
	}
    // Start is called before the first frame update
    void Start()
    {
		//get world object
		world = GameObject.Find("World").GetComponent<World>();
		FillVoxelMap();
		CreateChunkModel();
		CreateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
