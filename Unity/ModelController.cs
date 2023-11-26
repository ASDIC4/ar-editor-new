//<董静涛>
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using easyar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Globalization;
using UnityEngine.SceneManagement;
namespace Kanamori
{
    
    public class ModelController : MonoBehaviour
    {
        /// <summary>
        /// 游戏控制
        /// </summary>
        private GameController game;
        /// <summary>
        /// 摄像头前方
        /// </summary>
        public Transform frontCamera;
        /// <summary>
        /// 添加界面
        /// </summary>
        public GameObject addUI;
        public Button btnAdd;
        /// <summary>
        /// 保存界面
        /// </summary>
        public GameObject saveUI;
        /// <summary>
        /// UI控制游戏对象
        /// </summary>
        private UIControlObject uiControl;
        /// <summary>
        /// 显示信息文本
        /// </summary>
        public Text textShow;
        public SparseSpatialMapWorkerFrameFilter mapWorker;
        public SparseSpatialMapController map;
        /// <summary>
        /// 稀疏空间地图
        /// </summary>
        public Transform ssMap;
        /// <summary>
        /// 添加的物体
        /// </summary>
       // ------------------------------------------------
        /// 添加的物体
        public GameObject blueBox;
        public GameObject gbPrefab;
        public Dropdown mapDropdown;
        public static string tmpIP = "http://47.93.242.88";
        public static string tmpPort = "8080";
        public static string modelGet = "/asset/search/app/model/";
        public static string userId ;
        public List<int> assetIds = new List<int>();
        public List<int> userIds = new List<int>();
        public List<String> assetNames = new List<string>();
        public List<String> assetPaths = new List<string>();
        public List<String> assetTypes = new List<string>();
        private Dictionary<string, AssetBundle> abCache = new Dictionary<string, AssetBundle>();
        string modelURL;
        
        // ------------------------------------------------
        //public int mapzyid;
        void Start()
        {
            game = FindObjectOfType<GameController>();
            uiControl = FindObjectOfType<UIControlObject>();
            btnAdd.interactable = false;
            userId = PlayerPrefs.GetString("userid");
            // ------------------------------------------------
            Initialize();
            // ------------------------------------------------
            Close();
            Load();
            LoadMap();
        }
        /// <summary>
        /// 本地化地图
        /// </summary>
        private void LoadMap()
        {
            //设置地图
            map.MapManagerSource.ID = game.GetMapID();
            map.MapManagerSource.Name = game.GetMapName();
            //地图获取反馈
            map.MapLoad += (map, status, error) =>
            {
                if (status)
                {
                    textShow.text = "地图加载成功。";
                }
                else
                {
                    textShow.text = "地图加载失败：" + error;
                }
            };
            //定位成功事件
            map.MapLocalized += () =>
            {
                textShow.text = "稀疏空间定位成功。";
                btnAdd.interactable = true;
            };
            //停止定位事件
            map.MapStopLocalize += () =>
            {
                textShow.text = "停止稀疏空间定位。";
                btnAdd.interactable = false;
            };
            textShow.text = "开始本地化稀疏空间。";
            mapWorker.Localizer.startLocalization();    //本地化地图
        }

