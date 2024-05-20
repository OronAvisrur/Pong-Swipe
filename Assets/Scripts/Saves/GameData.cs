using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Varibles to save:
    private int score;
    private int playerLifes;
    private float waitTimeForCoins;
    private float ballSpeed;
    private float goalKeeperSpeed;
    private int powerupID;
    private float[] goalKeeperLocation = new float[2];
    private float[] ballLocation = new float[2];

    public int Score{ get { return score; } set { score = value; } }
    public int PlayerLifes { get { return playerLifes; } set { playerLifes = value; } }
    public float WaitTimeForCoins { get { return waitTimeForCoins; } set { waitTimeForCoins = value; } }
    public float BallSpeed { get { return ballSpeed; } set { ballSpeed = value; } }
    public float GoalKeeperSpeed { get { return goalKeeperSpeed; } set { goalKeeperSpeed = value; } }
    public int PowerupID { get { return powerupID; } set { score = powerupID; } }
    public float[] GoalKeeperLocation
    {
        get { return goalKeeperLocation; }
        set { CopyArrayData(goalKeeperLocation, value); }
    }
    public float[] BallLocation
    {
        get{ return ballLocation; }
        set { CopyArrayData(ballLocation, value); }
    }
   
    public GameData(int currentScore, float[] currentGoalKeeperLocation, float[] currentBallLocation, int currentPlayerLifes, float currentWaitTimeForCoins, float currentGoalKeeperSpeed, float currentBallSpeed, int currentPowerupID)
    {
        score = currentScore;
        CopyArrayData(currentGoalKeeperLocation,goalKeeperLocation);
        CopyArrayData(currentBallLocation,ballLocation);
        playerLifes = currentPlayerLifes;
        waitTimeForCoins = currentWaitTimeForCoins;
        goalKeeperSpeed = currentGoalKeeperSpeed;
        ballSpeed = currentBallSpeed;
        powerupID = currentPowerupID;

    }

    public void CopyArrayData(float[] src, float[] dest)
    {
        //run on the smallest length
        int length = src.GetLength(0) < dest.GetLength(0) ? src.GetLength(0) : dest.GetLength(0);
        for(int i = 0; i < length; i++)
        {
            dest[i] = src[i];
        }

    }
}
