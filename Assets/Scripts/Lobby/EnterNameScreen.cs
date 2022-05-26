using UnityEngine;
using UnityEngine.UI;

public class EnterNameScreen : MonoBehaviour
{
    [SerializeField] private string nameErrorDescription;
    [SerializeField] private Text descriptionText;
    [SerializeField] private InputField nameInput;

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
}
