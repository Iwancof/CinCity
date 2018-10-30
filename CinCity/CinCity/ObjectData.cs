using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CinCity
{
    class BaseRoadClass
    {
        public Point Coordinate = new Point(-1, -1);
        public int ID;
        public string TypeName;

        //int ConnectedPoint;

        public List<int> ConnectedPointsID;

        public BaseRoadClass(int i, Point c,string type,List<int> cps) {
            ID = i;
            Coordinate = c;
            ConnectedPointsID = cps;
            TypeName = type;
        }

        public Point GetObjectCoordinate() { return Coordinate; }
        public int GetObjectID() { return ID; }
        public string GetObjectType() { return TypeName; }
        public List<int> GetList() { return ConnectedPointsID; }

        public void Add(int x) { ConnectedPointsID.Add(x); }
        public void SetList(List<int> l) { ConnectedPointsID = l; }
        

    }
    class BaseBuildClass
    {
        public Point Coordinate = new Point(-1, -1);
        public int ID;
        public string TypeName;

        public BaseBuildClass(int i,Point p,string type) {
            ID = i;
            Coordinate = p;
            TypeName = type;
        }
        
        public Point GetObjectCoordinate() { return Coordinate; }
        public int GetObjectID() { return ID; }
        public string GetObjectType() { return TypeName; }
        public void Add(int x) { }
        public List<int> GetList(){ return new List<int>(); }
        public void SetList(List<int> x) { }
    }
}