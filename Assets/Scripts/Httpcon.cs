using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System;
using System.Web;


namespace Http
{
    public class Httpcon
    {


        public static Httpcon instance;

        public static Httpcon getinstance()
        {
            if (instance == null)
                instance = new Httpcon();
            return instance;
        }

        public string Getroletype(string url, int timeout)
        {
            Stream myresponsestrem;
            StreamReader mystreamrender;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html:charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = timeout;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 myresponsestrem = response.GetResponseStream();
                 mystreamrender = new StreamReader(myresponsestrem, Encoding.GetEncoding("utf-8"));

            }
            catch(Exception ex){
                throw ex;
            }
      

            string retstring = mystreamrender.ReadToEnd();
            mystreamrender.Close();
            myresponsestrem.Close();
            return retstring;
       
        }

        public string Getrolename(string url,int timeout,string roletype)
        {
          //  Encoding myEncoding = Encoding.GetEncoding("gb2312");
            Stream myresponsestrem;
            StreamReader mystreamrender;
            var Url = url + HttpUtility.UrlEncode("roletype") + "=" + HttpUtility.UrlEncode(roletype);
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.ContentType = "text/html:charset=UTF-8";
            request.Method = "GET";
            request.Timeout = timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            myresponsestrem = response.GetResponseStream();
            mystreamrender = new StreamReader(myresponsestrem, Encoding.GetEncoding("utf-8"));

            string retstring = mystreamrender.ReadToEnd();
            mystreamrender.Close();
            myresponsestrem.Close();
            return retstring;
        }


        public bool  Checklogin(string url,string roletype,string rolename,string password,int timeout)
        {
            Stream myresponsestrem;
            StreamReader mystreamrender;
            string param = "roletype=" + roletype + "&rolename=" + rolename + "&password="+password;
            byte[] bs = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8"; 
            request.ContentLength = bs.Length;
            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(bs, 0, bs.Length);
            stream.Close();
       
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            myresponsestrem = response.GetResponseStream();
            mystreamrender = new StreamReader(myresponsestrem, Encoding.GetEncoding("utf-8"));
            string retstring = mystreamrender.ReadToEnd();
            mystreamrender.Close();
            myresponsestrem.Close();
            if (retstring.Equals("true"))
                return true;
            else
                return false;
            //Stream myresponsestrem;
           // StreamReader mystreamrender;
           /* myresponsestrem = response.GetResponseStream();
            mystreamrender = new StreamReader(myresponsestrem, Encoding.GetEncoding("utf-8"));
            string retstring = mystreamrender.ReadToEnd();
            mystreamrender.Close();
            myresponsestrem.Close();
            return retstring;*/
        }

        public string GetSingleQues(string url, int timeout)
        {
            Stream myresponsestrem;
            StreamReader mystreamrender;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html:charset=UTF-8";
                request.UserAgent = null;
                request.Timeout = timeout;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                myresponsestrem = response.GetResponseStream();
                mystreamrender = new StreamReader(myresponsestrem, Encoding.GetEncoding("utf-8"));

            }
            catch (Exception ex)
            {
                throw ex;
            }


            string retstring = mystreamrender.ReadToEnd();
            mystreamrender.Close();
            myresponsestrem.Close();
            return retstring;

        }

    }

}
