using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class Malus : Label
    {
        private Tween tween;

        public Malus ():base() {}

        public override void _Ready()
        {
            tween = (Tween)GetNode("Tween");
            AddMalus();
        }

        public void AddMalus()
        {
            tween.InterpolateProperty(this, "modulate:a", 0f, 1f, 1f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "rect_scale:x", 1f, 1.5f, 1f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "rect_scale:y", 1f, 1.5f, 1f, Tween.TransitionType.Linear);
            //tween.InterpolateProperty(this, "rect_position", RectPosition, new Vector2(RectPosition.x, RectPosition.y - 50), 2.5f, Tween.TransitionType.Linear);
            tween.InterpolateProperty(this, "modulate:a", 1f, 0f, 2.5f, Tween.TransitionType.Linear);
            tween.InterpolateCallback(this, 3.2f, "Remove");
            tween.Start();
        }

        private void Remove()
        {
            QueueFree();
        }

    }
}