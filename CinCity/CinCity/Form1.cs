using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CinCity
{
    public partial class CinCity : Form
    {
        Graphics g; //ここに描画をする。
        Bitmap MainBitmap; //ここに描画される。
        Dictionary<string, Image> MarkerImages; //日本語での名前と画像のセット
        List<int> CriterionMarkersNumber;
        Dictionary<int, Point> CriterionMarkers;//目安の座標セット --もしかしたら三つでいいかもしれない。
        List<int> RoadMarkersNumber;
        Dictionary<int, Point> RoadMarkers; //道信号の座標セット。すべてのIDと座標で一般化できていないでの実質ウンコ
        Dictionary<int, bool> RoadMarkersAvailable;
        double X_Converting_coefficient = 1; //X座標の変換係数。C#側一ピクセル当たりのカメラの大きさ
        double Y_Converting_coefficient = 1; //Y
        bool IsMarkered = false; //すべての目安マーカーをマークできたか
        public int Dif = 16; //目安座標を端からどれくらいずらすか。
        Image BuildingImage = Image.FromFile("Build.png");
        Dictionary<int, Point> BuildPoint = new Dictionary<int,Point>();//ID,Point
        Random rand = new Random();

        Point TestRoad = new Point(500, 0);
        public string drawstirng = "";
        int x_p = 0;
        int y_p = 1;
        int TestDrawType = 0;

        Point ClearPoint = new Point(-1, -1);

        List<Pen> PensList = new List<Pen>();

        List<BaseRoadClass> Roads = new List<BaseRoadClass>();
        List<BaseBuildClass> Builds = new List<BaseBuildClass>();

        Point CarPoint = new Point(-1, -1);
        Point CarTarget;
        int CarTargetID;
        //List<IObjectData> Objects = new List<IObjectData>(); //道や建物などのすべてのデータ

        int MarksAvaiInitCount = 0;
        int MarksAvaiInitMax = 50; //タイマーは1msで回っているので500ms(0.5秒) 追記　全然違かった
        //一定時間でマーカーが有効かどうかを初期化。→受動的にすると誤反応が多発
        //(超連続で建物が出るため、動かしたら追尾するような定数にする必要あり)。

        public CinCity() {
            InitializeComponent();

            PensList.Add(Pens.Black);
            PensList.Add(Pens.Blue);
            PensList.Add(Pens.Red);
            PensList.Add(Pens.Green);
            PensList.Add(Pens.Gray);

            this.Text = "CinCity";

            listener = new TcpListener(IPAddr, Port);

            MainBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height); //描画用にビットマップを作成
            g = Graphics.FromImage(MainBitmap); //描画用のグラフィックを作成
            g.Clear(this.BackColor); //初期化

            DispTim.Enabled = true; //タイマースタート
            ProcessTimer.Enabled = true;

            Markerinit();

            ServerStart(); //サーバースタート(とりあえず) 
        }

        private void Markerinit() {
            MarkerImages = new Dictionary<string, Image>();
            MarkerImages.Add("目安-左上", GetImage(0));
            MarkerImages.Add("目安-右上", GetImage(1));
            MarkerImages.Add("目安-左下", GetImage(2));
            MarkerImages.Add("目安-右下", GetImage(3));
            MarkerImages.Add("道1", GetImage(4));
            MarkerImages.Add("道2", GetImage(5));
            MarkerImages.Add("道3", GetImage(6));
            MarkerImages.Add("道4", GetImage(7));
            MarkerImages.Add("建物1", GetImage(8));
            MarkerImages.Add("建物2", GetImage(9));
            MarkerImages.Add("建物3", GetImage(10));
            MarkerImages.Add("建物4", GetImage(11));
            MarkerImages.Add("クリーナー1", GetImage(12));
            MarkerImages.Add("クリーナー2", GetImage(13));
            MarkerImages.Add("クリーナー3", GetImage(14));
            MarkerImages.Add("クリーナー4", GetImage(15));


            CriterionMarkersNumber = new List<int>();
            for (int i = 0; i < 4; i++)
                CriterionMarkersNumber.Add(i); //ID -- 0,1,2,3

            CriterionMarkers = new Dictionary<int, Point>();
            foreach (int i in CriterionMarkersNumber)
                CriterionMarkers.Add(i, new Point(0, 0));

            RoadMarkersNumber = new List<int>();
            for (int i = 4; i < 8; i++)
                RoadMarkersNumber.Add(i);      //ID -- 4,5,6,7

            RoadMarkers = new Dictionary<int, Point>();
            RoadMarkersAvailable = new Dictionary<int, bool>();
            foreach (int i in RoadMarkersNumber) {
                RoadMarkers.Add(i, new Point(-1, -1));
                RoadMarkersAvailable.Add(i, false);
            }
        }

        private void CoordinateConverion(int Dif) {
            for (int i = 0; i < 4; i++)
                if (CriterionMarkers[i] == new Point(0, 0))
                    return; //すべてのマーカーの情報がそろっていなかったらやめる。

            X_Converting_coefficient = (CriterionMarkers[1].X - CriterionMarkers[0].X) / (double)(pictureBox1.Width - Dif * 2 - MarkerImages["目安-左上"].Width);
            Y_Converting_coefficient = (CriterionMarkers[2].Y - CriterionMarkers[0].Y) / (double)(pictureBox1.Height - Dif * 2 - MarkerImages["目安-左上"].Height);

            IsMarkered = true;
        }


        private Image GetImage(int n) {
            return Image.FromFile(@"s6X6_250_Images\s6X6_250-" + n.ToString() + ".png");
        }


        private void DispTim_Tick(object sender, EventArgs e) {
            CoordinateConverion(Dif);
            g.Clear(this.BackColor); //まず初期化

            //目安マーカー　左上 右上 左下 右下

            g.DrawString(drawstirng, new Font("MS UI", 7), Brushes.Blue, new Point(70, 100));
            g.DrawString(TestRoad.ToString(), new Font("MS UI", 10), Brushes.Blue, new Point(100, 70));



            g.DrawImage(MarkerImages["目安-左上"], Dif, Dif);
            g.DrawImage(MarkerImages["目安-右上"], pictureBox1.Width - MarkerImages["目安-右上"].Width - Dif, Dif);
            g.DrawImage(MarkerImages["目安-左下"], Dif, pictureBox1.Height - MarkerImages["目安-左下"].Height - Dif);
            g.DrawImage(MarkerImages["目安-右下"], pictureBox1.Width - MarkerImages["目安-右下"].Width - Dif, pictureBox1.Height - MarkerImages["目安-右下"].Height - Dif);

            bool isd = true;
            foreach (KeyValuePair<int, Point> p in RoadMarkers) //チェックのため道と認識した場所を描画
                if (isd && p.Value.X != -1) //初期値で-1なのでそれ以外
                    g.DrawRectangle(Pens.Blue, p.Value.X, p.Value.Y, MarkerImages["目安-左上"].Width, MarkerImages["目安-左上"].Height); //描画

            List<int> DrewList = new List<int>();
            foreach (BaseRoadClass o in Roads) {
                DrewList.Add(o.GetObjectID()); //これからこのオブジェクトは描画されるので登録して
                foreach (int i in o.GetList())//そのオブジェクトにつながっているものすべてのIDを回し
                    foreach (BaseRoadClass so in Roads)//該当のオブジェクトを探して
                        if (so.GetObjectID() == i && !DrewList.Contains(i)) //それならば
                            g.DrawLine(new Pen(Color.Gray, 7), o.GetObjectCoordinate(), so.GetObjectCoordinate());
            }
            foreach (BaseBuildClass o in Builds) //すべての建物を描画
                g.DrawImage(BuildingImage, o.GetObjectCoordinate());
            { 
}
            g.DrawEllipse(Pens.Green,TestRoad.X - MarkerImages["道1"].Width / 2, TestRoad.Y - MarkerImages["道1"].Height / 2, (float)Math.Sqrt(5000),(float)Math.Sqrt(5000));
            if (Roads.Count != 0) { //車の描画
                if (CarPoint.X == -1) {
                    CarPoint = Roads[0].GetObjectCoordinate();
                    if (Roads[0].GetList().Count != 0) {
                        CarTargetID = rand.Next(Roads[0].GetList().Count);
                        CarTarget = Roads[CarTargetID].GetObjectCoordinate();
                    }
                } else {
                    if (CarPoint == CarTarget) {
                        CarTargetID = rand.Next(Roads[CarTargetID].GetList().Count);
                        CarTarget = Roads[CarTargetID].GetObjectCoordinate();
                        Console.WriteLine("Arrive");
                    } else {
                        if (CarTarget.X - CarPoint.X == 0) { CarPoint.Y += Math.Max(-3, Math.Min(3, CarTarget.Y - CarPoint.Y)); Console.WriteLine("aa"); } 
                        else if (CarTarget.Y - CarPoint.Y == 0) { CarPoint.X += Math.Max(-3, Math.Min(3, CarTarget.X - CarPoint.X)); Console.WriteLine("bb"); }
                        else {
                            double Slope = (CarTarget.X - CarPoint.X) / (CarTarget.Y - CarPoint.Y);

                            int cxp = Math.Max(Math.Min(3, CarTarget.X - CarPoint.X), -3);
                            int cyp = (int)(cxp * Slope);
                            Console.WriteLine("Slope : " + Slope.ToString());

                            CarPoint.X += cxp;
                            CarPoint.Y += cyp;
                            Console.WriteLine("PlusPoint : " + new Point(cxp, cyp).ToString());
                        }
                        }
                }
                //Console.WriteLine(CarPoint.ToString());
            }


            //g.DrawString(.ToString(), new Font("MS UI", 10), Brushes.Blue, new Point(100, 90));

            Image img;
            if (TestDrawType == 0) img = MarkerImages["道1"];
            else if (TestDrawType == 1) img = MarkerImages["クリーナー1"];
            else img = MarkerImages["建物1"];
            g.DrawImage(img, TestRoad); //テスト用
            g.DrawEllipse(Pens.Black, CarPoint.X, CarPoint.Y, 10, 10);


            pictureBox1.Image = MainBitmap; //画面に描画された描画
        }

        private void CinCity_Resize(object sender, EventArgs e) {
            this.MainBitmap = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(MainBitmap);
        }

        double gnp(Point p1, Point p2) { //GetNearingPoint
            return Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2); //そのまま。めんどいから平方根は取ってないよ。
        }

