using System;
using LiteNetLib;

namespace Battleships.Multiplayer.ClientSide;

public class Client : IDisposable {
  private NetManager client;
  private EventBasedNetListener listener;

  public Client(string ip) {
    System.Console.WriteLine("created client");
    listener = new EventBasedNetListener();
    client = new NetManager(listener);
    client.Start();
    client.Connect(ip, 9050, "SomeConnectionKey");

    listener.NetworkReceiveEvent += GetEvent;

    Poll();
  }

  private void GetEvent(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod) {
    Console.WriteLine("We got: {0}", reader.GetString(100));
    reader.Recycle();
  }

  public void Poll() {
    client.PollEvents();
  }

  public void Dispose() {
    client.Stop();
  }
}