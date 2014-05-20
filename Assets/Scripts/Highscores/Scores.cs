using UnityEngine;
using System.Collections;

public static class Scores {

	public static int total = 0;

	public static int maxCombo = 0;
	public static int enemiesKilled = 0;
	public static int turnsSurvived = 0;

	public static void clearScores()
	{
		total = 0;
		
		maxCombo = 0;
		enemiesKilled = 0;
		turnsSurvived = 0;
	}
}