/*        public List<int> GetNearistObjectID(Point p1, List<string> TypeName,bool IsNear) {
            double Distance = 1000000;
            int ID = -1;
            List<int> IDs = new List<int>();
            for (int i = 0; i < Objects.Count; i++) {
                if (!TypeName.Contains(Objects[i].GetObjectType()))
                    continue; //道でなかったらダメ
                if (Distance > gnp(p1, Objects[i].GetObjectCoordinate())) {
                    //continue; //距離が短いほうが良い。
                    Distance = gnp(p1, Objects[i].GetObjectCoordinate());
                    ID = Objects[i].GetObjectID();
                }
                if (IsNear && 5000 > gnp(p1, Objects[i].GetObjectCoordinate())) {
                    IDs.Add(Objects[i].GetObjectID());
                    Console.WriteLine(Objects[i].GetObjectType());
                }
            }
            if (ID != -1) IDs.Add(ID);
            return IDs;
        }
*/
        public List<int> GetNearistRoad(Point p,bool isMultiple) {
            double Distance = 1000000;
            double dis = 0;
            int NearistID = -1;
            List<int> IDs = new List<int>();
            for(int i = 0;i < Roads.Count; i++) {
                BaseRoadClass o = Roads[i];
                dis = gnp(p, o.GetObjectCoordinate());
                if(Distance > dis) {
                    Distance = dis;
                    NearistID = i;
                    continue;
                }
                if(isMultiple &&  dis < 5000)
                    IDs.Add(i);
            }
            IDs.Add(NearistID);
            return IDs;
        }

        public List<int> GetNearistBuild(Point p,bool isMultiple) {
            double Distance = 1000000;
            double dis = 0;
            int NearistID = -1;
            List<int> IDs = new List<int>();
            for (int i = 0; i < Builds.Count; i++) {
                BaseBuildClass o = Builds[i];
                dis = gnp(p, o.GetObjectCoordinate());
                if (Distance > dis) {
                    Distance = dis;
                    NearistID = i;
                    continue;
                }
                if (isMultiple && dis < 5000)
                    IDs.Add(i);
            }
            IDs.Add(NearistID);
            return IDs;
        }

        private void ProcessTimer_Tick(object sender, EventArgs e) {
            MarksAvaiInitCount++;
            objope();
            
            if (TestRoad.X < 0) x_p = 1;
            else if (TestRoad.X > pictureBox1.Width - MarkerImages["目安-右下"].Width) x_p = -1;
            if (TestRoad.Y < 0) y_p = 1;
            else if (TestRoad.Y > pictureBox1.Height - MarkerImages["目安-右下"].Height) y_p = -1;

            //TestRoad.X += x_p;
            //TestRoad.Y += y_p;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            //g.DrawImage(MarkerImages["道1"], e.X, e.Y);
            TestRoad = new Point(e.X + 10, e.Y + 10);
        }

        private void Clear_Click(object sender, EventArgs e) {
            Roads = new List<BaseRoadClass>();
            Builds = new List<BaseBuildClass>();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            TestDrawType = (TestDrawType + 1) % 3;
        }
    }
}
