using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;

public partial class TerrainGen : MeshInstance3D {
    [Export] public int QuadSize = 50;
    [Export] public int Resolution = 100;
    [Export] public float Height = 4f;
    [Export] public float Frequency = 0.1f;

    private FastNoiseLite _noise = new FastNoiseLite();

    public override void _Ready() {
        Mesh = GenerateTerrainMesh();
        GenerateCollision();
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
                float cx = (float)x / w * QuadSize - QuadSize * 0.5f;
                float cz = (float)z / h * QuadSize - QuadSize * 0.5f;
                float cy = _noise.GetNoise2D(cx, cz) * Height;

                verts.Add(new Vector3(cx, cy, cz));
                normals.Add(Vector3.Zero);
                uvs.Add(new Vector2((float)x / w, (float)z / h));
            }
        }



        // int oneDindex = (x * w) + y;
        for (int z = 0; z < h; z++) {
            for (int x = 0; x < w; x++) {

                // Map 2d pos into index (get every pos from the current quad)
                int a = x + z * (w + 1); // bottom left
                int b = x + (z + 1) * (w + 1); // top left
                int c = (x + 1) + z * (w + 1); // bottom right
                int d = (x + 1) + (z + 1) * (w + 1); // top right

                // First triangle
                indices.Add(c);
                indices.Add(b);
                indices.Add(a);

                // Second triangle
                indices.Add(d);
                indices.Add(b);
                indices.Add(c);

                // --- Calculate normals for these triangles ---
                Vector3 normal1 = ((verts[c] - verts[a]).Cross(verts[b] - verts[a])).Normalized();
                Vector3 normal2 = ((verts[d] - verts[c]).Cross(verts[b] - verts[c])).Normalized();

                normals[a] += normal1;
                normals[b] += normal1 + normal2;
                normals[c] += normal1 + normal2;
                normals[d] += normal2;
            }
        }

        // Normalize all vertex normals
        for (int i = 0; i < normals.Count; i++) {
            normals[i] = normals[i].Normalized();
        }

        // --- Convert lists to Godot arrays ---
        Godot.Collections.Array surface = new();
        surface.Resize((int)Mesh.ArrayType.Max);

        surface[(int)Mesh.ArrayType.Vertex] = verts.ToArray();
        surface[(int)Mesh.ArrayType.Normal] = normals.ToArray();
        surface[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
        surface[(int)Mesh.ArrayType.Index] = indices.ToArray();


        // --- Add surface to mesh ---
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surface);

        return mesh;
    }

    // I fucking hate this function
    private void GenerateCollision() {
        if (Mesh is ArrayMesh arrayMesh) {
            ConcavePolygonShape3D collision = new ConcavePolygonShape3D();
            collision.Data = arrayMesh.GetFaces();

            StaticBody3D body = new StaticBody3D();
            CollisionShape3D shape = new CollisionShape3D();
            shape.Shape = collision;

            body.AddChild(shape);
            AddChild(body);
        }
    }

}
