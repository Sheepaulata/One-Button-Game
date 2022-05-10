using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame {
	
    public class Menu : Node
    {
        public static Menu Instance { get; private set; }

        public const string PRESSED = "pressed";

        [Export] protected PackedScene tuto;
        [Export] protected PackedScene level;

        [Export] protected NodePath playBtnPath;
        [Export] protected NodePath quitBtnPath;

        protected Button playBtn;
        protected Button quitBtn;
        protected Button helpBtn;

        private Sprite car;
        private Tween tween;

        private Menu ():base() {}

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(Menu)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            tween = (Tween)GetNode("Tween");
            car = (Sprite)GetNode("Car");

            playBtn = (Button)GetNode(playBtnPath);
            playBtn.Connect(PRESSED, this, nameof(ToPlay));

            quitBtn = (Button)GetNode(quitBtnPath);
            quitBtn.Connect(PRESSED, this, nameof(ToQuit));

            helpBtn = (Button)GetNode("HelpButton");
            helpBtn.Connect(PRESSED, this, nameof(ToHelp));

            Push();
        }

        private void Push()
        {
            tween.InterpolateProperty(car, "position:x", car.Position.x, car.Position.x + 1400f, 1.5f, Tween.TransitionType.Linear, Tween.EaseType.Out);
            tween.InterpolateProperty(playBtn, "rect_position:x", playBtn.RectPosition.x, playBtn.RectPosition.x + 750, 2f, Tween.TransitionType.Elastic);
            tween.InterpolateProperty(helpBtn, "rect_position:y", helpBtn.RectPosition.y, helpBtn.RectPosition.y - 210, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut, 1f);
            tween.InterpolateProperty(quitBtn, "rect_position:y", quitBtn.RectPosition.y, quitBtn.RectPosition.y - 210, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut, 1f);
            tween.Start();
        }

        protected void ToPlay()
        {
            GetParent().AddChild(level.Instance());
            QueueFree();
        }

        protected void ToHelp()
        {
            GetParent().AddChild(tuto.Instance());
            QueueFree();
        }

        protected void ToQuit()
        {
            GetTree().Quit();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            tween.Start();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}