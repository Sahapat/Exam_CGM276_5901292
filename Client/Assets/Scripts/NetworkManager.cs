using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] Text showTxt = null;
    [SerializeField] InputField inputField = null;

    SocketIOComponent socketIOComponent = null;

    void Awake()
    {
        socketIOComponent = GetComponent<SocketIOComponent>();
    }
    void Start()
    {
        socketIOComponent.On("login", OnLoginSuccess);
        socketIOComponent.On("return result", OnReturnResult);
    }
    public void SendSuggestion()
    {
        int suggestNumber = 0;
        try
        {
            suggestNumber = int.Parse(inputField.text);
        }
        catch(System.InvalidCastException)
        {
            suggestNumber = 0;
        }
        string data = JsonUtility.ToJson(new PlayerJSON(PlayerData.name,suggestNumber));
        socketIOComponent.Emit("request suggest",new JSONObject(data));
    }
    void OnLoginSuccess(SocketIOEvent socketIOEvent)
    {
        string data = JsonUtility.ToJson(new PlayerJSON(PlayerData.name, -1));
        socketIOComponent.Emit("login success", new JSONObject(data));
    }
    void OnReturnResult(SocketIOEvent socketIOEvent)
    {
        var result = CheckJSON.CreateFromJSON(socketIOEvent.data.ToString());
        if (result.status)
        {
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
        }
        else
        {
            showTxt.text = "Your number is not correct";
            showTxt.color = Color.red;
        }
    }
}