        /// <summary>
        /// 返回菜单
        /// </summary>
        public void Back()
        {
            if (game)
            {
                game.BackMenu();
            }
        }
        /// <summary>
        /// 添加物体
        /// </summary>
        public void Add()
        {
            // var tf = Instantiate(blueBox, ssMap).transform;
            // ------------------------------------------------
            QueryModel();
            if(gbPrefab == null) { Debug.Log("GbPrefab is null in add!"); }
            //var tf = Instantiate(gbPrefab, ssMap).transform;
            // ------------------------------------------------
           // tf.gameObject.AddComponent<GestureControl>();
          //  tf.gameObject.AddComponent<ZoomControl>();
          //  tf.gameObject.AddComponent<RotateControl>();
           // tf.gameObject.AddComponent<MoveControl>();
           // tf.position = frontCamera.position;
        }
        // ------------------------------------------------
        /// <summary>
        /// 从服务器获取模型
        /// </summary>
        public void QueryModel()
        {
            string rawURL = tmpIP + ":" + tmpPort + "/asset/";
            modelURL = rawURL + mapDropdown.options[mapDropdown.value].text;
            StartCoroutine(LoadModel());
        }
        IEnumerator LoadModel()
        {
            UnityWebRequest bundleUwr = UnityWebRequest.Get(modelURL);
            yield return bundleUwr.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(bundleUwr.error))
            {
                Debug.LogWarning($"get {modelURL} Error: {bundleUwr.error}");
                Debug.Log(bundleUwr.downloadHandler.text);
                yield break;
            }
            else
            {
                byte[] data = bundleUwr.downloadHandler.data;

                // 从URL中提取文件名

                string fileName = Path.Combine("bundle/", Path.GetFileName(modelURL));
                // 将数据保存到本地可持久化目录，使用提取的文件名
                SaveBundleDataToLocalFile(data, fileName);

                Debug.Log($"Data {modelURL} received and saved successfully.");
            }

