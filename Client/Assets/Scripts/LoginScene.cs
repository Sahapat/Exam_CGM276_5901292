using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class LoginScene : MonoBehaviour
{
    [SerializeField]InputField InputField = null;
    public void Login()
    {
        try
        {
            PlayerData.name = (InputField.text != string.Empty)?InputField.text:"Anonymouse";
        }
        catch(System.NullReferenceException)
        {
            PlayerData.name = "Anonymouse";
        }
        SceneManager.LoadScene("Main");
    }
}
