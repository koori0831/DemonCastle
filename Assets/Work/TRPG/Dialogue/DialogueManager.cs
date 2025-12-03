using System;
using System.Collections.Generic;
using UnityEngine;
using Work.TRPG.Code;

namespace Work.TRPG.Dialogue
{
    public class DialogueManager : MonoBehaviour, IDialogueEvents
    {
        [SerializeField] private TRPGDiceSystem diceSystem;

        private DialogueContainerSO _currentContainer;
        private NodeData _currentNode;
        private readonly Dictionary<string, NodeData> _nodeLookup = new();

        public event Action<DialogueState> OnNodeEntered;
        public event Action OnDialogueEnded;
        public event Action<CheckNodeData> OnCheckNodeRequested;
        public event Action<ChoiceNodeData> OnChoiceNodePresented;

        public bool IsRunning => _currentContainer != null;
        public NodeData CurrentNode => _currentNode;

        public void StartDialogue(DialogueContainerSO container)
        {
            if (container == null)
            {
                Debug.LogError("DialogueManager: container is null");
                return;
            }

            _currentContainer = container;
            _nodeLookup.Clear();
            foreach (var node in container.NodeDataList)
            {
                if (string.IsNullOrEmpty(node.Guid))
                    continue;

                _nodeLookup[node.Guid] = node;
            }

            if (!_nodeLookup.TryGetValue(container.StartNodeGuid, out _currentNode))
            {
                Debug.LogError("DialogueManager: Start node not found");
                StopDialogue();
                return;
            }

            EnterNode(_currentNode);
        }

        public void StopDialogue()
        {
            _currentContainer = null;
            _currentNode = null;
            _nodeLookup.Clear();
            OnDialogueEnded?.Invoke();
        }

        public void Proceed()
        {
            if (!IsRunning)
                return;

            switch (_currentNode.NodeType)
            {
                case DialogueNodeType.Start:
                case DialogueNodeType.Dialogue:
                    MoveToNextFromSimpleNode();
                    break;
                case DialogueNodeType.Choice:
                    Debug.LogWarning("DialogueManager: Use SelectChoice for choice nodes");
                    break;
                case DialogueNodeType.Check:
                    PerformCheckNode();
                    break;
                case DialogueNodeType.End:
                    StopDialogue();
                    break;
            }
        }

        public void SelectChoice(string choiceGuid)
        {
            if (_currentNode is not ChoiceNodeData choiceNode)
            {
                Debug.LogWarning("DialogueManager: Current node is not a choice node");
                return;
            }

            foreach (var choice in choiceNode.Choices)
            {
                if (choice.ChoiceGuid != choiceGuid)
                    continue;

                MoveToNode(choice.NextNodeGuid);
                return;
            }

            Debug.LogWarning("DialogueManager: Choice guid not found");
        }

        public void ResolveCheck(CheckResult result)
        {
            if (_currentNode is not CheckNodeData)
            {
                Debug.LogWarning("DialogueManager: Current node is not a check node");
                return;
            }

            string nextGuid = FindCheckNodeOutput(_currentNode.Guid, result);
            MoveToNode(nextGuid);
        }

        private void EnterNode(NodeData node)
        {
            _currentNode = node;

            var state = new DialogueState
            {
                CurrentNodeGuid = node.Guid,
                NodeType = node.NodeType,
                NodeData = node
            };

            OnNodeEntered?.Invoke(state);

            if (node is ChoiceNodeData choiceNode)
            {
                OnChoiceNodePresented?.Invoke(choiceNode);
            }
            else if (node is CheckNodeData checkNode)
            {
                OnCheckNodeRequested?.Invoke(checkNode);
            }
        }

        private void MoveToNode(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                StopDialogue();
                return;
            }

            if (!_nodeLookup.TryGetValue(guid, out var nextNode))
            {
                Debug.LogWarning($"DialogueManager: Unable to find node {guid}");
                StopDialogue();
                return;
            }

            EnterNode(nextNode);
        }

        private void MoveToNextFromSimpleNode()
        {
            if (_currentNode == null)
            {
                return;
            }

            foreach (var link in _currentContainer.NodeLinks)
            {
                if (link.BaseNodeGuid != _currentNode.Guid)
                    continue;

                MoveToNode(link.TargetNodeGuid);
                return;
            }

            StopDialogue();
        }

        private void PerformCheckNode()
        {
            if (_currentNode is not CheckNodeData checkNode)
            {
                return;
            }

            if (diceSystem == null)
            {
                Debug.LogWarning("DialogueManager: DiceSystem is null");
                StopDialogue();
                return;
            }

            int statValue = GetStatValue(checkNode.TargetStat);
            if (checkNode.StatOverride >= 0)
            {
                statValue = checkNode.StatOverride;
            }

            CheckInfo result = diceSystem.RollDice(statValue);
            ResolveCheck(result.result);
        }

        private string FindCheckNodeOutput(string nodeGuid, CheckResult result)
        {
            string portName = result switch
            {
                CheckResult.CriticalSuccess => "Critical",
                CheckResult.Success => "Success",
                CheckResult.Failure => "Failure",
                CheckResult.Fumble => "Fumble",
                _ => string.Empty
            };

            foreach (var link in _currentContainer.NodeLinks)
            {
                if (link.BaseNodeGuid == nodeGuid && link.PortName == portName)
                {
                    return link.TargetNodeGuid;
                }
            }

            return string.Empty;
        }

        private int GetStatValue(StatType statType)
        {
            // TODO: Connect to actual character stats once available.
            return 50;
        }
    }
}
