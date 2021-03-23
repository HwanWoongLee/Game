using UnityEngine;

using System.Collections;




using GooglePlayGames;

using GooglePlayGames.BasicApi;

using GooglePlayGames.BasicApi.SavedGame;

public class GameCenter : MonoBehaviour {

	void Start () {

        Social.localUser.Authenticate(

                (bool success) =>

                {

                    Debug.Log(" - Social:SingIn= " + success.ToString());

                }

            );

	}

	

	public void OnButtonClick()

    {

        long score = GameManager.instance.gameData.topWave;

        string leader_board_id = "SIMPLEBKREAR_HIGHSCORE";
        

        Social.ReportScore(score, leader_board_id,

                (bool success) =>

                {

                    Social.ShowLeaderboardUI();

                }

        );

        

    }

    public void OnButtonClick2()
    {
        Social.ShowAchievementsUI();
    }

}