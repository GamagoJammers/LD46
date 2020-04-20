using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public EnemyGenerator enemyGenerator;

    public RainManager rainManager;

    [Header("Max Enemy")]
    public int maxEnemyProgressionRate;
    public int maxEnemyLimit;
    [Header("Min Enemy Instatiation")]
    public int minEnemyInstantiationProgressionRate;
    public int minEnemyInstantiationLimit;
    [Header("Max Enemy Instatiation")]
    public int maxEnemyInstantiationProgressionRate;
    public int maxEnemyInstantiationLimit;
    [Header("Min Rain Time")]
    public int minRainTimeProgressionRate;
    public int minRainTimeLimit;
    [Header("Max Rain Time")]
    public int maxRainTimeProgressionRate;
    public int maxRainTimeLimit;
    [Header("Min Rain Duration")]
    public int minRainDurationProgressionRate;
    public int minRainDurationLimit;
    [Header("Max Rain Duration")]
    public int maxRainDurationProgressionRate;
    public int maxRainDurationLimit;
    [Header("Thunder")]
    public int thunderChanceRate;
    public int thunderChanceLimit;

    private int actualMaxEnemyProgressionRate;
    private int actualMinEnemyInstantiationProgressionRate;
    private int actualMaxEnemyInstantiationProgressionRate;
    private int actualMinRainTimeProgressionRate;
    private int actualMaxRainTimeProgressionRate;
    private int actualMinRainDurationProgressionRate;
    private int actualMaxRainDurationProgressionRate;
    private int actualThunderChanceRate;

    private float timer;
    private int actualTime;

    // Start is called before the first frame update
    void Start()
    {
        actualMaxEnemyProgressionRate = maxEnemyProgressionRate;
        actualMinEnemyInstantiationProgressionRate = minEnemyInstantiationProgressionRate;
        actualMaxEnemyInstantiationProgressionRate = maxEnemyInstantiationProgressionRate;
        actualMinRainTimeProgressionRate = minRainTimeProgressionRate;
        actualMaxRainTimeProgressionRate = maxRainTimeProgressionRate ;
        actualMinRainDurationProgressionRate = minRainDurationProgressionRate;
        actualMaxRainDurationProgressionRate = maxRainDurationProgressionRate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.instance.isPaused)
        {
            timer += Time.deltaTime;
            actualTime = Mathf.RoundToInt(timer % 60);
            EnemyProgression();
            RainProgression();
        }
    }

    private void EnemyProgression()
    {
        if (actualTime - actualMaxEnemyProgressionRate == 0 && enemyGenerator.maxEnemyNb < maxEnemyLimit)
        {
            actualMaxEnemyProgressionRate += maxEnemyProgressionRate;
            enemyGenerator.maxEnemyNb += 1;
        }
        if (actualTime - actualMaxEnemyInstantiationProgressionRate == 0 && enemyGenerator.timeBetweenInstantiation.max > maxEnemyInstantiationLimit)
        {
            actualMaxEnemyInstantiationProgressionRate += maxEnemyInstantiationProgressionRate;
            enemyGenerator.timeBetweenInstantiation.max -= 1;
        }
        if (actualTime - actualMinEnemyInstantiationProgressionRate == 0 && enemyGenerator.timeBetweenInstantiation.min > minEnemyInstantiationLimit)
        {
            actualMinEnemyInstantiationProgressionRate += minEnemyInstantiationProgressionRate;
            enemyGenerator.timeBetweenInstantiation.min -= 1;
        }
    }

    private void RainProgression()
    {
        if (actualTime - actualMinRainDurationProgressionRate == 0 && minRainDurationLimit > rainManager.minRainDuration)
        {
            actualMinRainDurationProgressionRate += minRainDurationProgressionRate;
            rainManager.minRainDuration += 1;
        }
        if (actualTime - actualMaxRainDurationProgressionRate == 0 && maxRainDurationLimit > rainManager.maxRainDuration)
        {
            actualMaxRainDurationProgressionRate += maxRainDurationProgressionRate;
            rainManager.maxRainDuration += 1;
        }
        if (actualTime - actualMinRainTimeProgressionRate == 0 && minRainTimeLimit < rainManager.minRainTime)
        {
            actualMinRainTimeProgressionRate += minRainTimeProgressionRate;
            rainManager.minRainTime -= 1;
        }
        if (actualTime - actualMaxRainTimeProgressionRate == 0 && maxRainTimeLimit < rainManager.maxRainTime)
        {
            actualMaxRainTimeProgressionRate += maxRainTimeProgressionRate;
            rainManager.maxRainTime -= 1;
        }
        if (actualTime - actualThunderChanceRate == 0 && thunderChanceLimit > rainManager.thunderChance)
        {
            actualThunderChanceRate += thunderChanceRate;
            rainManager.thunderChance += 1;
        }
    }
}
