using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmightySingleton : MonoBehaviour
{
    #region Singleton And Awake()
    private static AlmightySingleton _instance;
    public static AlmightySingleton Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
	#endregion

	#region TestDelegate
	public delegate void TestSignal(string signalID);
    public event TestSignal TestSignalEvent;

    public void FireSignal(string signalID)
    {
        TestSignalEvent(signalID);
    }
    #endregion

    // We need a struct holding player stats (Health and Heat)
    // We need access to UI elements or a way for UI elements to access Player stats
    // We need a way for the Player and their Gun to access the stats
    // Notable Unity tools
    // - Gradient Variable - gradient.Evaluate(float) takes 0-1 and returns a color corresponding to the amount. Set the fill color on the UI to this.
    // Use an Observer Pattern to signal to the UI to update?

    PlayerGameStats _playerStats = null;
    public PlayerGameStats PlayerStats => _playerStats;

	void Start()
	{
        if(_playerStats == null)
		{
            _playerStats = new PlayerGameStats(100, 50, 4f);
		}        
	}

	private void Update()
	{
        _playerStats.Cooldown(Time.deltaTime);
	}
}
