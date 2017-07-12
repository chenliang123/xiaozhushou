using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Edu_Simulator
{
    [DataContract]
    public class DataInfo
    {
        [DataMember]
        public int StudentCount { get; set; }
        [DataMember]
        public string StudentNumbers { get; set; }
        [DataMember]
        public StudentInfo[] Student { get; set; }
    }
    [DataContract]
    public class ClassInfo
    {
        [DataMember]
        public DataInfo Data { get; set; }
    }
    [DataContract]
    public class CBInfo
    {
        [DataMember]
        public string data { get; set; }
    }
    [DataContract]
    public class AnswerInfo
    {
        [DataMember]
        public int CBID { get; set; }
        [DataMember]
        public string CBAnswer { get; set; }
    }
    [DataContract]
    public class StudentInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string SEAT { get; set; }
    }

    /// <summary>
    /// 年级-班级
    /// </summary>
    [DataContract]
    public class Gradelist
    {
        [DataMember]
        public int schoolid { get; set; }
        [DataMember]
        public string schoolname { get; set; }
        [DataMember]
        public Grade[] gradelist { get; set; }
    }
    [DataContract]
    public class Grade
    {
        [DataMember]
        public int grade { get; set; }
        [DataMember]
        public string gradename { get; set; }
        [DataMember]
        public Classes[] classlist { get; set; }
    }
    [DataContract]
    public class Classes
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int schoolid { get; set; }
        [DataMember]
        public int grade { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int roomid { get; set; }
        [DataMember]
        public int orderid { get; set; }
        [DataMember]
        public string appip { get; set; }
        [DataMember]
        public int courseid { get; set; }
        [DataMember]
        public string coursename { get; set; }
    }

    //{"ret":"0","msg":"获取班级列表成功","data":{"gradelist":[{"grade":"3","gradename":"三年级","classlist":[
    //{"id":"1634","schoolid":"33","grade":"3","name":"三(1)班","seatxy":"1,1","orderid":"1","roomid":"1356","roomname":"三年级一班","building":"","hdid":"","hdip":"1","hdport":"80","appip":"172.18.2.110","courseid":"0"}]}]},"count":"1"}

    [DataContract]
    public class User
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string nick { get; set; }
        [DataMember]
        public string account { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int courseid { get; set; }
        [DataMember]
        public string studentno { get; set; }
        [DataMember]
        public string schoolname { get; set; }
        [DataMember]
        public string schoolcode { get; set; }
        [DataMember]
        public Classes[] classlist { get; set; }
    }
    //{"id":"3842","type":"1","phone":"201604081438","nick":"","account":"wangqi","name":"王奇",
    //"pinyin":"","imageurl":"","age":"0","gender":"1","province":"北京","city":"北京","district":"东城区","address":"111111111111111",
    //"studentno":"","schoolid":"33","courseid":"0","teachyears":"0","intro":"1111111111111",
    //"classlist":[{"id":"1634","schoolid":"33","grade":"3","name":"三(1)班","seatxy":"1,1","orderid":"1","roomid":"1356","roomname":"三年级一班","building":"","hdid":"","hdip":"1","hdport":"80","appip":"172.18.2.110","courseid":"12","coursename":"语文"}],
    //"schoolname":"测试小学","schoolcode":"3477786fefd57d7403631c839b6787d4"}"
}
