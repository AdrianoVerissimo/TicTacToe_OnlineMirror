using UnityEngine;
using UnityEngine.UI;

public class EnterNameScreen : MonoBehaviour
{
    private const string PlayerPrefsNameKey = "PlayerName";

    [SerializeField] private string nameErrorDescription;
    [SerializeField] private Text descriptionText;
    [SerializeField] private InputField nameInput;

    private void Start()
    {
        SetupInputField();
    }

    private void OnEnable()
    {
        ClearDescriptionText();
    }

    public void TryConnect()
    {
        bool isValid = ValidateFields();
        if (!isValid)
        {
            SetDescriptionText(nameErrorDescription);
            return;
        }

        SavePlayerName();

        LobbyController.ShowConnectionScreen();
    }
    public bool ValidateFields()
    {
        string typedName = nameInput.text;
        bool isValid = false;

        isValid = !string.IsNullOrEmpty(typedName);

        return isValid;
    }

    public void SetDescriptionText(string text)
    {
        descriptionText.text = text;
    }
    public void ClearDescriptionText()
    {
        SetDescriptionText(" ");
    }

    private void SetupInputField()
    {
        bool hasName = PlayerPrefs.HasKey(PlayerPrefsNameKey);
        if (!hasName)
            return;

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInput.text = defaultName;
    }

    private void SavePlayerName()
    {
        string playerName = nameInput.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
