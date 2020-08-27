using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using System.Data;

namespace PTXServicesTest
{
    public partial class GetData : Form
    {
        private System.Timers.Timer N1Timer;
        const string appID = "de9846031af54c0e95cd3f0c68e6c9e9";
        const string appKey = "VVnz4vQe5Lsu4qtmUJAr79uVLRQ";
        int GetA1 = 0;

        string strconnection = ConfigurationManager.ConnectionStrings["PTXConn"].ConnectionString;

        public GetData()
        {
            InitializeComponent();
        }

        private void BtnStat_Click(object sender, EventArgs e)
        {
            N1Timer = new System.Timers.Timer();
            N1Timer.Interval = 5 * 1000;
            N1Timer.Elapsed += new ElapsedEventHandler(N1TimerWork);
            N1Timer.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            N1Timer.Stop();
            N1Timer = null;
        }

        //開始抓N1DataV3
        private void N1TimerWork(object sender, ElapsedEventArgs e)
        {
            int count = DeletePTX(); //先清空資料

            InsertN1DataV3("Tainan", "橘9"); //新增資料
            InsertN1DataV3("Tainan", "111"); //新增資料
            GetA1 = GetA1 + 1;            
            Console.WriteLine(GetA1);
        }

        private int DeletePTX()
        {
            SqlHelper del = new SqlHelper();
            return del.ExecuteNonQueryText("DELETE [PTXData].[dbo].[N1DataV3]", null);

        }


