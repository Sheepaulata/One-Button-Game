using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame {
	
    public class Main : Node
    {
        public static Main Instance { get; private set; }

        [Export] protected PackedScene menu;

        private Main ():base() {}

        public override void _Ready()
        {
            if (Instance != null){  
                Free();
                GD.Print($"{nameof(Main)} Instance already exist, destroying the last added.");
                return;
            }
            
            Instance = this;
            GD.Randomize();
            AddChild(menu.Instance());
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && Instance == this) 
                Instance = null;

            base.Dispose(pDisposing);
        }
    }
}