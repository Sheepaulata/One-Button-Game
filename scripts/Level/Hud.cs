using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class Hud : CanvasLayer
    {
        public static Hud Instance { get; private set; }

        private Hud ():base() {}

        [Export] PackedScene pause;

        public HBoxContainer top;
        private Label scoreTxt;
        private Label timerTxt;

        private Button btnPause;
        private Button btnContinue;
        private Button btnQuit;

        private const float TIME_PENALTY = 2.5f;
        public float score = 0;
        public float timer = 0;
        public TimeSpan timeInSecond;

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(Hud)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            top = (HBoxContainer)GetNode("Top");

            btnPause = (Button)GetNode<Button>("Top/Pause");
            btnPause.Connect("pressed", this, nameof(ToPause));

            RefreshUI();

            btnContinue = (Button)GetNode<Button>("Pause/Continue");
            btnContinue.Connect("pressed", this, nameof(ToContinue));

            btnQuit = (Button)GetNode<Button>("Pause/Quit");
            btnQuit.Connect("pressed", this, nameof(ToQuit));

        }

        public void AddTime(float pTime)
        {
            timer += pTime;
            timeInSecond = TimeSpan.FromSeconds(timer);
            RefreshUI();
        }

        public void AddScore()
        {
            score += TIME_PENALTY;
            RefreshUI();
        }

        private void ToPause()
        {
            GetNode<HBoxContainer>("Top").Visible = false;
            GetNode<VBoxContainer>("Pause").Visible = true;
            GetTree().Paused = true;
        }

        private void ToContinue()
        {
            GetNode<VBoxContainer>("Pause").Visible = false;
            GetNode<HBoxContainer>("Top").Visible = true;
            GetTree().Paused = false;
        }

        private void ToQuit()
        {
            GetTree().Quit();
        }

        void RefreshUI()
        {
            GetNode<Label>("Top/Score/Penalty").Text = "+ " + score.ToString();
            GetNode<Label>("Top/Score/Timer").Text = timeInSecond.ToString(@"mm\:ss");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}