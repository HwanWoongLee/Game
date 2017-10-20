using UnityEngine;
using System.Collections;

public class UIWave : MonoBehaviour {
    public UILabel spawnLabel;
    public UILabel waveLavel;
    public UILabel timeLabel;

    public void RendSpawnLabel()
    {
        spawnLabel.text = EnemyManager.instance.curEnemyNum
            + " / "
            + EnemyManager.instance.limitEnemyNum;
    }
    public void RendWaveLabel()
    {
        waveLavel.text = GameManager.instance.waveNum
            + " / "
            + GameManager.instance.waveMax;
    }
    public void RendTimeLabel()
    {
        timeLabel.text = "Next Wave : " + GameManager.instance.waveTime.ToString("N1");
    }

    private void Update()
    {
        RendSpawnLabel();
        RendWaveLabel();
        RendTimeLabel();
    }
}
