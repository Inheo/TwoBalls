
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;

namespace MistplaySDK
{

    public class MistplayPostBuildProcess_IOS : MonoBehaviour
    {
#if UNITY_IOS
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget,
                                          string path)
    {

    /*
       var GoogleAdMobIds = Resources.Load<GoogleAdMobIds>("GoogleAdMobIds");

        string appId = GoogleAdMobIds.AdMobIdIOS; 

        Debug.Log("SETTING APPID TO " + appId);

        string plistPath = Path.Combine(path, "Info.plist");
        PlistDocument plist = new PlistDocument();

        plist.ReadFromFile(plistPath);

        if(appId!= null && !appId.Equals(""))
        {
                plist.root.SetString("GADApplicationIdentifier", appId);
        }

            */
        /*

        REMOVING THIS SINCE APPLOVIN HANDLES IT

        PlistElementDict rootDict = plist.root;

        string key = "SKAdNetworkItems";

        PlistElementArray array = rootDict.CreateArray(key);

        string SKAdNetworkIdentifier = "SKAdNetworkIdentifier";


        TextAsset SkAdNetworkXMLFile = Resources.Load<TextAsset>("SKADNetworks");

        XmlDocument SKAdNetworksXMLDoc = new XmlDocument();

        SKAdNetworksXMLDoc.LoadXml(SkAdNetworkXMLFile.text);

        XmlNodeList SKAdNetworkStrings = SKAdNetworksXMLDoc.GetElementsByTagName("string");

        PlistElementDict dict;

        foreach (XmlNode SKAdNetworkString in SKAdNetworkStrings)
        {
            print(SKAdNetworkString.InnerText);
            dict = array.AddDict();
            dict.SetString(SKAdNetworkIdentifier, SKAdNetworkString.InnerText);
        }

        */


            /*
        // Set the IDFA request description:
        const string k_TrackingDescription = "Your data will be used to provide you a better and personalized ad experience.";

        // Set the description key-value in the plist:
        plist.root.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);

        /*
        PlistElementDict dict = array.AddDict();

        dict.SetString(SKAdNetworkIdentifier, "22mmun2rn5.skadnetwork");

        dict = array.AddDict();
        dict.SetString(SKAdNetworkIdentifier, "238da6jt44.skadnetwork");
        */


            //commenting since Applovin handles this
        // File.WriteAllText(plistPath, plist.WriteToString());
            
    }
#endif
    }

}