using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HttpManager : MonoBehaviour
{

    [SerializeField]
    private string URL;

    [Header("MenuRegistro")]
    [SerializeField] InputField userfield;
    [SerializeField] InputField passfield;

    [Header("Textos")]
    [SerializeField] Text[] textos;
    int contador = 0;
    private int usuarios = 0;

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    public void ClickRegistro()
    {
        DataApi data = new DataApi();

        data.username = userfield.text;
        data.password = passfield.text;

        string postData = JsonUtility.ToJson(data); 

        StartCoroutine(Registro(postData));
    }

    public void ClickIngreso()
    {
        DataApi data = new DataApi();

        data.username = userfield.text;
        data.password = passfield.text;

        string postData = JsonUtility.ToJson(data);

        StartCoroutine(Ingreso(postData));
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

    IEnumerator Registro(string postData)
    {

        Debug.Log(postData);
        
        string url = URL + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");


        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            DataApi resData = JsonUtility.FromJson<DataApi>(www.downloadHandler.text);

            //resData.usuario.username;

            Debug.Log("Bienvenido " + resData.usuario.username + ", id: " + resData.usuario._id);

            StartCoroutine(Ingreso(postData));
            PlayerPrefs.SetString("token", resData.token);
            PlayGame();
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator Ingreso(string postData)
    {

        Debug.Log(postData);

        string url = URL + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");


        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            //Debug.Log(www.downloadHandler.text);
            DataApi resData = JsonUtility.FromJson<DataApi>(www.downloadHandler.text);

            //resData.usuario.username;

            Debug.Log("Bienvenido " + resData.usuario.username + ", id: " + resData.usuario._id);
            PlayerPrefs.SetString("token", resData.token);
            PlayGame();
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Principal");
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

[System.Serializable]
public class DataApi
{
    public string username;
    public string password;
    public DataUser usuario;
    public string token;
}

[System.Serializable]
public class DataUser
{
    public string _id;
    public string username;
    public bool estado;
    public int score;
}
