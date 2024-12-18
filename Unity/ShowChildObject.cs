﻿//<董静涛>
using UnityEngine;

namespace Kanamori
{
    public class ShowChildObject : MonoBehaviour
    {
        /// <summary>
        /// 进入事件
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            SetVisible(true);
        }
        /// <summary>
        /// 退出事件
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            SetVisible(false);
        }
        /// <summary>
        /// 设置子元素是否可见
        /// </summary>
        /// <param name="status">状态</param>
        public void SetVisible(bool status)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(status);
            }
        }
    }
}

//</董静涛>