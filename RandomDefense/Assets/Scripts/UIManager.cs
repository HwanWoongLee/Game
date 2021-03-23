using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    static public UIManager instance;

    public GameObject waitState;
    public GameObject waveState;

    public GameObject   unitInfo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void SendUnitInfo(int _id)
    {
        unitInfo.GetComponent<UIUnitInfo>().SetUnitInfo(_id);

        unitInfo.SetActive(true);
    }
    public void OffUnitInfo()
    {
        unitInfo.SetActive(false);
    }

    private void UpdateUIState()
    {
        switch (GameManager.instance.curGameState)
        {
            case GameManager.GameState.wait:
                waitState.SetActive(true);
                waveState.SetActive(false);
                break;
            case GameManager.GameState.wave:
                waitState.SetActive(false);
                waveState.SetActive(true);       
                break;
        }
    }
    
    private void Update()
    {
        UpdateUIState();
    }
}
