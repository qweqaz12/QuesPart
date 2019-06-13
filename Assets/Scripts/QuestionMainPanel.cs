using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace UI
{
    public enum QuestionType
    {
        QuestionType_None,
        QuestionType_Single = 1,
        QuestionType_MultiChoice = 2,
        QuestionType_Judge = 3
    }

    //题目基类
    public class BaseQuestion
    {
        public QuestionType mQuestionType = QuestionType.QuestionType_None;

        public BaseQuestion(QuestionType type)
        {
            mQuestionType = type;
        }

        //获取最终用户答题结果信息
        public virtual UserAnswerInfoClass GetUserAnwserInfo()
        {
            UserAnswerInfoClass behaveInfo = new UserAnswerInfoClass();
            return behaveInfo;
        }

        //获取题目信息
        public virtual QuestionInfoClass GetQuestionInfo()
        {
            QuestionInfoClass questInfo = new QuestionInfoClass();
            return questInfo;
        }
    }

    //单选类
    public class SingleQuestion : BaseQuestion
    {

        public SingleQuestion():base(QuestionType.QuestionType_Single)
        {
            
        }
    }

    //多选类
    public class MultiQuestion : BaseQuestion
    {

        public MultiQuestion() : base(QuestionType.QuestionType_MultiChoice)
        {

        }
    }

    //判断题类
    public class JudgeQuestion : BaseQuestion
    {

        public JudgeQuestion() : base(QuestionType.QuestionType_Judge)
        {

        }
    }

    //题目类型
    public enum questionTypeEnum
    {
        singleChoice,
        multiChoice,
        judge
    }

    //题目工艺类型
    public enum questionProcessTypeEnum
    {
        none,
        Chemical = 1,
        Fire = 2,
        Booming = 3,
        safty = 4
    }

    //题目信息类
    public class QuestionInfoClass
    {
        public int questionId;
        public string questionEasyOrHard;
        public questionTypeEnum questionType;
        public Controller questionState;// 0 , 1 , 2(白，绿，黄)=>(未点击，已保存，已点击未保存)
        public int questionScore;
        public string questionDescrib;
        public float questionNeedTimer;
        public bool userIsAccuarcy;
        public questionProcessTypeEnum questionProcessType;


        public int userSelectedOption;
        public int[] trueAnwer;
    }

    //个人信息类别
    class UserInfoClass
    {
        string headIconUrl;
        string userId;
    }

    //结果信息
    public class UserAnswerInfoClass
    {
        public int answerNumber;
        public int getAllScore;
    }


    public class QuestionMainPanel : MonoBehaviour
    {
        GComponent mRootUI = null;
        
        //倒计时模块参数
        private bool mHasStartCountDown = false;
        private float mCountDownTimer = 50f;
        private float mRecordCountTimer = 0.0f;
        private GTextField mCountDownText = null;

        //本次临时答题题库List
        private List<QuestionInfoClass> mListQuestion = new List<QuestionInfoClass>();

        //当前题目号
        public int currentQuestioID;

        //个人信息模块初始化
        GLoader mHeadIconLoad = null;

        //题目显示部分初始化
        GList mQuestionInfoList = null;
        GComponent mQuestionDescrib = null;

        //题目作答部分初始化
        GComponent mAnswerAreaPanel = null;

        //控制器初始化
        Controller mAnswerAreaControll = null;

        public static GameObject CreatePanel()
        {
            GameObject panelObj = new GameObject("QuestionMainPanel");
            panelObj.layer = LayerMask.NameToLayer("UI");

            UIPanel panel = panelObj.AddComponent<UIPanel>();
            panel.packageName = "QuesPart";
            panel.componentName = "Component1";
            panel.fitScreen = FitScreen.FitSize;
            panel.SetSortingOrder(10, true);

            panel.CreateUI();

            UIContentScaler contentScaler = panelObj.AddComponent<UIContentScaler>();
            contentScaler.designResolutionX = 1136;
            contentScaler.designResolutionY = 640;
            contentScaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            contentScaler.screenMatchMode = UIContentScaler.ScreenMatchMode.MatchWidthOrHeight;

            QuestionMainPanel wp = panelObj.AddComponent<QuestionMainPanel>();

            return panelObj;
        }
        //awake test
        void Awake()
        {

            string m_ques = Http.Httpcon.getinstance().GetSingleQues("http://119.29.249.112:10087/api/item/SingleQuestion/getall", 50000);
            List<Pojo.SingleQuestion> ques = M_Utils.Utils.getinstance().parse<Pojo.SingleQuestion>(m_ques);
       
            for (int i = 0; i < ques.Count; i++)
            {
                QuestionInfoClass questionItem = new QuestionInfoClass();
                questionItem.questionDescrib =ques[i].Title;
                questionItem.questionEasyOrHard = "**";
                questionItem.questionType = i>5?questionTypeEnum.singleChoice:questionTypeEnum.multiChoice;
                questionItem.questionState = null;
                questionItem.questionId = i;
                questionItem.questionScore = int.Parse(ques[i].Score);
                questionItem.questionNeedTimer = 30;
                questionItem.trueAnwer = new int[] { 0,0,0,1};
                questionItem.questionProcessType = questionProcessTypeEnum.Fire;

                mListQuestion.Add(questionItem);
            }
        }
        //start
        void Start()
        {
            mRootUI = GetComponent<UIPanel>().ui;

            mQuestionDescrib = mRootUI.GetChild("QuestionDescribPanel").asCom;
            mAnswerAreaPanel = mRootUI.GetChild("AnswerAreaPanel").asCom;
            mAnswerAreaControll = mAnswerAreaPanel.GetController("QuestypeControll");

            mHeadIconLoad = mRootUI.GetChild("user_icon").asLoader;
            //头像右侧个人空间
            GList mHeadCenterlist = mRootUI.GetChild("userinfol_ist").asList;
            mHeadCenterlist.RemoveChildren();

            for(int i = 0 ; i < 5 ; i ++ )
            {
                GButton mHeadCenterListItem = mHeadCenterlist.AddItemFromPool().asButton;
                mHeadCenterListItem.text = "个人空间";
            }
           
            mHeadCenterlist.onClickItem.Add(OnHeadCenterListClick);

            //题目列表list
            GList mQuestionList = mRootUI.GetChild("id_list").asList;
            mQuestionList.RemoveChildren();
            for (int j = 0 ; j < mListQuestion.Count; j ++ )
            {
                GButton mQuestionListItem = mQuestionList.AddItemFromPool().asButton;
                mListQuestion[j].questionState = mQuestionListItem.GetController("btncolor");
                mQuestionListItem.text = j.ToString();
            }

            mQuestionList.onClickItem.Add(OnQuestionListItemClick);

            //按钮初始化
            mRootUI.GetChild("save_btn").asButton.onClick.Add(OnSaveBtnClick);
            mRootUI.GetChild("submit_btn").asButton.onClick.Add(OnSubmitBtnClick);
            mRootUI.GetChild("zoombtn").asButton.onClick.Add(OnZoomingBtnClick);
            mRootUI.GetChild("closebtn").asButton.onClick.Add(OnCloseBtnClick);

            //倒计时标志开始初始化
            mCountDownText = mRootUI.GetChild("timerlimit").asTextField;
            
            mHasStartCountDown = true;

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                mHeadIconLoad.url = "ui://atru89wocnep15";             
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.LogError("123");
            }
            //倒计时结束判断
            if(mHasStartCountDown)
            {
                mRecordCountTimer += Time.deltaTime;
                if (mRecordCountTimer > 1.0f)
                {
                    mRecordCountTimer = 0.0f;
                    mCountDownTimer -= 1.0f;
                    if(mCountDownTimer >= 0.0f)
                    {
                        mCountDownText.text = mCountDownTimer.ToString();
                    }
                    else
                    {
                        //结束面板发送数据
                        mHasStartCountDown = false;
                    }
                }
            }
        }

        //选择题号
        void OnQuestionListItemClick(EventContext context)
        {
            GButton itemObj = (GButton)context.data;
            currentQuestioID = int.Parse(itemObj.text);
            if(mListQuestion[currentQuestioID].questionState.selectedIndex != 1)
                mListQuestion[currentQuestioID].questionState.SetSelectedIndex(2);

            for (int j = 0; j < mListQuestion.Count; j++)
            {
                if(currentQuestioID == j)
                {
                    mQuestionDescrib.GetChild("question_text").text = mListQuestion[j].questionDescrib;//显示题目描述

                    switch(mListQuestion[j].questionType)//显示答题区域
                    {
                        case questionTypeEnum.judge:
                            mAnswerAreaControll.SetSelectedIndex(2);
                            break;
                        case questionTypeEnum.multiChoice:
                            mAnswerAreaControll.SetSelectedIndex(1);
                            break;
                        case questionTypeEnum.singleChoice:
                            mAnswerAreaControll.SetSelectedIndex(0);
                            break;
                    }
                }
            }
        }
        
        //倒计时初始化
        private float InitGetCountDownTimers(List<QuestionInfoClass> ts)
        {
            for(int i = 0;i < ts.Count; i++)
            {
                mCountDownTimer += ts[i].questionNeedTimer;
            }
            return mCountDownTimer;
        }

        //倒计时结束发送数据
        private void StopCountDownAndSend()
        {
            mHasStartCountDown = false;
            mCountDownTimer = 0.0f;
            mRecordCountTimer = 0.0f;

            UserAnswerInfoClass userInfoDatas = new UserAnswerInfoClass();
            userInfoDatas.answerNumber = 0;
            userInfoDatas.getAllScore = 0;
            //结果数据发送
        }

        //用户头像右侧按钮List
        void OnHeadCenterListClick(EventContext context)
        {
            GButton itemObj = (GButton)context.data;
            Debug.LogError(itemObj.text);
        }

        //关闭按钮
        void OnCloseBtnClick()
        {

        }

        //缩放按钮
        void OnZoomingBtnClick()
        {

        }

        //保存按钮
        void OnSaveBtnClick()
        {
            mListQuestion[currentQuestioID].questionState.SetSelectedIndex(1);
            GComponent gc = null;
            bool isright = false;

            switch (mListQuestion[currentQuestioID].questionType)
            {
                case questionTypeEnum.judge:
                    gc = mAnswerAreaPanel.GetChild("n3").asCom;
                    int[] ii = new int[] { 1 };
                    isright = JudgeQuestionAcccuarry(gc, ii, questionTypeEnum.judge);
                    mListQuestion[currentQuestioID].userIsAccuarcy = isright;
                    Debug.Log(isright);
                    break;
                case questionTypeEnum.multiChoice:
                    gc = mAnswerAreaPanel.GetChild("n2").asCom;
                    int[] j = new int[] { 0, 1, 1, 1 };
                    isright = JudgeQuestionAcccuarry(gc, j, questionTypeEnum.multiChoice);
                    mListQuestion[currentQuestioID].userIsAccuarcy = isright;
                    Debug.Log(JudgeQuestionAcccuarry(gc, j, questionTypeEnum.multiChoice));
                    break;
                case questionTypeEnum.singleChoice:
                    gc = mAnswerAreaPanel.GetChild("n1").asCom;
                    int[] ii1 = new int[] { 1 };
                    isright = JudgeQuestionAcccuarry(gc, ii1, questionTypeEnum.singleChoice);
                    mListQuestion[currentQuestioID].userIsAccuarcy = isright;
                    break;
            }
        }

        //判断 正确错误
        bool JudgeQuestionAcccuarry(GComponent gc,int[] answer,questionTypeEnum questype)
        {
            if (questype == questionTypeEnum.multiChoice)
            {
                bool result = true;
                int[] choiceitem = new int[4];
                choiceitem[0] = gc.GetChild("A_choice").asButton.GetController("button").selectedIndex;
                choiceitem[1] = gc.GetChild("B_choice").asButton.GetController("button").selectedIndex;
                choiceitem[2] = gc.GetChild("C_choice").asButton.GetController("button").selectedIndex;
                choiceitem[3] = gc.GetChild("D_choice").asButton.GetController("button").selectedIndex;

                if (answer.Length != choiceitem.Length)
                    return false;
                for (int i = 0; i < answer.Length; i++)
                {
                    if (answer[i] != choiceitem[i])
                        return false;
                }
                return result;
            }
            else
            {
                int selectIndex = gc.GetController("radioncontrol").selectedIndex;
                mListQuestion[currentQuestioID].userSelectedOption = selectIndex;
                if (selectIndex != answer[0])
                    return false;
                return true;
            }
        }

        //提交按钮
        void OnSubmitBtnClick()
        {
            SaveCurrentUserAnwserInfo();//DateManger中存放已答题目数据
            Destroy(this.gameObject);
        }

        //结束存储答题情况
        void SaveCurrentUserAnwserInfo()
        {
            for(int i = 0; i < mListQuestion.Count; i++)
            {
                if (mListQuestion[i].userIsAccuarcy)
                {
                    Logic.DataManger.getInstance().mAnsweredQuesntionNumber++;
                    Logic.DataManger.getInstance().mUserScore += mListQuestion[i].questionScore;
                }
            }
            Debug.Log("答对了----" + Logic.DataManger.getInstance().mAnsweredQuesntionNumber+"题");
        }
    }
}