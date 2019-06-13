using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Logic
{
    public class DataManger
    {
        private static DataManger sInstance = null;

        //用户名称
        public string m_UserName = null;

        //用户答题得分
        public int mUserScore = 0;
        //用户答题数量
        public int mAnsweredQuesntionNumber = 0;
        //答题准确率
        public float mAnsweredAccuary = 0.0f;

    
    //用户答题信息

        public static DataManger getInstance()
        {
            if (sInstance == null)
            {
                sInstance = new DataManger();
            }
            return sInstance;
        }

        //设置用户状态
        public void SetUserState(string str)
        {
        }
    }
}