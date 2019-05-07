using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] Text showTxt = null;
    [SerializeField] InputField inputField = null;
    [SerializeField] Button btnSendSuggestion = null;

    SocketIOComponent socketIOComponent = null;

    bool isGameEnd = false;

    void Awake()
    {
        socketIOComponent = GetComponent<SocketIOComponent>();
    }
    void Start()
    {
        socketIOComponent.On("login", OnLoginSuccess);
        socketIOComponent.On("return result", OnReturnResult);
        socketIOComponent.On("new lottery created", OnNewLotteryCreated);
    }
    public void SendSuggestion()
    {
        if (isGameEnd)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
        else
        {
            int suggestNumber = 0;
            try
            {
                suggestNumber = int.Parse(inputField.text);
            }
            catch (System.InvalidCastException)
            {
                suggestNumber = 0;
            }
            string data = JsonUtility.ToJson(new PlayerJSON(PlayerData.name, suggestNumber));
            socketIOComponent.Emit("request suggest", new JSONObject(data));
        }
    }
    void OnLoginSuccess(SocketIOEvent socketIOEvent)
    {
        string data = JsonUtility.ToJson(new PlayerJSON(PlayerData.name, -1));
        socketIOComponent.Emit("login success", new JSONObject(data));
    }
    void OnReturnResult(SocketIOEvent socketIOEvent)
    {
        var result = CheckJSON.CreateFromJSON(socketIOEvent.data.ToString());

        switch (result.status)
        {
            case -1:
                showTxt.text = "Too Low";
                showTxt.color = Color.red;
                break;
            case 0:
                if (result.name == PlayerData.name)
                {
                    showTxt.text = "You win the lottery";
                    showTxt.color = Color.green;
                }
                else
                {
                    showTxt.text = $"{result.name} win the lottery";
                    showTxt.color = Color.yellow;
                }
                break;
            case 1:
                showTxt.text = "Too High";
                showTxt.color = Color.red;
                break;
        }
    }
    void OnNewLotteryCreated(SocketIOEvent socketIOEvent)
    {
        isGameEnd = true;
        btnSendSuggestion.GetComponentInChildren<Text>().text = "New Game";
    }
}
