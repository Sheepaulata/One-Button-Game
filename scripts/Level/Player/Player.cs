using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class Player : KinematicBody2D
    {
        public static Player Instance { get; private set; }
        public object Circuit { get; private set; }

        [Export] private float speed = 10f;
        [Export] private float bounciness = -0.9f;

        [Export] private PackedScene malusFactory;

        private Sprite arrow;
        private Tween tween;
        private Particles2D trail;
        private Sprite playerSprite;
        public Camera2D camera;
        public Area2D area;

        private float friction = 0.98f;
        public float rotationTarget;

        public Vector2 velocity = default;
        private Vector2 acceleration = default;

        protected bool isPressed;
        public bool playerOne;
        public bool playerTwo;

        private bool collided = false;

        private Player ():base() {}

        public override void _Ready()
        {
            if (Instance != null){
                Free();
                GD.Print($"{nameof(Player)} Instance already exist, destroying the last added.");
                return;
            }
            Instance = this;

            area = (Area2D)GetNode("Area2D");
            arrow = (Sprite)GetNode("Arrow");
            playerSprite = (Sprite)GetNode("Player");
            tween = (Tween)GetNode("Tween");
            trail = (Particles2D)GetNode("Trail");
            camera = (Camera2D)GetNode("Camera2D");
        }

        private Malus CreateMalus() => (Malus)malusFactory.Instance();

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            playerOne = Input.IsKeyPressed((int)KeyList.Enter);
            playerTwo = Input.IsKeyPressed((int)KeyList.Space);
        }


        public override void _Process(float deltaTime)
        {
            OnCollision();
            tween.InterpolateCallback(this, 6.2f, "Move", deltaTime);

        }

        private void Move(float pDeltaTime)
        {
            velocity += acceleration;
            velocity *= friction;

            if (playerOne && !isPressed)
            {
                rotationTarget = arrow.GlobalRotationDegrees;
                arrow.RotationDegrees = 0;
            }

            isPressed = playerOne;

            if (playerTwo)
                velocity += Vector2.Up.Rotated(Rotation) * speed * pDeltaTime;

            tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, rotationTarget, 0.3f, Tween.TransitionType.Quart, Tween.EaseType.Out);
            tween.Start();

            DrawTrail();
        }

        private void OnCollision()
        {
            KinematicCollision2D collider = MoveAndCollide(velocity);

            if (collider != null && velocity.Length() > 0.25f)
            {
                //CHANGE ROTATION
                //Vector2 result = velocity.Bounce(col.Normal);
                //rotationTarget = Vector2.Up.AngleTo(result) * 180 / Mathf.Pi;

                velocity *= bounciness;
                AddBounce();
                AddMalus();

                collided = true;
            }
            else if (collider == null)
                collided = false;

        }

        private void DrawTrail()
        {
            (trail.ProcessMaterial as ParticlesMaterial).Angle = -RotationDegrees;
            trail.SpeedScale = 1 + velocity.Length() * 0.2f;
            trail.Visible = true;
        }

        private void AddMalus()
        {
            if (collided != false) { 

                Malus malus = CreateMalus();
                LevelContainer.Instance.AddChild(malus);
                malus.RectGlobalPosition = Position - malus.RectScale * malus.RectSize * .5f;

                Hud.Instance.AddScore();
            }
        }

        private void AddBounce()
        {
            tween.InterpolateProperty(this, "scale:x", 0.7f, 0.5f, 0.2f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "scale:y", 0.7f, 1f, 0.2f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "scale:x", 0.5f, 0.7f, 0.2f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "scale:y", 1f, 0.7f, 0.2f, Tween.TransitionType.Linear);
            tween.Start();
        }

        public void ApparitionFade()
        {
            tween.InterpolateProperty(playerSprite.Material, "shader_param/pixelFactor", 0f, 1f, 3f, Tween.TransitionType.Quart, Tween.EaseType.In, 4f) ;
            tween.InterpolateProperty(arrow, "modulate:a", 0f, 1f, 2f, Tween.TransitionType.Quart, Tween.EaseType.In, 5f);
            tween.InterpolateProperty(trail, "modulate:a", 0f, 1f, 2f, Tween.TransitionType.Quart, Tween.EaseType.In, 5f);
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