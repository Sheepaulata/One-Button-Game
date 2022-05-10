using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame {
	
    public class Tuto : Node2D
    {
        public static Tuto Instance { get; private set; }

        private Tuto ():base() {}

        [Export] protected PackedScene level;

        private Tween tween;

        private Sprite car;
        private Particles2D trail;
        private Sprite arrow;
        private Label space;
        private Label enter;
        private Label toDo;

        protected Button playBtn;

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(Tuto)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            playBtn = (Button)GetNode("Play");
            playBtn.Connect("pressed", this, nameof(ToPlay));

            tween = (Tween)GetNode("Tween");
            car = (Sprite)GetNode("Car");
            trail = (Particles2D)GetNode("Car/Trail");
            arrow = (Sprite)GetNode("Rotation");
            space = (Label)GetNode("Space");
            enter = (Label)GetNode("Enter");
            toDo = (Label)GetNode("ToDo");

            Push();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (Input.IsKeyPressed((int)KeyList.Enter)) {

            }
            if (Input.IsKeyPressed((int)KeyList.Space)) {
                (trail.ProcessMaterial as ParticlesMaterial).Gravity -= new Vector3(50, 0, 0);
            }
            else
                (trail.ProcessMaterial as ParticlesMaterial).Gravity = new Vector3(0, 0, 0);
        }


        private void Push()
        {
            tween.InterpolateProperty(car, "position:x", car.Position.x, car.Position.x + 575f, 3f, Tween.TransitionType.Elastic);
            tween.InterpolateProperty(arrow, "position:x", arrow.Position.x, arrow.Position.x - 575f, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut, 1f);
            tween.InterpolateProperty(space, "modulate:a", 0f, 1f, 1.2f, Tween.TransitionType.Quart, Tween.EaseType.In, 3f);
            tween.InterpolateProperty(enter, "modulate:a", 0f, 1f, 1.2f, Tween.TransitionType.Quart, Tween.EaseType.In, 3f);
            tween.InterpolateProperty(toDo, "modulate:a", 0f, 1f, 1.2f, Tween.TransitionType.Quart, Tween.EaseType.In, 3f);
            tween.InterpolateProperty(playBtn, "rect_position:y", playBtn.RectPosition.y, playBtn.RectPosition.y - 150, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut, 3.5f);
            tween.Start();
        }

        protected void ToPlay()
        {
            GetParent().AddChild(level.Instance());
            QueueFree();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}