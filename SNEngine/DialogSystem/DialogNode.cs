﻿using SiphoinUnityHelpers.XNodeExtensions;
using SiphoinUnityHelpers.XNodeExtensions.AsyncNodes;
using SNEngine.CharacterSystem;
using SNEngine.Repositories;
using SNEngine.Services;
using System.Collections.Generic;
using UnityEngine;

namespace SNEngine.DialogSystem
{
    public class DialogNode : AsyncNode, IDialogNode
    {
        [SerializeField] private Character _character;

        [SerializeField, TextArea] private string _text;

        public Character Character => _character;

        public override void Execute()
        {
            base.Execute();

            var serviceDialogs = NovelGame.GetService<DialogueUIService>();

            serviceDialogs.ShowDialog(this);
        }

        public string GetText()
        {
            return TextParser.ParseWithProperties(_text, graph as BaseGraph);
        }

        public int GetLengthText ()
        {
            return _text.Length;
        }

        public void MarkIsEnd ()
        {
            StopTask();

        }


    }
}