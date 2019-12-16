using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CredentialsHelper
{
    private static Dictionary<string, string> m_dictionary;

    public static string GetValue(string key)
    {
        if (m_dictionary == null)
        {
            InitDictionary();
        }

        return m_dictionary[key];
    }

    private static void InitDictionary()
    {
        TextAsset credsFile = Resources.Load("credentials") as TextAsset;

        string[] fLines = Regex.Split(credsFile.text, "\n |\r |\r\n");

        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string entry in fLines)
        {
            string[] pair = Regex.Split(entry, "=");
            dictionary[pair[0]] = pair[1];
        }

        m_dictionary = dictionary;
    }
}
