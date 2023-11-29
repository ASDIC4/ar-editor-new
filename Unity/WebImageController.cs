//<董静涛><zhangyue>
using easyar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static easyar.ImageTargetController;
using UnityEngine.UI;


namespace Kanamori
{
    public class WebImageController : MonoBehaviour
    {
        private GameController game;
        string imageURL;
        string bundleURL;
        private GameObject gb;
        public Transform cube;
        public Button btnadd;
        public Button btndel;
        // GameObject scriptTargetObject;
        public ImageTrackerFrameFilter tracker;
        public GameObject scenemaster;
        public Dropdown imageDropdown;
        public Dropdown modelDropdown;
        public ImageTargetController targetController;
        private IEnumerator coroutine;
        private Dictionary<string, AssetBundle> abCache = new Dictionary<string, AssetBundle>();
        public static string tmpIP = "http://47.93.242.88";
        public static string tmpPort = "8080";
        public static string imgGet = "/asset/search/app/pv/";
        public static string modelGet = "/asset/search/app/model/";
        public static string userId ;
        public List<int> image_assetIds = new List<int>();
        public List<int> image_userIds = new List<int>();
        public List<String> image_assetNames = new List<string>();
        public List<String> image_assetPaths = new List<string>();
        public List<String> image_assetTypes = new List<string>();
        public List<int> model_assetIds = new List<int>();
        public List<int> model_userIds = new List<int>();
        public List<String> model_assetNames = new List<string>();
        public List<String> model_assetPaths = new List<string>();
        public List<String> model_assetTypes = new List<string>();
        // Start is called before the first frame update
        void Start()
        {
            game = FindObjectOfType<GameController>();
            userId = PlayerPrefs.GetString("userid");
            string rawURL = tmpIP + ":" + tmpPort;
            string imgQuery = rawURL + imgGet + userId;
            string modelQuery = rawURL + modelGet + userId;
            Debug.Log("rawURL:" + rawURL);
            Debug.Log("imgQuery:" + imgQuery);
            Debug.Log("modelQuery:" + modelQuery);

            StartCoroutine(GetImageResponse(imgQuery));
            StartCoroutine(GetModelResponse(modelQuery));
        }
        public void Delete()
        {

            RemoveAllChildren(scenemaster);
            Debug.Log("Successful Delete" + Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL)));
            abCache[Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL))].Unload(false);
            abCache.Remove(Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL)));
            Destroy(targetController);
            StopCoroutine(coroutine);
            scenemaster.SetActive(true);
            btnadd.enabled = true;
            btnadd.interactable = true;
            btndel.enabled = false;
            btndel.interactable = false;

        }
        public static void RemoveAllChildren(GameObject parent)
        {
            Transform transform;
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                transform = parent.transform.GetChild(i);
                GameObject.Destroy(transform.gameObject);
            }
        }
        IEnumerator GetImageResponse(string URL)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL);
            yield return uwr.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(uwr.error))
            {
                Debug.LogWarning($"get {URL} Error: {uwr.error}");
                Debug.Log(uwr.downloadHandler.text);
                yield break;
            }
            else
            {
                string json = uwr.downloadHandler.text;

                Debug.Log(json);

                int idx1 = 0;
                String tmp = json;

                do
                {
                    idx1 = tmp.IndexOf("assetId");

                    int idx2 = tmp.IndexOf("userId");
                    int assetId = int.Parse(tmp.Substring(idx1 + 9, idx2 - idx1 - 11));

                    int idx3 = tmp.IndexOf("assetName");
                    int userId = int.Parse(tmp.Substring(idx2 + 8, idx3 - idx2 - 10));

                    int idx4 = tmp.IndexOf("assetPath");
                    string assetName = tmp.Substring(idx3 + 12, idx4 - idx3 - 15);

                    int idx5 = tmp.IndexOf("assetType");
                    string assetPath = tmp.Substring(idx4 + 12, idx5 - idx4 - 15);

                    int idx6 = tmp.IndexOf("}");
                    string assetType = tmp.Substring(idx5 + 12, idx6 - idx5 - 13);
                    idx1 = idx6 + 1;
                    if (assetType == "picture")
                    {
                        image_assetIds.Add(assetId);
                        image_userIds.Add(userId);
                        image_assetNames.Add(assetName);
                        image_assetPaths.Add(assetPath);
                        image_assetTypes.Add(assetType);
                    }
                    tmp = tmp.Substring(idx1);
                } while (tmp.IndexOf("assetPath") != -1);


                List<Dropdown.OptionData> imageOptions = new List<Dropdown.OptionData>();

                foreach (string assetName in image_assetNames)
                {
                    imageOptions.Add(new Dropdown.OptionData(assetName));
                }
                imageDropdown.options = imageOptions;
            }
        }
        public void send4()
        {

            //StopAllCoroutines();
            StartCoroutine(Save());
            //StopAllCoroutines();
        }
        IEnumerator Save()
        {
            
            Plane_Object plane_object = new Plane_Object();
            plane_object.assetId = image_assetIds[imageDropdown.value];
             plane_object.objectId = model_assetIds[modelDropdown.value];
            
            // map_object.objectId = dynamicObject.objectId;

            plane_object.rotationX = gb.transform.localRotation[0];
            plane_object.rotationY = gb.transform.localRotation[1];
            plane_object.rotationZ = gb.transform.localRotation[2];
            plane_object.positionX = gb.transform.localPosition[0];
            plane_object.positionY = gb.transform.localPosition[1];
            plane_object.positionZ = gb.transform.localPosition[2];
            plane_object.scaleX = gb.transform.localScale[0];
            plane_object.scaleY = gb.transform.localScale[1];
            plane_object.scaleZ = gb.transform.localScale[2];
            string url_upload = "http://47.93.242.88:8080/plane/upload/" + plane_object.assetId + "/" + plane_object.objectId.ToString() + "/" +
     plane_object.positionX.ToString() + "/" + plane_object.positionY.ToString() + "/" + plane_object.positionZ.ToString() + "/" + plane_object.rotationX.ToString() +
    "/" + plane_object.rotationY.ToString() + "/" + plane_object.rotationZ.ToString() + "/" + plane_object.scaleX.ToString() + "/" + plane_object.scaleY.ToString() + "/" + plane_object.scaleZ.ToString() + "/";
            Debug.Log("url_upload " + url_upload);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url_upload, ""))
            {
                // webRequest.SetRequestHeader("Content-Type", "application/json");
                // webRequest.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
                // webRequest.SetRequestHeader("Connection", "keep-alive");
                //Debug.Log("webRequest "+webRequest.ToString());
                Debug.Log("wbRequest " + webRequest.ToString());
                yield return webRequest.SendWebRequest();



                if (!string.IsNullOrWhiteSpace(webRequest.error))
                {
                    Debug.Log("Failed");
                    yield break;
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log("Json part:");
                    Debug.Log(jsonResponse);
                    // 解析JSON
                    UploadData userData = JsonUtility.FromJson<UploadData>(jsonResponse);

                    if (userData != null)
                    {
                        Debug.Log("Code: " + userData.code);
                        Debug.Log("Msg: " + userData.msg);

                        if (userData.code == 0) // Assuming 0 means success
                        {
                            Debug.Log("data: " + userData.data);

                        }
                        else
                        {
                            Debug.LogError("Login failed: " + userData.msg);

                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to parse JSON");
                    }
                }
            }
        }
        public void Back()
        {
            if (game)
            {
                game.BackMenu();
            }
        }
        IEnumerator GetModelResponse(string URL)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(URL);
            yield return uwr.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(uwr.error))
            {
                Debug.LogWarning($"get {URL} Error: {uwr.error}");
                Debug.Log(uwr.downloadHandler.text);
                yield break;
            }
            else
            {
                string json = uwr.downloadHandler.text;

                Debug.Log(json);

                int idx1 = 0;
                String tmp = json;

                do
                {
                    idx1 = tmp.IndexOf("assetId");

                    int idx2 = tmp.IndexOf("userId");
                    int assetId = int.Parse(tmp.Substring(idx1 + 9, idx2 - idx1 - 11));

                    int idx3 = tmp.IndexOf("assetName");
                    int userId = int.Parse(tmp.Substring(idx2 + 8, idx3 - idx2 - 10));

                    int idx4 = tmp.IndexOf("assetPath");
                    string assetName = tmp.Substring(idx3 + 12, idx4 - idx3 - 15);

                    int idx5 = tmp.IndexOf("assetType");
                    string assetPath = tmp.Substring(idx4 + 12, idx5 - idx4 - 15);

                    int idx6 = tmp.IndexOf("}");
                    string assetType = tmp.Substring(idx5 + 12, idx6 - idx5 - 13);
                    idx1 = idx6 + 1;
                    if (assetType == "model")
                    {
                        model_assetIds.Add(assetId);
                        model_userIds.Add(userId);
                        model_assetNames.Add(assetName);
                        model_assetPaths.Add(assetPath);
                        model_assetTypes.Add(assetType);
                    }
                    tmp = tmp.Substring(idx1);
                } while (tmp.IndexOf("assetPath") != -1);


                List<Dropdown.OptionData> modelOptions = new List<Dropdown.OptionData>();

                foreach (string assetName in model_assetNames)
                {
                    modelOptions.Add(new Dropdown.OptionData(assetName));
                }
                modelDropdown.options = modelOptions;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Add()
        {
            string rawURL = tmpIP + ":" + tmpPort + "/asset/";
            string imgPath = image_assetPaths[imageDropdown.value];
            string modelPath = model_assetPaths[modelDropdown.value];
            imageURL = rawURL + imgPath;
            bundleURL = rawURL + modelPath;
            coroutine = LoadImage();
            StartCoroutine(coroutine);
            btnadd.enabled = false;
            btnadd.interactable = false;
            btndel.enabled = true;
            btndel.interactable = true;
        }

        IEnumerator LoadImage()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(imageURL);
            yield return uwr.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(uwr.error))
            {
                Debug.LogWarning($"get {imageURL} Error: {uwr.error}");
                Debug.Log(uwr.downloadHandler.text);
                yield break;
            }
            else
            {
                byte[] data = uwr.downloadHandler.data;

                // 从URL中提取文件名
                string fileName = Path.Combine("image/", Path.GetFileName(imageURL));

                // 将数据保存到本地可持久化目录，使用提取的文件名
                SaveDataToLocalFile(data, fileName);

                Debug.Log($"Data {imageURL} received and saved successfully.");
            }

            easyar.GUIPopup.EnqueueMessage($"1111", 1);
            UnityWebRequest bundleUwr = UnityWebRequest.Get(bundleURL);
            yield return bundleUwr.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(bundleUwr.error))
            {
                Debug.LogWarning($"get {bundleURL} Error: {bundleUwr.error}");
                Debug.Log(bundleUwr.downloadHandler.text);
                yield break;
            }
            else
            {
                byte[] data = bundleUwr.downloadHandler.data;

                // 从URL中提取文件名

                string fileName = Path.Combine("bundle/", Path.GetFileName(bundleURL));
                // 将数据保存到本地可持久化目录，使用提取的文件名
                SaveBundleDataToLocalFile(data, fileName);

                Debug.Log($"Data {bundleURL} received and saved successfully.");
            }

            easyar.GUIPopup.EnqueueMessage($"2222", 1);
            Debug.Log("1111PathName" + Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL)));
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL)));
            abCache.Add(Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(bundleURL)), bundle);
            GameObject gbPrefab = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
             gb = Instantiate(gbPrefab);
            gb.gameObject.AddComponent<GestureControl>();
            gb.gameObject.AddComponent<MoveControl>();
            gb.gameObject.AddComponent<RotateControl>();
            gb.gameObject.AddComponent<ZoomControl>();
            gb.transform.SetParent(transform, false);
            gb.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            var targetController = gameObject.AddComponent<ImageTargetController>();

            Debug.Log(targetController);

            targetController.ActiveControl = TargetController.ActiveControlStrategy.HideWhenNotTracking;
            targetController.SourceType = DataSource.ImageFile;
            targetController.ImageFileSource = new ImageFileSourceData()
            {
                PathType = PathType.Absolute,
                Path = Path.Combine(Application.persistentDataPath, "image/", Path.GetFileName(imageURL)),
                Name = "image",
                Scale = 0.2f,
            };
            targetController.Tracker = tracker;
            targetController.TargetLoad += (Target target, bool status) =>
            {
                Debug.Log(target.name());
                Debug.Log(status);
                easyar.GUIPopup.EnqueueMessage($"{target.name()},{status}", 1);
            // scriptTargetObject.SetActive(true);
        };
            targetController.TargetUnload += (Target target, bool status) =>
            {
                Debug.Log(target);
                Debug.Log(status);
                easyar.GUIPopup.EnqueueMessage($"{target.name()},{status}", 1);
            };
        }
        void SaveDataToLocalFile(byte[] data, string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllBytes(path, data);
            Debug.Log($"save to {path} successfully");
        }
        void SaveBundleDataToLocalFile(byte[] data, string fileName)
        {

            string path = Path.Combine(Application.persistentDataPath, fileName);


            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllBytes(path, data);
            Debug.Log($"save to {path} successfully");
        }
    }
}
//<董静涛>