        public IRestResponse GetResponse(RestClient client)
        {
            var request = new RestRequest(Method.GET);
            string gmtStr = DateTime.UtcNow.ToString("r");
            string signature = HMACSHA1Text(@"x-date: " + gmtStr, appKey);
            string Authorization = @"hmac  username=""" + appID + @""", algorithm=""hmac-sha1"", headers=""x-date"", signature=""" + signature + @"""";
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", Authorization);
            request.AddHeader("x-date", gmtStr);
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            IRestResponse response = client.Execute(request);
            return response;
        }

        public void InsertN1DataV3(string city,string routeName)
        {
            try
            {
                //橘9路線資料
                RestClient client = new RestClient($"http://ptx.transportdata.tw/MOTC/v3/Bus/EstimatedTimeOfArrival/City/{city}/{routeName}?&$format=JSON");
                IRestResponse response = GetResponse(client);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var APIResult = JsonConvert.DeserializeObject<BusN1DataV3>(response.Content);
                    if (APIResult != null)
                    {
                        string RouteUID, RouteID, RouteName, SubRouteUID, SubRouteID, SubRouteName, DestinationStopID, DestinationStopName, StopUID, StopID, StopName, PlateNumb, ScheduledTime, CurrentStop;
                        int Direction, EstimateTime, StopStatus, StopCountDown;
                        DateTime DataTime, RecTime, TransTime;
                        bool IsLastBus;
                        string CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string Result = string.Empty;
                        foreach (N1DataV3 PTXData in APIResult.N1Datas)
                        {
                            
                            RouteUID = PTXData.RouteUID;
                            RouteID = PTXData.RouteID;
                            RouteName = PTXData.RouteName.Zh_tw;
                            SubRouteUID = PTXData.SubRouteUID;
                            SubRouteID = PTXData.SubRouteID;
                            SubRouteName = PTXData.SubRouteName.Zh_tw;
                            Direction = PTXData.Direction;
                            DestinationStopID = PTXData.DestinationStopID;
                            DestinationStopName = PTXData.DestinationStopName.Zh_tw;
                            StopUID = PTXData.StopUID;
                            StopID = PTXData.StopID;
                            StopName = PTXData.StopName.Zh_tw;
                            PlateNumb = PTXData.PlateNumb;
                            EstimateTime = PTXData.EstimateTime;
                            ScheduledTime = PTXData.ScheduledTime;
                            IsLastBus = PTXData.IsLastBus;
                            CurrentStop = PTXData.CurrentStop;
                            StopStatus = PTXData.StopStatus;
                            StopCountDown = PTXData.StopCountDown;
                            DataTime = PTXData.DataTime;
                            RecTime = PTXData.RecTime;
                            TransTime = PTXData.TransTime;

                            Result = Result +

                                              "INSERT INTO N1DataV3(RouteUID,  RouteID,  RouteName, SubRouteUID, SubRouteID, SubRouteName,  Direction,  DestinationStopID,  DestinationStopName,  StopUID,  StopID,  StopName,  PlateNumb, EstimateTime, ScheduledTime, IsLastBus,  CurrentStop,  StopStatus, StopCountDown,  DataTime,  RecTime,  TransTime)" +
                                              "VALUES(" +
                                              " '" + RouteUID + "' " +
                                              ",'" + RouteID + "' " +
                                              ",'" + RouteName + "' " +
                                              ",'" + SubRouteUID + "' " +
                                              ",'" + SubRouteID + "' " +
                                              ",'" + SubRouteName + "' " +
                                              ",'" + Direction + "' " +
                                              ",'" + DestinationStopID + "' " +
                                              ",'" + DestinationStopName + "' " +
                                              ",'" + StopUID + "' " +
                                              ",'" + StopID + "' " +
                                              ",'" + StopName + "' " +
                                              ",'" + PlateNumb + "' " +
                                              ",'" + EstimateTime + "' " +
                                              ",'" + ScheduledTime + "' " +
                                              ",'" + IsLastBus + "' " +
                                              ",'" + CurrentStop + "' " +
                                              ",'" + StopStatus + "' " +
                                              ",'" + StopCountDown + "' " +
                                              ",'" + DataTime.ToString("yyyy/MM/dd HH:mm:ss") + "' " +
                                              ",'" + RecTime.ToString("yyyy/MM/dd HH:mm:ss") + "' " +
                                              ",'" + TransTime.ToString("yyyy/MM/dd HH:mm:ss") + "' " +
                                              ") ;";


                        }
                        //寫入資料
                        if (Result != "")
                        {
                            SqlConnection connnew = new SqlConnection(strconnection);

                            try
                            {
                                connnew.Open();
                                SqlCommand cmmdnew = new SqlCommand(Result, connnew);
                                cmmdnew.ExecuteNonQuery();
                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("N1DataV3 error ");
                                Console.WriteLine(ex.Message);
                                
                            }
                            finally
                            {
                                connnew.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="EncryptText"></param>
        /// <param name="EncryptKey"></param>
        /// <returns></returns>
        public static string HMACSHA1Text(string EncryptText, string EncryptKey)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(EncryptKey);
            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(EncryptText);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);

        }
    }







    //下面是宣告資料型態
    public class BusN1DataV3
    {
        public string UpdateTime { get; set; } //(DateTime) : [平臺] 資料更新日期時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz) ,
        public int UpdateInterval { get; set; } //(integer) : [平臺] 資料更新週期(秒) ,
        public string SrcUpdateTime { get; set; } //(DateTime) : [來源端平臺] 資料更新時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz) ,
        public int SrcUpdateInterval { get; set; } //(integer) : 來源端平台資料更新週期(秒)['-1: 不定期更新'] ,
        public string AuthorityCode { get; set; } //(string) : 業管機關簡碼 ,
        public List<N1DataV3> N1Datas { get; set; } //(Array[N1Data]) : 資料(陣列)
    }
    public class N1DataV3
    {
        public string RouteUID { get; set; } //(string, optional): 路線唯一識別代碼，規則為 {業管機關代碼} + {RouteID}，其中 {業管機關代碼} 可於Authority API中的AuthorityCode欄位查詢 ,
        public string RouteID { get; set; } //(string): 地區既用中之路線代碼(為原資料內碼) ,
        public NameType RouteName { get; set; } //(NameType, optional): 路線名稱 ,
        public string SubRouteUID { get; set; } //(string, optional): 附屬路線唯一識別代碼，規則為 {業管機關簡碼} + {SubRouteID}，其中 {業管機關簡碼} 可於Authority API中的AuthorityCode欄位查詢 ,
        public string SubRouteID { get; set; } //(string, optional): 地區既用中之附屬路線代碼(為原資料內碼) ,
        public NameType SubRouteName { get; set; } //(NameType, optional): 附屬路線名稱 ,
        public int Direction { get; set; } //(integer): 車輛去返程(該方向指的是此公車運具目前所在路線的去返程方向，非指站牌所在路線的去返程方向，使用時請加值業者多加注意) : [0:'去程',1:'返程',2:'迴圈',255:'未知'] ,
        public string DestinationStopID { get; set; } //(string, optional): 迄點站站牌ID代碼 ,
        public NameType DestinationStopName { get; set; } //(NameType, optional): 迄點站站牌名稱 ,
        public string StopUID { get; set; } //(string, optional): 站牌唯一識別代碼，規則為 {業管機關簡碼} + {StopID}，其中 {業管機關簡碼} 可於Authority API中的AuthorityCode欄位查詢 ,
        public string StopID { get; set; } //(string): 地區既用中之站牌代碼(為原資料內碼) ,
        public NameType StopName { get; set; } //(NameType, optional): 站牌名稱 ,
        public string PlateNumb { get; set; } //(string, optional): 車牌號碼 [値為値為-1時，表示目前該站牌無車輛行駛] ,
        public int EstimateTime { get; set; } //(integer, optional): 到站時間預估(秒) [當StopStatus値為1~4或PlateNumb値為-1時，EstimateTime値為空値; 反之，EstimateTime有値] ,
        public string ScheduledTime { get; set; } //(string, optional): 預排班表時間 ,
        public bool IsLastBus { get; set; } //(boolean, optional): 是否為末班車 ,
        public string CurrentStop { get; set; } //(string, optional): 車輛目前所在站牌代碼 ,
        public int StopStatus { get; set; } //(integer, optional): 車輛狀態備註 : [0:'正常',1:'尚未發車',2:'交管不停靠',3:'末班車已過',4:'今日未營運'] ,
        public int StopCountDown { get; set; } //(integer, optional): 路線經過站牌之順序 ,
        public DateTime DataTime { get; set; } //(DateTime, optional): 系統演算該筆預估到站資料的時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz) ,
        public DateTime RecTime { get; set; } //(DateTime): 來源端平台接收時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz) ,
        public DateTime TransTime { get; set; } //(DateTime): 來源端平台資料傳出時間(ISO8601格式:yyyy-MM-ddTHH:mm:sszzz)
    }
    public class NameType
    {
        public string Zh_tw { get; set; } //(string, optional): 中文繁體名稱 ,
        public string En { get; set; } //(string, optional): 英文名稱
    }
}
