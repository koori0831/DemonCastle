using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Work.TRPG.Dialogue
{
    [CreateAssetMenu(menuName = "SO/TRPG/Dialogue/Dialogue Container", fileName = "DialogueContainer")]
    public class DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string startNodeGuid;
        [SerializeField] private List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
        [SerializeReference] private List<NodeData> nodeDataList = new List<NodeData>();

        [SerializeField] private StringTableCollection mainTable;
        [SerializeField] private List<StringTableCollection> relatedTables = new List<StringTableCollection>();

        public string StartNodeGuid => startNodeGuid;
        public IReadOnlyList<NodeLinkData> NodeLinks => nodeLinks;
        public IReadOnlyList<NodeData> NodeDataList => nodeDataList;

        public StringTableCollection MainTable => mainTable;
        public IReadOnlyList<StringTableCollection> RelatedTables => relatedTables;

        public void SetStartNode(string guid)
        {
            startNodeGuid = guid;
        }


        public void SetNodes(IEnumerable<NodeData> nodes)
        {
            nodeDataList.Clear();
            if (nodes == null)
            {
                return;
            }

            nodeDataList.AddRange(nodes);
        }

        public void SetLinks(IEnumerable<NodeLinkData> links)
        {
            nodeLinks.Clear();
            if (links == null)
            {
                return;
            }

            nodeLinks.AddRange(links);
        }

        public void SetMainTable(StringTableCollection table)
        {
            mainTable = table;
        }

        public void SetRelatedTables(IEnumerable<StringTableCollection> tables)
        {
            relatedTables.Clear();
            if (tables == null)
            {
                return;
            }
            relatedTables.AddRange(tables);
        }

        public void AddNode(NodeData node)
        {
            if (node == null || string.IsNullOrEmpty(node.Guid))
            {
                return;
            }

            RemoveNode(node.Guid);
            nodeDataList.Add(node);
        }

        public bool RemoveNode(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            int removed = nodeDataList.RemoveAll(node => node.Guid == guid);
            nodeLinks.RemoveAll(link => link.BaseNodeGuid == guid || link.TargetNodeGuid == guid);
            if (startNodeGuid == guid)
            {
                startNodeGuid = string.Empty;
            }

            return removed > 0;
        }

        public void AddLink(NodeLinkData linkData)
        {
            nodeLinks.RemoveAll(link =>
                link.BaseNodeGuid == linkData.BaseNodeGuid &&
                link.PortName == linkData.PortName);

            nodeLinks.Add(linkData);
        }

        public void AddRelatedTable(StringTableCollection table)
        {
            if (table == null || relatedTables.Contains(table))
            {
                return;
            }
            relatedTables.Add(table);
        }

        public void RemoveRelatedTable(StringTableCollection table)
        {
            if (table == null)
            {
                return;
            }
            relatedTables.Remove(table);
        }

        public bool TryGetNode(string guid, out NodeData node)
        {
            node = null;
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            node = nodeDataList.Find(n => n.Guid == guid);
            return node != null;
        }

        public IReadOnlyList<NodeData> GetConnectedNodes(string baseGuid)
        {
            List<NodeData> results = new List<NodeData>();
            if (string.IsNullOrEmpty(baseGuid))
            {
                return results;
            }

            foreach (var link in nodeLinks)
            {
                if (link.BaseNodeGuid != baseGuid)
                {
                    continue;
                }

                if (TryGetNode(link.TargetNodeGuid, out var node))
                {
                    results.Add(node);
                }
            }

            return results;
        }
    }
}
