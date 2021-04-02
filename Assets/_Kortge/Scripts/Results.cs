using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ends the game either with a game over or with progress to the next scene.
/// </summary>
public class Results : MonoBehaviour
{
    /// <summary>
    /// Determines if either the boss or the player are dead.
    /// </summary>
    private bool gameOver = false;
    /// <summary>
    /// Determines if the player killed the boss.
    /// </summary>
    private bool victory;
    /// <summary>
    /// Determines how long the animation will be allowed to be played out before exiting the zone.
    /// </summary>
    private float endTime;
    public AudioManager audioManager;
    /// <summary>
    /// Leaves the player in the zone for a period of time depending on who was killed and either sends them to the next level if victorious or sends them to the game over screen if they failed.
    /// </summary>
    private void Update()
    {
        if (gameOver)
        {
            endTime -= Time.deltaTime;
            if(endTime <= 0)
            {
                if (victory)
                {
                    Outbreak.Game.GotoNextLevel();
                }
                else Outbreak.Game.GameOver();
            }
        }
    }
    /// <summary>
    /// Starts the update with a set time.
    /// </summary>
    /// <param name="bossKilled"></param>
    public void ResultsIn(bool bossKilled)
    {
        if (bossKilled)
        {
            endTime = 20;
            audioManager.Play("Crowd Cheer");
        }
        else endTime = 1;
        victory = bossKilled;
        gameOver = true;
    }
}
