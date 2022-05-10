using Com.IsartDigital.OneButtonGame;
using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class LevelContainer : Node2D
    {
        public static LevelContainer Instance { get; private set; }

        [Export] private PackedScene playerFactory;
        private Player player;

        [Export] private PackedScene circuitFactory;
        [Export] private PackedScene lightFactory;

        [Export] private PackedScene hudFactory;
        private Hud hud;

        private Tween tween;

        public float tweenTimer;

        protected bool pause;


        private LevelContainer ():base() {}

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(LevelContainer)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            tween = (Tween)GetNode("Tween");

            StartGame();
        }

        private Player CreatePlayer() => (Player)playerFactory.Instance();
        private Hud CreateHud() => (Hud)hudFactory.Instance();

        private void StartGame()
        {
            tweenTimer = 0;
            AddChild(circuitFactory.Instance());
            AddPlayer();
            AddChild(lightFactory.Instance());
            ZoomCamera();
            AddHud();
        }

        private void AddPlayer()
        {
            player = CreatePlayer();
            AddChild(player);
            player.RotationDegrees -= 90;
            player.rotationTarget -= 90;
            player.ApparitionFade();
        }


        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            pause = Input.IsKeyPressed((int)KeyList.Space);
        }

        private void ZoomCamera()
        {
            float lTimer = 0;
            Player.Instance.camera.Zoom = Vector2.One * 2f;
            
            tween.InterpolateProperty(Player.Instance.camera, "global_position", Circuit.Instance.cameraOfsetZero.GlobalPosition, Circuit.Instance.cameraOfsetOne.GlobalPosition, 1f, Tween.TransitionType.Linear, Tween.EaseType.In, lTimer);
            tween.InterpolateProperty(Player.Instance.camera, "global_position", Circuit.Instance.cameraOfsetOne.GlobalPosition, Circuit.Instance.cameraOfsetTwo.GlobalPosition, 1f, Tween.TransitionType.Linear, Tween.EaseType.In, lTimer += 1f);
            tween.InterpolateProperty(Player.Instance.camera, "global_position", Circuit.Instance.cameraOfsetTwo.GlobalPosition, Vector2.Zero, 0.5f, Tween.TransitionType.Linear, Tween.EaseType.In, lTimer += 1f);
            tween.InterpolateProperty(Player.Instance.camera, "zoom", Player.Instance.camera.Zoom, Vector2.One, 1f, Tween.TransitionType.Circ, Tween.EaseType.In, lTimer += 0.5f);
            tween.Start();
        }

        private void AddHud()
        {
            hud = CreateHud();
            AddChild(hud);
            tween.InterpolateProperty(hud.top, "modulate:a", 0f, 1f, 1.2f, Tween.TransitionType.Quart, Tween.EaseType.In, 5.2f);
            tween.Start();
        }

        public override void _Process(float deltaTime)
        {
            tween.InterpolateCallback(hud, 5.8f, "AddTime", deltaTime);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}