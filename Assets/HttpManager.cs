using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpManager : MonoBehaviour
{

    [SerializeField]
    private string URL;
    // Start is called before the first frame update

    [SerializeField] Text[] textos;
    int contador = 0;
    private int usuarios = 0;

    void Start()
    {

    }

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string url = URL + "/scores";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);

            foreach (ScoreData score in resData.data)
            {
                usuarios++;
            }

            for (int i = 0; i <= usuarios; i++)
            {
                for (int j = 0; j < usuarios- 1 ; j++)
                {
                    if (resData.data[j].value < resData.data[j + 1].value)
                    {
                        var temp = resData.data[j];
                        resData.data[j] = resData.data[j + 1];
                        resData.data[j + 1] = temp;
                    }
                    
                    
                }
            }

            foreach (ScoreData s in resData.data)
            {
                textos[contador].text = s.username + " : " + s.value;
                contador++;
                //Debug.Log(s.user_id + " | " + s.value);
                //Debug.Log(usuarios);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void Reseteo()
    {
        contador = 0;
        usuarios = 0;
    }

}


[System.Serializable]
public class ScoreData
{
    public int user_id;
    public int value;
    public string username;

}

[System.Serializable]
public class Scores
{
    public ScoreData[] data;
}
