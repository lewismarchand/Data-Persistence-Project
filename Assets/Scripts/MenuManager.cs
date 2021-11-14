using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public string CurrentPlayerName;
    public string BestScoreName;
    public int BestScorePoints;

    public TextMeshProUGUI CurrentBestScore;

    [SerializeField] private GameObject nameInput;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBestScore();

        if (BestScorePoints == 0)
        {
            CurrentBestScore.text = "No best score";
            return;
        }
        else
        {
            CurrentBestScore.text = "Best Score : " + BestScoreName + " : " + BestScorePoints;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string BestScoreName;
        public int BestScorePoints;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();

        data.BestScoreName = BestScoreName;
        data.BestScorePoints = BestScorePoints;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (!File.Exists(path))
        {
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        BestScoreName = data.BestScoreName;
        BestScorePoints = data.BestScorePoints;
    }

    public void StartGame()
    {
        CurrentPlayerName = nameInput.GetComponent<TMP_InputField>().text;

        if (CurrentPlayerName == "")
        {
            StartCoroutine(WarnInvalidName());
            return;
        }
        
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
Application.Quit()
#endif
    }

    IEnumerator WarnInvalidName()
    {
        Color defaultBackgroundColor = nameInput.GetComponent<TMP_InputField>().image.color;
        Color incorectNameBackgroundColor = new Color(1f, 0.7f, 0.8f);
        float blinkTime = 0.075f;
        nameInput.GetComponent<TMP_InputField>().image.color = incorectNameBackgroundColor;
        yield return new WaitForSeconds(blinkTime);
        nameInput.GetComponent<TMP_InputField>().image.color = defaultBackgroundColor;
        yield return new WaitForSeconds(blinkTime);
        nameInput.GetComponent<TMP_InputField>().image.color = incorectNameBackgroundColor;
        yield return new WaitForSeconds(blinkTime);
        nameInput.GetComponent<TMP_InputField>().image.color = defaultBackgroundColor;
        yield return new WaitForSeconds(blinkTime);
        nameInput.GetComponent<TMP_InputField>().image.color = incorectNameBackgroundColor;
        yield return new WaitForSeconds(blinkTime);
        nameInput.GetComponent<TMP_InputField>().image.color = defaultBackgroundColor;
    }
}
