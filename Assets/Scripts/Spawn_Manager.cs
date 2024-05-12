using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum laserType
{
    DEAULT,
    V_BEAM,
    H_BEAM,
    VU_HEAVY,
    VD_HEAVY,
    VU_STUMPING,
    VD_STUMPING,
    HL_HEAVY,
    HR_HEAVY,
    HL_STUMPING,
    HR_STUMPING
}
public enum snakeType
{
    SIMPLE,
    DOUBLE
}
public class Spawn_Manager : MonoBehaviour
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnLaserRequest(laserType type)
    {
        switch(type)
        {
            case laserType.V_BEAM: SpawnObstacle(vBeamLaser, spawnLocation01);
                break;
            case laserType.H_BEAM:
                SpawnObstacle(vBeamLaser, spawnLocation01);
                break;
            case laserType.VU_HEAVY:
                SpawnObstacle(vuHeavylaser, spawnLocation01);
                break;
            case laserType.VD_HEAVY:
                SpawnObstacle(vdHeavylaser, spawnLocation01);
                break;
            case laserType.HL_HEAVY:
                SpawnObstacle(hlHeavylaser, spawnLocation01);
                break;
            case laserType.HR_HEAVY:
                SpawnObstacle(hrHeavylaser, spawnLocation01);
                break;
            case laserType.VU_STUMPING:
                SpawnObstacle(vuStumpinglaser, spawnLocation01);
                break;
            case laserType.VD_STUMPING:
                SpawnObstacle(vdStumpinglaser, spawnLocation01);
                break;
            case laserType.HL_STUMPING:
                SpawnObstacle(hlStumpinglaser, spawnLocation01);
                break;
            case laserType.HR_STUMPING:
                SpawnObstacle(hrStumpinglaser, spawnLocation01);
                break;
            default:
                break;

        }
    }

    public void SpawnBombRequest()
    {
        SpawnObstacle(simpleBomb, spawnLocation01);
    }
    public void SpawnCircleRequest()
    {
        SpawnObstacle(simpleCircle, spawnLocation01);
    }
    public void SpawnSquareRequest()
    {
        SpawnObstacle(simpleSquare, spawnLocation01);
    }
    public void SpawnSnakeRequest(snakeType type)
    {
        switch(type)
        {
            case snakeType.SIMPLE: SpawnObstacle(simpleSnake, spawnLocation01);
                break;
            case snakeType.DOUBLE: SpawnObstacle(doubleSnake, spawnLocation01);
                break;
            default:
                break;
        }
    }
    void SpawnObstacle(GameObject obstacle, Transform spawnPoint)
    {
        Instantiate(obstacle, spawnPoint);
    }
}
