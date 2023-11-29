//<董静涛><zhangyue>
using UnityEngine;
using UnityEngine.UI;
using easyar;
using System;
using UnityEngine.Networking;
using System.Net;
using System.Text;
namespace Kanamori
{
    public class MapId
    {
        public int code;
        public string msg;
        public int data;

    }
    public class Map_Object
    {
        public int  mapId;
        public int objectId;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
    }
    public class Plane_Object
    {
        public int assetId;
        public int objectId;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        public float scaleX;
        public float scaleY;
        public float scaleZ;
    }
    public class UploadData
    {
        public int code;
        public string msg;
        public string data;
    }
    public class MapController : MonoBehaviour
    {

        /// <summary>
        /// 游戏控制
        /// </summary>
        private GameController game;
        public Button btnSave;
        /// <summary>
        /// 显示文本
        /// </summary>
        public Text text;
        public ARSession session;
        /// <summary>
        /// 稀疏空间工作框架
        /// </summary>
        public SparseSpatialMapWorkerFrameFilter mapWorker;
        /// <summary>
        /// 稀疏空间地图
        /// </summary>
        public SparseSpatialMapController map;
        public  static int mapzyid;
        void Start()
        {
            game = FindObjectOfType<GameController>();
            btnSave.interactable = false;
            session.WorldRootController.TrackingStatusChanged += OnTrackingStatusChanged;
            if (session.WorldRootController.TrackingStatus == MotionTrackingStatus.Tracking)
            {
                btnSave.interactable = true;
            }
            else
            {
                btnSave.interactable = false;
            }
        }
        /// <summary>
        /// 跟踪状态事件
        /// </summary>
        /// <param name="status"></param>
        private void OnTrackingStatusChanged(MotionTrackingStatus status)
        {
            if (status == MotionTrackingStatus.Tracking)
            {
                btnSave.interactable = true;
                text.text = "进入跟踪状态。";
            }
            else
            {
                btnSave.interactable = false;
                text.text = "跟踪状态异常";
            }
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        public void SaveMap()
        {
            btnSave.interactable = false;
            //地图保存结果反馈
            mapWorker.BuilderMapController.MapHost += (mapInfo, isSuccess, error) =>
            {
                if (isSuccess)
                {
                    game.SaveMapID(mapInfo.ID);
                    game.SaveMapName(mapInfo.Name);
                    text.text = "地图保存成功。";
                    string usrid = PlayerPrefs.GetString("userid"); ;
                    string esyid = mapInfo.ID;
                    string esynm = mapInfo.Name;
                    Debug.Log("按任意键发送数据到服务端");
                    var wc = new WebClient();
                    var url = "http://47.93.242.88:8080/scene/getMapId/" + usrid + "/" + esyid + "/" + esynm;
                    Debug.Log($"请求服务地址:{url}，时间：{DateTime.Now.ToString()}");
                    //模拟一个json数据发送到服务端
                    var jsonModel = "";
                    Debug.Log(jsonModel);
                    //发送到服务端并获得返回值
                    var returnInfo = wc.DownloadData(url);
                    //把服务端返回的信息转成字符串
                    var str = Encoding.UTF8.GetString(returnInfo);
                    Debug.Log($"服务端返回信息：{str},时间：{DateTime.Now.ToString()}");
                    Debug.Log("Json part:" + str);
                    MapId mapid = JsonUtility.FromJson<MapId>(str);
                    Debug.Log("mapid：" + mapid.data);
                    mapzyid = mapid.data;
                }
                else
                {
                    text.text = "地图保存出错：" + error;
                    btnSave.interactable = true;
                }
            };
            try
            {
                //保存地图
                mapWorker.BuilderMapController.Host(game.inputName, null);
                text.text = "开始保存地图，请稍等。";
            }
            catch (Exception ex)
            {
                text.text = "保存出错：" + ex.Message;
                btnSave.interactable = true;
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        public void Back()
        {
            if (game)
            {
                game.BackMenu();
            }
        }


    }
}

//</董静涛>