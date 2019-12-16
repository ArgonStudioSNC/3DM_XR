using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;

public class ChristmasPopupScript : MonoBehaviour
{
    public Transform getCardButton;
    public Transform addressForm;
    public Text editFieldText;


    private Text m_messageText;
    private Vector3 m_originalPosition;
    private bool m_isAddressFieldShown = false;


    protected void Awake()
    {
        if (DateTime.Now > new DateTime(2020, 2, 1)) SkipChristmas();
    }

    protected void Start()
    {
        m_messageText = getCardButton.GetComponentInChildren<Text>();
        m_originalPosition = transform.localPosition;
    }

    protected void OnGUI()
    {
        transform.localPosition = m_originalPosition;
        if (m_isAddressFieldShown)
        {
            TouchScreenKeyboard touchScreenKeyboard = addressForm.GetComponent<InputField>().touchScreenKeyboard;
            if (touchScreenKeyboard != null) editFieldText.text = touchScreenKeyboard.text;

            int keyboardHeight = MobileUtilities.GetKeyboardHeight(false);
            if (keyboardHeight > 0)
            {
                Button button = addressForm.GetComponentInChildren<Button>();
                float sendButtonPosY = button.transform.position.y - (button.transform.GetComponent<RectTransform>().rect.height + 20);
                transform.localPosition = new Vector3(transform.localPosition.x, keyboardHeight - sendButtonPosY, transform.localPosition.z);
            }
        }
    }


    public void ToggleEditField()
    {
        if (m_isAddressFieldShown)
        {
            m_messageText.text = "Je n'ai pas reçu la carte de vœux.";
            addressForm.gameObject.SetActive(false);
        }
        else
        {
            m_messageText.text = "Entrez votre adresse complète.";
            addressForm.gameObject.SetActive(true);
            addressForm.GetComponent<InputField>().ActivateInputField();
        }

        m_isAddressFieldShown = !m_isAddressFieldShown;
    }

    public void SkipChristmas()
    {
        SceneManager.LoadScene("3DMXR-Menu");
    }

    public void SendMail()
    {
        try
        {
            string message = buildMail();

            sendMail("contact@argonstudio.ch", "Demande de carte de Noël", message);
        }
        catch (MissingFieldException e)
        {
            Debug.Log(e);
            m_messageText.text = e.Message;
        }
        catch (SmtpException e)
        {
            m_messageText.text = "L'envoie à échoué (" + e.StatusCode + ")";
        }

        ToggleEditField();
        m_messageText.text = "Merci. Vous allez reçevoir la carte dans les prochains jours.";
    }


    private string buildMail()
    {
        string message = "<!DOCTYPE html><html lang='fr'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='x-apple-disable-message-reformatting'><title>3DM-XR Christmas 2019</title></head>";
        message = string.Concat(message, "<body><p>La personne suivante souhaite une carte de Noël<p>");

        if (editFieldText.text == "")
        {
            throw new MissingFieldException("Adresse requise");
        }
        message = string.Concat(message, "<p>", editFieldText.text, "</p>");

        message = string.Concat(message, "</body></html>");
        return message;
    }

    private void sendMail(string recipient, string subject, string body)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(CredentialsHelper.GetValue("mail"));
        mail.To.Add(recipient);
        mail.Subject = subject;
        mail.Body = body;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;

        SmtpClient smtpServer = new SmtpClient(CredentialsHelper.GetValue("server"));
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(CredentialsHelper.GetValue("mail"), CredentialsHelper.GetValue("password")) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("Mail successfully send");
    }
}
