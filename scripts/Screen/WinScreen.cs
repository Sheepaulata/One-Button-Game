using Com.IsartDigital.OneButtonGame.Level;
using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame {
	
    public class WinScreen : CanvasLayer
    {
        public static WinScreen Instance { get; private set; }

        private WinScreen ():base() {}

        private Button btnQuit;

        private Tween tween;
        private Sprite back;
        private Control win;

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(WinScreen)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;

            btnQuit = (Button)GetNode<Button>("Win/Quit");
            btnQuit.Connect("pressed", this, nameof(ToQuit));

            tween = (Tween)GetNode("Tween");
            back = (Sprite)GetNode("Win/Win");
            win = (Control)GetNode("Win");

            AddScreen();
            CarLeave();
        }

        private void CarLeave()
        {
            tween.InterpolateProperty(Player.Instance, "global_position:x", Player.Instance.GlobalPosition.x, Player.Instance.GlobalPosition.x - 1000f, 1f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(Player.Instance, "rotation_degrees", Player.Instance.RotationDegrees, -90, 1f, Tween.TransitionType.Linear);
            tween.Start();

            //Player.Instance.velocity
            Player.Instance.camera.Current = false;
        }

        private void AddScreen()
        {
            tween.InterpolateProperty(back, "position:y", back.Position.y, back.Position.y + 600, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut);
            tween.InterpolateProperty(win.Material, "shader_param/pixelFactor", 0f, 1f, 3f, Tween.TransitionType.Quart, Tween.EaseType.In, 0.5f);
            tween.InterpolateProperty(btnQuit, "rect_position:y", 650f, 450f, 3f, Tween.TransitionType.Elastic, Tween.EaseType.InOut, 0.5f);
            tween.Start();

            GetNode<Label>("Win/VBoxContainer/Score").Text = "Score : " + Hud.Instance.timeInSecond.Minutes + ":" + (Hud.Instance.timeInSecond.Seconds + Hud.Instance.score).ToString();
        }

        private void ToQuit()
        {
            GetTree().Quit();
        }
    }
}