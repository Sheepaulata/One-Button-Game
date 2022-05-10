using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame.Level {
	
    public class Arrow : Sprite
    {

        [Export] private float rotationSpeed = 160f;
        private float minRotation = 0f;
        private float maxRotation = -10;

        private float direction = 1;

        public Arrow ():base() {}

        public override void _Ready()
        {

        }

        private float Direction()
        {
            if (RotationDegrees >= 60)
                direction = -1;
            else if (RotationDegrees <= -60)
                direction = 1;
            return direction;
        }

        public override void _Process(float deltaTime)
        {
            RotationDegrees += rotationSpeed * deltaTime * Direction();
        }
    }
}