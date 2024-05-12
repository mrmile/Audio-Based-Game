using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullSpawnManager : MonoBehaviour
{
    [Header("Spawn Locations")]
    [SerializeField] Transform spawnLocation01;

    [Header("Lasers")]
    [SerializeField] GameObject vBeamLaser;
    [SerializeField] GameObject hBeamLaser;
    [SerializeField] GameObject vuHeavylaser;
    [SerializeField] GameObject vdHeavylaser;
    [SerializeField] GameObject hlHeavylaser;
    [SerializeField] GameObject hrHeavylaser;
    [SerializeField] GameObject vuStumpinglaser;
    [SerializeField] GameObject vdStumpinglaser;
    [SerializeField] GameObject hlStumpinglaser;
    [SerializeField] GameObject hrStumpinglaser;

    [Header("Bombs")]
    [SerializeField] GameObject simpleBomb;

    [Header("Circles")]
    [SerializeField] GameObject simpleCircle;

    [Header("Squares")]
    [SerializeField] GameObject simpleSquare;

    [Header("Snakes")]
    [SerializeField] GameObject simpleSnake;
    [SerializeField] GameObject doubleSnake;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn_vBeamLaser(float width, float height, float minRandX, float maxRandX, float livingTime, float warningTime)
    {
        GameObject newPrefab = Instantiate(vBeamLaser);

        
        V_BeamLaser prefabScript = newPrefab.GetComponent<V_BeamLaser>();
        if (prefabScript != null)
        {
            prefabScript.width = width;
            prefabScript.height = height;
            prefabScript.minRandX = minRandX;
            prefabScript.maxRandX = maxRandX;
            prefabScript.livingTime = livingTime;
            prefabScript.warningTime = warningTime;
        }
    }

    public void Spawn_vBeamLaser2()
    {

    }
}
