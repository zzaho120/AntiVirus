using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRadarChart : MonoBehaviour
{
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    private Stats stats;
    public CanvasRenderer radarMeshCanvasRenderer;

    //private void Awake()
    //{
    //    radarMeshCanvasRenderer = transform.Find("RadarMesh").GetComponent<CanvasRenderer>();
    //}

    public void SetStats(Stats stats)
    {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 *5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 50f;

        Vector3 eVirusVertex = Quaternion.Euler(0,0, -angleIncrement*0)* Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.StatType.E);
        int eVirusVertexIndex = 1;
        Vector3 bVirusVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.StatType.B);
        int bVirusVertexIndex = 2;
        Vector3 pVirusVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.StatType.P);
        int pVirusVertexIndex = 3;
        Vector3 iVirusVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.StatType.I);
        int iVirusVertexIndex = 4;
        Vector3 tVirusVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.StatType.T);
        int tVirusVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[eVirusVertexIndex] = eVirusVertex;
        vertices[bVirusVertexIndex] = bVirusVertex;
        vertices[pVirusVertexIndex] = pVirusVertex;
        vertices[iVirusVertexIndex] = iVirusVertex;
        vertices[tVirusVertexIndex] = tVirusVertex;

        uv[0] = Vector2.zero;
        uv[eVirusVertexIndex] = Vector2.one;
        uv[bVirusVertexIndex] = Vector2.one;
        uv[pVirusVertexIndex] = Vector2.one;
        uv[iVirusVertexIndex] = Vector2.one;
        uv[tVirusVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = eVirusVertexIndex;
        triangles[2] = bVirusVertexIndex;

        triangles[3] = 0;
        triangles[4] = bVirusVertexIndex;
        triangles[5] = pVirusVertexIndex;

        triangles[6] = 0;
        triangles[7] = pVirusVertexIndex;
        triangles[8] = iVirusVertexIndex;

        triangles[9] = 0;
        triangles[10] = iVirusVertexIndex;
        triangles[11] = tVirusVertexIndex;

        triangles[12] = 0;
        triangles[13] = tVirusVertexIndex;
        triangles[14] = eVirusVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);
    }
}
