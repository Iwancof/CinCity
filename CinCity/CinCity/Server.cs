using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Drawing;
using System.IO;

namespace CinCity
{
    partial class CinCity
    {
        string[] fondtype = { "CM", "RO" ,"BU","CL"}; //構造物一覧的な

        public void CheckMessage(string str) {
            ClearPoint = new Point(-1, -1);
            for (int i = 0; i < fondtype.Length; i++) {
                string f = fondtype[i];
                int foundIndex = str.IndexOf(f);
                while (0 <= foundIndex) {
                    int id = int.Parse(str.Substring(foundIndex + 4, 2));
                    Point TextRoadPoint = new Point(int.Parse(str.Substring(foundIndex + 7, 3)), int.Parse(str.Substring(foundIndex + 11, 3)));
                    if (f == "CM") {
                        CriterionMarkers[id] = TextRoadPoint;
                    }
                    if (f == "RO") {
                        if (IsMarkered) {
                            double X_Co = (double)TextRoadPoint.X - (double)CriterionMarkers[0].X; //カメラ座標どうして差分を取って係数をかける
                            X_Co /= X_Converting_coefficient;
                            double Y_Co = (double)TextRoadPoint.Y - (double)CriterionMarkers[0].Y; //上記に同じ
                            Y_Co /= Y_Converting_coefficient;
                            RoadMarkers[id] = new Point((int)X_Co + Dif, (int)Y_Co + Dif);
                            RoadMarkersAvailable[id] = true;
                            //drawstirng = "p:" + p.ToString() + " D:" + (p.X - TestRoad.X).ToString() + "," + (p.Y - TestRoad.Y).ToString(); //テストコード
                        }
                    }
                    if(f == "BU") {
                        if (BuildPoint.ContainsKey(id))
                            BuildPoint[id] = TextRoadPoint;
                        else BuildPoint.Add(id, TextRoadPoint);
                    }
                    if(f == "CL") {
                        ClearPoint = TextRoadPoint;
                    }
                    int nextIndex = foundIndex + fondtype.Length;
                    if (nextIndex < str.Length) foundIndex = str.IndexOf(f, nextIndex);
                    else break;
                }
            }

        }

        TcpListener listener; //サーバー本体
        IPAddress IPAddr = IPAddress.Parse("127.0.0.1"); //使用するIPアドレス
        int Port = 51081;                                //使用するポート番号

        bool IsListening = false;
        
        private async void ServerStart() {
            while (true) {
                listener.Start(); //開始
                IsListening = true;

                TcpClient client = null;
                try { client = await listener.AcceptTcpClientAsync(); } //受け取り
                catch (Exception) { return; }


                if (client == null)
                    return;
                NetworkStream ns = client.GetStream(); //クライアントからストリーム情報を取得
                ns.ReadTimeout = 10000;
                ns.WriteTimeout = 10000;

                MemoryStream ms = new MemoryStream(); 

                byte[] resBytes = new byte[256];
                int resSize = 0;
                do {
                    //データの一部を受信する
                    resSize = ns.Read(resBytes, 0, resBytes.Length);
                    //Readが0を返した時はクライアントが切断したと判断
                    if (resSize == 0) break;
                    //受信したデータを蓄積する
                    ms.Write(resBytes, 0, resSize);
                    //まだ読み取れるデータがあるか、データの最後が\nでない時は受信を続ける
                } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');
                //受信したデータを文字列に変換
                string resMsg = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                ms.Close();
                //末尾の\nを削除
                resMsg = resMsg.TrimEnd('\n');
                CheckMessage(resMsg); //Pythonから送られてくる座標データを解析して適切に処理するすごーい関数。
            }
        }


        //この下いじるべからず

        private void button1_Click(object sender, EventArgs e) {
            if (IsListening) {
                Console.WriteLine("The server has already started. if you want to server's restart, please stop the server first.");
                return;
            }
            if (this.Text.Contains("State"))
                this.Text = this.Text.Replace("Stopping", "Working");
            else
                this.Text += " State : Working";
            ServerStart();
        }

        private void ServerStop_Click(object sender, EventArgs e) {
            //IsEnd = true;
            this.Text = this.Text.Replace("Working", "Stopping");
            try {
                IsListening = false;
                listener.Stop();
            } catch (Exception) { }
        }
    }
}
