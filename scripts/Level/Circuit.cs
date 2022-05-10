using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class Circuit : KinematicBody2D
    {
        public static Circuit Instance { get; private set; }

        private Circuit():base() {}

        [Export] PackedScene winScreen;

        private Area2D win;

        public Node2D cameraOfsetZero;
        public Node2D cameraOfsetOne;
        public Node2D cameraOfsetTwo;

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(Circuit)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            cameraOfsetZero = (Node2D)GetNode("Camera0");
            cameraOfsetOne = (Node2D)GetNode("Camera1");
            cameraOfsetTwo = (Node2D)GetNode("Camera2");

            win = (Area2D)GetNode("Area2D");

            win.Connect("area_entered", this, nameof(OnCollision));
        }

        protected void OnCollision(Area2D pArea)
        {
            if (pArea == Player.Instance.area)
            {
                GetTree().Paused = true;
                AddChild(winScreen.Instance());
            }
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}