using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CinCity
{
    partial class CinCity
    {
        void objope() {
            if (MarksAvaiInitCount > MarksAvaiInitMax) {
                if (ClearPoint.X != -1) { //オブジェクト削除
                    int id = GetNearistRoad(ClearPoint, false)[0];
                    for (int i = 0; i < Roads.Count; i++)
                        if (Roads[i].GetObjectID() == id) {
                            Roads.Remove(Roads[i]);
                            break;
                        }
                }

                foreach (int i in RoadMarkersNumber) { // 道を追加
                    if (RoadMarkersAvailable[i]) {
                        List<int> l = GetNearistRoad(RoadMarkers[i], true);
                        if (l.Count != 0) { //Roadが一つ以上ある場合。
                            //-1、つまり、近くにオブジェクトがない場合はコネクトのidはなしでOK!
                            foreach (BaseRoadClass o in Roads)
                                if (l.Contains(o.GetObjectID()))
                                    o.Add(Roads.Count);
                            Roads.Add(new BaseRoadClass(Roads.Count, RoadMarkers[i], "Road", l));
                        }
                        RoadMarkersAvailable[i] = false;
                    }
                }

                foreach (KeyValuePair<int, Point> kv in BuildPoint) {//建物追加
                    Builds.Add(new BaseBuildClass(Builds.Count, kv.Value, "Build"));
                    BuildPoint = new Dictionary<int, Point>();
                }

                MarksAvaiInitCount = 0;
            }
        }

    }
}
