using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S; // a private Singleton

    [Header("Inscribed")]
    public Text         uitLevel;  // The UIText_Level Text
    public Text         uitShots;  // The UIText_Shots Text
    public Vector3      castlePos; // The place to put castles
    public GameObject[] castles;   // An array of the castles
    public Text uitWind;
    public float maxWind = 3f;

    [Header("Dynamic")]
    public int          level;     // The current level
    public int          levelMax;  // The number of levels
    public int          shotsTaken;
    public GameObject   castle;    // The current castle
    public GameMode     mode = GameMode.idle;
    public string       showing = "Show Slingshot"; // FollowCam mode
    public float wind = 0f;

    void Start() {
        S = this; // Define the Singleton

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists
        if (castle != null) {
            Destroy( castle );
        }

        // Destroy old projectiles if they exist (the method is not yet written)
        Projectile.DESTROY_PROJECTILES(); // This will be underlined in red

        // Instantiate the new castle
        castle = Instantiate<GameObject>( castles[level] );
        castle.transform.position = castlePos;

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        //change view
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);

        wind = Random.Range(-2, maxWind);               
        wind = Mathf.Round(wind * 10f) / 10f;                 

        if (uitWind != null) {                                
            if (Mathf.Abs(wind) < 0.05f) {
                uitWind.text = "Wind: Calm";
            } else if (wind > 0) {
                uitWind.text = "Tailwind >> " + wind.ToString("0.0");
            } else {
                uitWind.text = "Headwind << " + Mathf.Abs(wind).ToString("0.0");
            }
        }
    }

    void UpdateGUI() {
        // Show the data in the GUITexts
        uitLevel.text = "Level: "+(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update() {
        UpdateGUI();

        // Check for level end
        if ( (mode == GameMode.playing) && Goal.goalMet ) {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }

    }

    void NextLevel() {
        level++;
    if (level >= castles.Length) {                          
        GameOverUI.SHOW("You Win!\nShots: " + shotsTaken);
        return;                                         
    }
    StartLevel();
    }

    // Static method that allows code anywhere to increment shotsTaken
    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }
    static public float WIND
    {
        get { return (S == null) ? 0f : S.wind;}
    }

    // Static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE() {
        return S.castle;
    }
}