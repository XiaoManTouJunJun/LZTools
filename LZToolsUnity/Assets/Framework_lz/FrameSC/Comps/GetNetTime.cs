 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// func: 获取当前时间段
/// author: lz910608@gmail.com
/// time: 2019-1-29
/// </summary>
namespace FrameWork_lz
{

    /// <summary>
    /// 当前时间段类型
    /// </summary>
    public enum TimeSlotType
    {
        Morning,
        Noon,
        Afternoon,
        Night
    }


    public class GetNetTime
    {

        private int year, mouth, day, hour, min, sec;

        public string timeURL = "http://cgi.im.qq.com/cgi-bin/cgi_svrtime";

        private int OneHourValue = 3600;

        private TimeSlotType curTimeSlotType;

        public TimeSlotType CurTimeSlot { get { return curTimeSlotType; } }



        IEnumerator GetCurTime()
        {
            WWW www = new WWW(timeURL);
            while (!www.isDone)
            {
                yield return www;
            }
            Debug.LogFormat("Time {0}", www.text);

            if (www.text == "" || www.text.Contains("<html>"))
            {
                GetLocalTime();
            }
            else
            {
                SplitTime(www.text);
            }
            www.Dispose();
        }



        void SetTimeSlotType(int hour)
        {
            if (hour >= 5 && hour < 11)//早上
            {
                curTimeSlotType = TimeSlotType.Morning;
            }
            else if (hour >= 11 && hour <= 13)//中午
            {
                curTimeSlotType = TimeSlotType.Noon;
            }
            else if (hour > 13 && hour <= 17)//下午
            {
                curTimeSlotType = TimeSlotType.Afternoon;
            }
            else if (hour > 17 && hour <= 21)//晚上
            {
                curTimeSlotType = TimeSlotType.Night;
            }

        }

        void GetLocalTime()
        {
            hour = System.DateTime.Now.Hour;
            Debug.LogFormat("local hour {0}  slotType {1}", hour, curTimeSlotType);
            SetTimeSlotType(hour);
        }

        void SplitTime(string dateTime)
        {
            dateTime = dateTime.Replace("-", "|");
            dateTime = dateTime.Replace(" ", "|");
            dateTime = dateTime.Replace(":", "|");
            string[] Times = dateTime.Split('|');
            year = int.Parse(Times[0]);
            mouth = int.Parse(Times[1]);
            day = int.Parse(Times[2]);
            hour = int.Parse(Times[3]);
            min = int.Parse(Times[4]);
            sec = int.Parse(Times[5]);

            SetTimeSlotType(hour);
             
        }
    }
}



