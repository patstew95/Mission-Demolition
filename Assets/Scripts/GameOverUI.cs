using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    static private GameOverUI S;          // singleton, same pattern as MissionDemolition

    [Header("Inscribed")]
    public GameObject panel;              // the GameOverPanel 
    public Text       messageText;        // optional: "Game Over" / score text

    void Awake() {
        S = this;
        panel.SetActive(false);           // hidden until the game ends
    }

    // Call this from anywhere: GameOverUI.SHOW("Out of shots!");
    static public void SHOW(string msg = "Game Over") {
        if (S == null) return;
        if (S.messageText != null) S.messageText.text = msg;
        S.panel.SetActive(true);
        Time.timeScale = 0f;              // freeze physics/projectiles
    }

    // Hook this to the Restart button's OnClick()
    public void Restart() {
        Time.timeScale = 1f;              // unfreeze before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}