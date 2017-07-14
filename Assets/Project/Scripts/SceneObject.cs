
using System.Collections.Generic;
using UnityEngine;

namespace MT
{
    public class SceneObject : MonoBehaviour
    {
         
        /// <summary>
        ///生成物体
        /// </summary>
        /// <typeparam name="T">生成类型</typeparam>
        /// <param name="obj">生成物体</param>
        public static Object AddObject(Object obj) 
        {
            Object o = Instantiate(obj);
            return o;
        }
        public static Object AddObject(Object obj, Transform parent) 
        {
            Object o = Instantiate(obj,parent);
            return o;
        }
        public static Object AddObject(Object obj, Transform parent, Vector3 position)
        {
            Object o = Instantiate(obj, position,Quaternion.identity, parent);
            return o;
        }
        public static Object AddObject(Object obj, Transform parent, Vector3 position, Vector3 angle)
        {
            Quaternion q = Quaternion.FromToRotation(Vector3.forward, angle);
            Object o =Instantiate(obj, position, q, parent);
            return o;
        }


        /// <summary>
        /// 根据名称返回查找物体（关闭物体无效）
        /// </summary>
        /// <param name="name">物体名字</param>
        /// <returns></returns>
        public static GameObject Find(string name)
        {
            GameObject g =null;
            Object[] obj = FindObjectsOfType<Object>();
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i].name == name)
                {
                    return g;
                }
            }
            return g;
        }
        /// <summary>
        /// 根据名称返回查找物体类型（关闭物体无效）
        /// </summary>
        /// <typeparam name="T">物体类型</typeparam>
        /// <param name="name">物体名字</param>
        /// <returns></returns>
        public static T Find<T>(string name)
        {
            T t =default(T);
            GameObject[] obj = FindObjectsOfType<GameObject>();
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i].name == name)
                {
                    t = obj[i].GetComponent<T>();
                    if (t != null)
                        return t;
                }
            }
            return t;
        }



        /// <summary>
        /// 根据名称返回查找物体（关闭物体无效）
        /// </summary>
        /// <param name="name">物体名字</param>
        /// <returns></returns>
        public static GameObject Find(Transform parent, string name)
        {
            GameObject g=null;
            Transform[] t = parent.GetComponentsInChildren<Transform>();
            for (int i = 0; i < t.Length; i++)
            {

                if (t[i].ToString().Substring(0, name.Length) == name)
                {
                    if (t[i].ToString().Substring(0, parent.name.Length) == parent.name)
                        continue;
                        g = t[i].gameObject;
                    return g;
                }
            }
            return g;
        }
        /// <summary>
        /// 根据名称返回查找物体类型（关闭物体无效）
        /// </summary>
        /// <typeparam name="T">物体类型</typeparam>
        /// <param name="name">物体名字</param>
        /// <returns></returns>
        public static T Find<T>(Transform _parent,string name)
        {
            T t = default(T);
            T[] Ts = _parent.GetComponentsInChildren<T>();
            for (int i = 0; i < Ts.Length; i++)
            {
                if (Ts[i].ToString().Substring(0, name.Length)==name)
                {
                    if (Ts[i].ToString().Substring(0, _parent.name.Length) == _parent.name)
                        continue;
                    t = Ts[i];
                    return t;
                }
            }
            return t;
        }  
        /// <summary>
           /// 根据名称返回查找物体类型（关闭物体无效）
           /// </summary>
           /// <typeparam name="T">物体类型</typeparam>
           /// <param name="name">物体名字</param>
           /// <returns></returns>
        public static List<T> Finds<T>(Transform _parent, string name)
        {
            List<T> t =new List<T>();
            T[] Ts=_parent.GetComponentsInChildren<T>();
            for (int i = 0; i < Ts.Length; i++)
            {
                if (Ts[i].ToString().Substring(0, _parent.name.Length) == _parent. name)
                    continue;
                if (Ts[i].ToString().Substring(0, name.Length) ==name)
                    t.Add(Ts[i]);
            }
            return t;
        }
        /// <summary>
        /// 根据名称返回查找物体类型（关闭物体无效）
        /// </summary>
        /// <typeparam name="T">物体类型</typeparam>
        /// <param name="name">物体名字</param>
        /// <returns></returns>
        public static List<T> Finds<T>(Transform _parent)
        {
            List<T> t = new List<T>();
            T[] Ts = _parent.GetComponentsInChildren<T>();
            for (int i = 0; i < Ts.Length; i++)
            {
                if (Ts[i].ToString().Substring(0, _parent.name.Length) == _parent.name)
                    continue;
                 t.Add(Ts[i]);
            }
            return t;
        }
    }
}
