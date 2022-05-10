using Com.IsartDigital.OneButtonGame.Level;
using Godot;
using System;

namespace Com.IsartDigital.OneButtonGame {
	
    public class Network : Node
    {

        private Network ():base() {}

        public WebSocketClient client = new WebSocketClient();

        public override void _Ready() {

            client.Connect("connection_error", this, nameof(OnConnectionError));
            client.Connect("connection_established", this, nameof(OnConnection));
            client.Connect("data_received", this, nameof(OnData));
            client.ConnectToUrl("ws://localhost:8080");
        }
        public void OnData(Godot.Collections.Dictionary dict)
        {
            var type = (string)dict["type"];
            if (type == "accelerating")
            {
                GD.Print("Remote player accelerating");
                Player.Instance.playerOne = true;
            }
            else if (type == "deccelerating")
            {
                GD.Print("Remote player deccelerating");
                Player.Instance.playerOne = false;
            }
            if (type == "rotate")
            {
                GD.Print("Remote player rotate");
                Player.Instance.playerTwo = true;
            }
            else if (type != "rotate")
            {
                GD.Print("Remote player rotate");
                Player.Instance.playerTwo = false;
            }
        }

        private void OnConnectionError()
        {
            GD.Print("connection error");
        }

        private void OnConnection()
        {
            GD.Print("connection established");

            /*var peer = client.GetPeer(1);

            var dict = new Godot.Collections.Dictionary();
            dict["message"] = "hello from client";*/
        }
        private void OnData()
        {
            GD.Print("data received");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            client.Poll();
        }
    }
}