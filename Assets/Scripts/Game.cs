﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public List<Player> players = new List<Player>(); // All players in the game

    public int totalRounds = 5; // Rounds to play before the game ends.
    private int round = 1; // The current round.

    public float gunAccuracy = 0.5f; // 1 = 100% hit rate, 0 = 0% hit rate

    public int pot = 500; // The prize for winning a round

    public float secondsBeforeFiring = 3.0f; // The number of seconds between the players drawing and firing

    // Use this for initialization
    void Start () {

        while (round <= totalRounds)
        {
            // Reset all players
            for (int i = 0; i < players.Count; i++)
                players[i].reset();

            playRound();
            round++;
        }
	}
	
    /// <summary>
    /// Plays a round
    /// </summary>
    void playRound()
    {
        while (playerTargetsSet() == false); // Loop until targets set

        for (int i = 0; i < players.Count; i++)
            players[i].draw();

        wait(secondsBeforeFiring);

        // Roll for duds
        for (int i = 0; i < players.Count; i++)
        {
            if (Random.Range(0.0f, 1.0f) >= gunAccuracy)
                players[i].shoot(true);
            else
                players[i].shoot(false);
        }

        List<Player> livingPlayers = getLivingPlayers();

        if (livingPlayers.Count == 1)
        {
            livingPlayers[0].points += pot;
        }
        else if (livingPlayers.Count == 2)
        {
            int potDifference = Random.Range(-(pot / 10), pot / 10);

            livingPlayers[0].points += (pot / 2) + potDifference;
            livingPlayers[1].points += (pot / 2) - potDifference;
        }
        else
        {
            // no winners
        }
    }

    /// <summary>
    /// Waits boi
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// Checks if all players have confirmed a target
    /// </summary>
    /// <returns>True if all players have a target, else false.</returns>
    private bool playerTargetsSet()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].targetSet == false)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Gets a list of living players.
    /// </summary>
    /// <returns></returns>
    private List<Player> getLivingPlayers()
    {
        List<Player> livingPlayers = new List<Player>();

        // See who's still alive
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].isAlive)
                livingPlayers.Add(players[i]);
        }

        return livingPlayers;
    }
}
