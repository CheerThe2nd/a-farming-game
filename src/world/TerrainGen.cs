using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

public partial class TerrainGen : MeshInstance3D {
	[Export] public int Size = 50;
	[Export] public int Resolution = 100;  // number of subdivisions
	[Export] public float Height = 4f;
	[Export] public float Frequency = 0.1f;

	private FastNoiseLite _noise = new FastNoiseLite();

	public override void _Ready() {
		Mesh = GenerateTerrainMesh();
	}

	private ArrayMesh GenerateTerrainMesh() {
		var mesh = new ArrayMesh();

		List<Vector3> verts = new();
		List<Vector3> normals = new();
		List<Vector2> uvs = new();
		List<int> indices = new();

		_noise.Frequency = Frequency;

		int w = Resolution;
		int h = Resolution;

		// --- Generate vertices ---
		for (int z = 0; z <= h; z++) {
			for (int x = 0; x <= w; x++) {
				float xx = (float)x / w * Size - Size * 0.5f;
				float zz = (float)z / h * Size - Size * 0.5f;

				float yy = _noise.GetNoise2D(xx, zz) * Height;

				verts.Add(new Vector3(xx, yy, zz));
				normals.Add(Vector3.Up);
				uvs.Add(new Vector2((float)x / w, (float)z / h));
			}
		}

		// --- Generate triangle indices ---
		for (int z = 0; z < h; z++) {
			for (int x = 0; x < w; x++) {
				int i = x + z * (w + 1);

				int a = i;
				int b = i + w + 1;
				int c = i + 1;
				int d = i + w + 2;

				// First tri
				indices.Add(a);
				indices.Add(b);
				indices.Add(c);

				// Second tri
				indices.Add(c);
				indices.Add(b);
				indices.Add(d);
			}
		}

		// Convert lists to Godot arrays
		Godot.Collections.Array surface = new();
		surface.Resize((int)Mesh.ArrayType.Max);

		surface[(int)Mesh.ArrayType.Vertex] = verts.ToArray();
		surface[(int)Mesh.ArrayType.Normal] = normals.ToArray();
		surface[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
		surface[(int)Mesh.ArrayType.Index] = indices.ToArray();

		// Add surface to mesh
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surface);

		return mesh;
	}
}
