/*
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using UnityEngine;

namespace MistplaySDK
{
#if UNITY_2018_1_OR_NEWER
    public class MistplayPostBuildProcess_Android : UnityEditor.Android.IPostGenerateGradleAndroidProject
#else
    public class Mistplay_PostprocessBuild
#endif
    {

#if UNITY_2018_1_OR_NEWER
        public int callbackOrder
        {
            get { return 1; }
        }

#endif


        
         // Example taken from https://github.com/Over17/UnityAndroidManifestCallback
         

        public void OnPostGenerateGradleAndroidProject(string basePath)
        {
            // If needed, add condition checks on whether you need to run the modification routine.
            // For example, specific configuration/app options enabled

           var GoogleAdMobIds = Resources.Load<GoogleAdMobIds>("GoogleAdMobIds");

            string AdMobIDAndroid = GoogleAdMobIds.AdMobIDAndroid; 
            if (AdMobIDAndroid !=null && !AdMobIDAndroid.Equals(""))
            {
                var androidManifest = new AndroidManifest(GetManifestPath(basePath));

                Debug.Log("SETTING GOOGLE APP ID");
                androidManifest.SetGoogleApplicationID(AdMobIDAndroid);
                // Add your XML manipulation routines

                androidManifest.Save();

                Debug.Log("New Manifest is saved");
            }
        }

        private string _manifestFilePath;

        private string GetManifestPath(string basePath)
        {
            if (string.IsNullOrEmpty(_manifestFilePath))
            {
                var pathBuilder = new StringBuilder(basePath);
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
                pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
                _manifestFilePath = pathBuilder.ToString();
            }
            return _manifestFilePath;
        }
    }


    internal class AndroidXmlDocument : XmlDocument
    {
        private string m_Path;
        protected XmlNamespaceManager nsMgr;
        public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
        public AndroidXmlDocument(string path)
        {
            m_Path = path;
            using (var reader = new XmlTextReader(m_Path))
            {
                reader.Read();
                Load(reader);
            }
            nsMgr = new XmlNamespaceManager(NameTable);
            nsMgr.AddNamespace("android", AndroidXmlNamespace);
        }

        public string Save()
        {
            return SaveAs(m_Path);
        }

        public string SaveAs(string path)
        {
            using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                Save(writer);
            }
            return path;
        }
    }


    internal class AndroidManifest : AndroidXmlDocument
    {
        private readonly XmlElement ApplicationElement;

        public AndroidManifest(string path) : base(path)
        {
            ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal XmlNode GetActivityWithLaunchIntent()
        {
            return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                    "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
        }

        internal void SetApplicationTheme(string appTheme)
        {
            ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
        }

        internal void SetStartingActivityName(string activityName)
        {
            GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
        }

        internal void SetGoogleApplicationID(string id)
        {
            var gadAppIdNode = SelectSingleNode("//application//meta-data[@android:name='com.google.android.gms.ads.APPLICATION_ID']/@android:value", nsMgr);
            gadAppIdNode.Value = id;
        }


    }

}
*/