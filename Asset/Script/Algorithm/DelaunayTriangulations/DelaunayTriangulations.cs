using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace Algorithm
{
    public class DelaunayTriangulations
    {
        public List<Triangle> Create(List<Vector2I> points, Vector2I center)
        {
            var span = Mathf.Min(center.X, center.Y);
            Vector2I p1 = new Vector2I(center.X - 2 * span, center.Y - span);
            Vector2I p2 = new Vector2I(center.X + 2 * span, center.Y - span);
            Vector2I p3 = new Vector2I(center.X, center.Y + 2 * span);

            //모든 점들을 포함하는 거대한 삼각형을 만듬
            Triangle bigTriangle = new Triangle(p1, p2, p3);
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(bigTriangle);

            for (var i = 0; i < points.Count; i++)
            {
                //현재 만들어진 삼각형들중에 점을 추가했을 때 그안에 포함되는 점이있다면 그 삼각형을 bad리스트에 넣는다.
                List<Triangle> badTriangles = new List<Triangle>();
                for (var t = 0; t < triangleList.Count; t++)
                {
                    if (triangleList[t].PointInCircumcircle(points[i]))
                    {
                        badTriangles.Add(triangleList[t]);
                    }
                }

                //삼각형의 변들을 조사
                List<Line> polygon = new List<Line>();
                foreach (var badTriangle in badTriangles)
                {
                    //삼각형들에 변을 가져오고 이미 있는 변이면 제거하고 처음나온 변이면 추가
                    var lines = badTriangle.GetLines();
                    foreach (var line in lines)
                    {
                        if (polygon.Contains(line))
                        {
                            polygon.Remove(line);
                        }
                        else
                        {
                            polygon.Add(line);
                        }
                    }
                }

                //이삼각형에서 조사할것은 끝났으니 제거한다.
                foreach (var bad in badTriangles) triangleList.Remove(bad);

                //가져온 변 기준으로 새로운 삼각형을 생성
                foreach (var edge in polygon)
                {
                    Triangle newTriangle = new Triangle(edge.p1, edge.p2, points[i]);
                    triangleList.Add(newTriangle);
                }
            }
            //가장 큰 삼각형을 제거
            triangleList.RemoveAll(triangle => triangle.RemovePoint(p1));
            triangleList.RemoveAll(triangle => triangle.RemovePoint(p2));
            triangleList.RemoveAll(triangle => triangle.RemovePoint(p3));
            return triangleList;
        }
    }
}