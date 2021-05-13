using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int m_trackCount = 5;
    public float m_trackWidth = 1.3f;

    public GameObject m_donePanel;
    public Button m_btnRestart;

    private void Awake()
    {
        Instance = this;
        m_btnRestart.onClick.AddListener(OnClickRestart);
    }

    public void GameOver(PlayerController player)
    {
        Camera.main.transform.SetParent(null);
        Destroy(player.gameObject);

        m_donePanel.SetActive(true);
    }

    private void OnClickRestart()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}