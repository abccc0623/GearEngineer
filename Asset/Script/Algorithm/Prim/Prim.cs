
using System.Collections.Generic;

namespace Algorithm
{
    public class Prim 
    {
        public Prim(List<BSPNode> BSPNodes)
        {
            List<(BSPNode from, BSPNode to)> connections = new List<(BSPNode, BSPNode)>();
            var MST = new HashSet<BSPNode>();
            MST.Add(BSPNodes[0]);
            
            //모든 노드를 다돌았을 때 종료                                                                         
            while (MST.Count != BSPNodes.Count)
            {
                //await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                //해당 노드에 가장 비용이 적은 노드를 가져온다.                                                           
                var minLength = float.MaxValue;
                BSPNode nextRoom = null;
                BSPNode startRoom = null;
                foreach (var node in MST)
                {
                    for (var i = 0; i < node.linkNode.Count; i++)
                    {
                        //만약 이미 지나온 노드라면 건너 뛴다.                                                        
                        if (MST.Contains(node.linkNode[i])) continue;
                        var candidate = node.linkNode[i];
                        if (candidate == node || MST.Contains(candidate))
                            continue;
        
                        //가장 비용이 적은 노드를 찾는다.                                                           
                        var targetLength = node.center.DistanceTo(node.linkNode[i].center);
                        if (targetLength < minLength)
                        {
                            minLength = targetLength;
                            nextRoom = node.linkNode[i];
                            startRoom = node;
                        }
                    }
                }
        
                if (nextRoom == null)
                {
                    break;
                }
        
                //비용이 가장 작은 노드를 MST에 다시 넣어줌.                                                           
                connections.Add((startRoom, nextRoom));
                MST.Add(nextRoom);
            }
        
            for (var i = 0; i < BSPNodes.Count; i++) BSPNodes[i].linkNode.Clear();
        
            foreach (var valueTuple in connections)
            {
                var start = valueTuple.from;
                var end = valueTuple.to;
        
                start.linkNode.Add(end);
                end.linkNode.Add(start);
            }
        }
    }
}