            Debug.Log("1111PathName" + Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(modelURL)));
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(modelURL)));
            //abCache.Add(Path.Combine(Application.persistentDataPath, "bundle/", Path.GetFileName(modelURL)), bundle);
            gbPrefab = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
            var tf = Instantiate(gbPrefab, ssMap).transform;
            tf.gameObject.AddComponent<GestureControl>();
            tf.gameObject.AddComponent<ZoomControl>();
            tf.gameObject.AddComponent<RotateControl>();
            tf.gameObject.AddComponent<MoveControl>();
            tf.position = frontCamera.position;
            tf.name = assetIds[mapDropdown.value].ToString();
            Debug.Log("gbPrefab is successful loaded!");
            bundle.Unload(true);
        }
        /// <summary>
        /// 保存模型至本地
        /// </summary>
        void SaveBundleDataToLocalFile(byte[] data, string fileName)
        {

            string path = Path.Combine(Application.persistentDataPath, fileName);


            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllBytes(path, data);
            Debug.Log($"save to {path} successfully");
        }
        /// <summary>
        /// 初始化选项
        /// </summary>
        public void Initialize()
        {
            string rawURL = tmpIP + ":" + tmpPort;
            string modelQuery = rawURL + modelGet + userId;
            Debug.Log("rawURL:" + rawURL);
            Debug.Log("modelQuery:" + modelQuery);

            StartCoroutine(GetModelResponse(modelQuery));
        }
        /// <summary>
        /// 获取模型路径
        /// </summary>
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
                        assetIds.Add(assetId);
                        userIds.Add(userId);
                        assetNames.Add(assetName);
                        assetPaths.Add(assetPath);
                        assetTypes.Add(assetType);
                    }
                    tmp = tmp.Substring(idx1);
                } while (tmp.IndexOf("assetPath") != -1);


                List<Dropdown.OptionData> modelOptions = new List<Dropdown.OptionData>();

                foreach (string assetName in assetNames)
                {
                    modelOptions.Add(new Dropdown.OptionData(assetName));
                }
                mapDropdown.options = modelOptions;
            }
        }
        // ------------------------------------------------
        /// <summary>
        /// 关闭保存界面
        /// </summary>
        public void Close()
        {
            addUI.SetActive(true);
            saveUI.SetActive(false);
        }
        void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0)
                && !EventSystem.current.IsPointerOverGameObject())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    TouchedObject(ray);
                }
            }
            else
            {
                if (Input.touchCount == 1
                && Input.touches[0].phase == TouchPhase.Began
                && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    TouchedObject(ray);
                }
            }
        }
        /// <summary>
        /// 点击物体处理
        /// </summary>
        /// <param name="ray"></param>
        private void TouchedObject(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                addUI.SetActive(false);
                saveUI.SetActive(true);
                uiControl.SetSelected(hit.transform);
                textShow.text = "选中物体";
            }
        }

        // void Update()
        // {
        //     if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         if (Physics.Raycast(ray, out RaycastHit hit))
        //         {
        //             addUI.SetActive(false);
        //             saveUI.SetActive(true);
        //         }
        //     }
        // }

        /// <summary>
        /// 保存物体
        /// </summary>
        /// 
        public void send3()
        {
            //StopAllCoroutines();
            StartCoroutine(Save());
            //StopAllCoroutines();
        }
        IEnumerator Save()
        {
            string[] jsons = new string[ssMap.childCount - 1];
            for (int i = 0; i < ssMap.childCount; i++)
            {
                if (ssMap.GetChild(i).name != "PointCloudParticleSystem")
                {
                    DynamicObject dynamicObject = new DynamicObject();
                    dynamicObject.objectId = ssMap.GetChild(i).name;
                    dynamicObject.position = ssMap.GetChild(i).localPosition;
                    dynamicObject.rotation = ssMap.GetChild(i).localEulerAngles;
                    dynamicObject.scale = ssMap.GetChild(i).localScale;

                    jsons[i - 1] = JsonUtility.ToJson(dynamicObject);

                    //zy
                    Map_Object map_object = new Map_Object();
                    map_object.mapId =  MapController.mapzyid;
                    map_object.objectId = int.Parse(dynamicObject.objectId);
                    // map_object.objectId = dynamicObject.objectId;
                    Debug.Log("Object Int Id=========::::"+int.Parse(dynamicObject.objectId));
                    map_object.rotationX = dynamicObject.rotation[0];
                    map_object.rotationY = dynamicObject.rotation[1];
                    map_object.rotationZ = dynamicObject.rotation[2];
                    map_object.positionX = dynamicObject.position[0];
                    map_object.positionY = dynamicObject.position[1];
                    map_object.positionZ = dynamicObject.position[2];
                    map_object.scaleX = dynamicObject.scale[0];
                    map_object.scaleY = dynamicObject.scale[1];
                    map_object.scaleZ = dynamicObject.scale[2];
                    string url_upload = "http://47.93.242.88:8080/scene/upload/" + map_object.mapId + "/" + map_object.objectId.ToString() + "/" +
             map_object.positionX.ToString() + "/" + map_object.positionY.ToString() + "/" + map_object.positionZ.ToString() + "/" + map_object.rotationX.ToString() +
            "/" + map_object.rotationY.ToString() + "/" + map_object.rotationZ.ToString() + "/" + map_object.scaleX.ToString() + "/" + map_object.scaleY.ToString() + "/" + map_object.scaleZ.ToString();
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
            }
            if (game)
            {
                game.SaveDynamicObject(jsons);
                textShow.text = "保存" + (ssMap.childCount - 1) + "个游戏对象";
            }
        }
        /// <summary>
        /// 删除物体
        /// </summary>
        public void Delete()
        {
            var go = uiControl.selected.gameObject;
            uiControl.ClearSelected();
            Destroy(go);
            textShow.text = "删除选中物体，请保存结果。";
        }
        /// <summary>
        /// 加载物体
        /// </summary>
        private void Load()
        {
            if (game)
            {
                var list = game.LoadDynamicObject();
                foreach (var item in list)
                {
                    var dynamicObject = JsonUtility.FromJson<DynamicObject>(item);
                     var tf = Instantiate(blueBox, ssMap).transform;
                   // var tf = Instantiate(gbPrefab, ssMap).transform;
                    tf.localPosition = dynamicObject.position;
                    tf.localEulerAngles = dynamicObject.rotation;
                    tf.localScale = dynamicObject.scale;
                }
            }
        }
    }
}

//</董静涛>