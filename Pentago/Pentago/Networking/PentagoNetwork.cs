using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pentago
{
    // Handle new peer discovered.
    public delegate void peerDiscoveredHandler(object msg, EventArgs e);

    // Handle Connection Request
    public delegate void peerConnectionRequestHandler(object msg, EventArgs e);

    public delegate void peerConnectedHandler(object sender, EventArgs e);

    public delegate void peerDisconnectedHancler(object sender, EventArgs e);

    public delegate void moveReceivedHandler(object move, EventArgs e);

    public delegate void playerRemovedHandler(object pt, EventArgs e);

    public class PentagoNetwork
    {
        public event peerDiscoveredHandler Discovered;
        public event peerConnectionRequestHandler ConnectionRequest;
        public event peerConnectedHandler Connected;
        public event peerDisconnectedHancler Disconnected;
        public event moveReceivedHandler MoveReceived;
        public event playerRemovedHandler PlayerRemoved;

        enum SentDataType {move, privateChat, globalChat, idRequest, idResponse, connectRequest, ping, pingResponse}

        //const int PORT_NUMBER = 32458;
        const int PORT_NUMBER = 12345;
        const string GAME_NAME = "Dragon Horde";
        int myId = new Random().Next();
        static int otherId;
        System.Net.IPEndPoint myIPEndPoint;

        System.Net.IPEndPoint pendingConnectRequester;

        NetPeerConfiguration config;
        NetPeer peer;
        public string peerName;
        bool peerOn = false;

        public string clientName;
        public bool iAmPlayer1 = true;

        public List<peerType> availablePeers {get; private set;}

        private DateTime lastPingTime;
        private DateTime nextPingTime;

        public PentagoNetwork(string playerName)
        {
            lastPingTime = DateTime.Now;
            nextPingTime = lastPingTime.AddSeconds(1);

            availablePeers = new List<peerType>();

            config = new NetPeerConfiguration("Pentago");
            // Enable DiscoveryResponse messages
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.EnableMessageType(NetIncomingMessageType.Data);
            config.ConnectionTimeout = 5;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 32;
            config.Port = PORT_NUMBER;

            peer = new NetPeer(config);
            try
            {
                peer.Start();
                peerOn = true;

                Console.WriteLine("Peer Started");


                Thread t = new Thread(waitForMessages);
                t.SetApartmentState(ApartmentState.STA);
                peerName = playerName;
                t.Start();

                Console.WriteLine(peerName);

                peer.DiscoverLocalPeers(PORT_NUMBER);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Caught exception: " + ex.Data);
            }
        }

        public void stop()
        {
            peer.Shutdown(peerName + " Shutting Down");
        }

        public void discoverPeers()
        {
            peer.DiscoverLocalPeers(PORT_NUMBER);
        }

        private void waitForMessages()
        {
            while (peerOn)
            {
                if (DateTime.Now > nextPingTime)
                {
                    for (int i = 0; i < availablePeers.Count; i++) //foreach (peerType p in availablePeers)
                    {
                        peerType p = availablePeers[i];
                        NetOutgoingMessage ping = peer.CreateMessage();
                        ping.Write((short)SentDataType.ping);
                        peer.SendUnconnectedMessage(ping, p.address);
                    }
                    lastPingTime = DateTime.Now;
                    nextPingTime = lastPingTime.AddSeconds(1);
                }

                for (int i = 0; i < availablePeers.Count; i++) //foreach (peerType p in availablePeers)
                {
                    peerType p = availablePeers[i];
                    if (p.lastPing < DateTime.Now.AddSeconds(-2))
                    {
                        availablePeers.Remove(p);
                        PlayerRemoved(p, EventArgs.Empty);
                    }
                }

                NetIncomingMessage msg;
                while ((msg = peer.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(msg.ReadString());
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            // In case status changed
                            // It can be one of these
                            // NetConnectionStatus.Connected;
                            // NetConnectionStatus.Connecting;
                            // NetConnectionStatus.Disconnected;
                            // NetConnectionStatus.Disconnecting;
                            // NetConnectionStatus.None;

                            // NOTE: Disconnecting and Disconnected are not instant unless client is shutdown with disconnect()
                            Console.WriteLine(msg.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)msg.SenderConnection.Status);

                            if(msg.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                if (Connected != null)
                                {
                                    foreach (peerType p in availablePeers)
                                    {
                                        if (p.address.Equals(peer.Connections[0].RemoteEndPoint))
                                        {
                                            clientName = p.name;
                                        }
                                    }
                                    Connected(msg, EventArgs.Empty);
                                }
                            }

                            if (msg.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                if (Disconnected != null)
                                {
                                    Disconnected(msg, EventArgs.Empty);
                                }
                            }
                            break;

                        case NetIncomingMessageType.ConnectionApproval:
                            msg.SenderConnection.Approve();
                            iAmPlayer1 = false;
                            break;

                        case NetIncomingMessageType.DiscoveryRequest:
                            #region old stuff
                            //NetOutgoingMessage resp = peer.CreateMessage();
                            //resp.Write(peerName);
                            //resp.Write(GAME_NAME);
                            //peer.SendDiscoveryResponse(resp, msg.SenderEndPoint);
                            
                            //System.Net.IPAddress myAddress;
                            //System.Net.IPAddress myAddressMask;
                            //myAddress = NetUtility.GetMyAddress(out myAddressMask);

                            //bool same = msg.SenderEndPoint.Address.Equals(myAddress);
                            //if (!same)
                            //{
                            //    bool discoverSender = true;
                            //    foreach (peerType p in availablePeers)
                            //    {
                            //        if (p.address.Equals(msg.SenderEndPoint.Address))
                            //        {
                            //            discoverSender = false;
                            //            break;
                            //        }
                            //    }
                            //    if (discoverSender)
                            //    {
                            //        peer.DiscoverKnownPeer(msg.SenderEndPoint);
                            //    }
                            //}
                            #endregion
                            Console.WriteLine("Type = DiscoveryRequest; Sender = " + msg.SenderEndPoint.ToString());

                            getPeerId(msg.SenderEndPoint);

                            if (!listContainsIPEndPoint(msg.SenderEndPoint) && msg.SenderEndPoint != myIPEndPoint)
                            {
                                peer.DiscoverKnownPeer(msg.SenderEndPoint);
                            }

                            break;

                        case NetIncomingMessageType.DiscoveryResponse:
                            Console.WriteLine("Type = DiscoveryResponse; Sender = " + msg.SenderEndPoint);

                            string foundPeerName = msg.ReadString();
                            string foundGameName = msg.ReadString();
                            Console.WriteLine(foundPeerName);
                            Console.WriteLine(foundGameName);
                            addPeerToList(foundPeerName, foundGameName, msg.SenderEndPoint);
                            Console.WriteLine("Available Peers:");
                            foreach (peerType p in availablePeers)
                            {
                                Console.WriteLine(p.name);
                            }
                            // Raise event causing lobby list to be updated.
                            if (Discovered != null)
                            {
                                Discovered(msg, EventArgs.Empty);
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            #region Data
                            //Console.ForegroundColor = ConsoleColor.Cyan;
                            //Console.WriteLine(msg.ReadString());
                            //Console.ResetColor();

                            short type = msg.ReadInt16();

                            //Console.WriteLine("Type = Data; Subtype = " + type + " Sender = " + msg.SenderEndPoint);

                            if (type == (short)SentDataType.move)
                            {
                                Console.WriteLine("Type = move; Sender = " + msg.SenderEndPoint);

                                moveType move = new moveType();
                                move.quad = msg.ReadInt16();
                                move.position = msg.ReadInt16();
                                move.isClockwise = msg.ReadBoolean();
                                if (MoveReceived != null)
                                {
                                    MoveReceived(move, EventArgs.Empty);
                                }
                            }
                            else if (type == (short)SentDataType.globalChat)
                            {
                                string message = msg.ReadString();
                            }
                            else if (type == (short)SentDataType.privateChat)
                            {
                                string message = msg.ReadString();
                            }
                            else if (type == (short)SentDataType.idRequest)
                            {
                                Console.WriteLine("Type = idRequest; Sender = " + msg.SenderEndPoint);

                                sendId(msg.SenderEndPoint);
                            }
                            else if (type == (short)SentDataType.idResponse)
                            {
                                Console.WriteLine("Type = idResponse; Sender = " + msg.SenderEndPoint);

                                otherId = msg.ReadInt32();

                                // Discovery Response
                                if (otherId != myId)
                                {
                                    NetOutgoingMessage resp = peer.CreateMessage();
                                    resp.Write(peerName);
                                    resp.Write(GAME_NAME);
                                    peer.SendDiscoveryResponse(resp, msg.SenderEndPoint);
                                }
                                else
                                {
                                    myIPEndPoint = msg.SenderEndPoint;
                                }
                            }
                            break;
                            #endregion

                        case NetIncomingMessageType.UnconnectedData:
                            #region Unconnected Data
                            type = msg.ReadInt16();

                            if (type == (short)SentDataType.move)
                            {
                                Console.WriteLine("Type = move; Sender = " + msg.SenderEndPoint);

                                short quadrant = msg.ReadInt16();
                                short position = msg.ReadInt16();
                                bool isClockwise = msg.ReadBoolean();
                            }
                            else if (type == (short)SentDataType.globalChat)
                            {
                                string message = msg.ReadString();
                            }
                            else if (type == (short)SentDataType.privateChat)
                            {
                                string message = msg.ReadString();
                            }
                            else if (type == (short)SentDataType.idRequest)
                            {
                                Console.WriteLine("Type = idRequest; Sender = " + msg.SenderEndPoint);

                                sendId(msg.SenderEndPoint);
                            }
                            else if (type == (short)SentDataType.idResponse)
                            {
                                Console.WriteLine("Type = idResponse; Sender = " + msg.SenderEndPoint);

                                otherId = msg.ReadInt32();

                                // Discovery Response
                                if (otherId != myId)
                                {
                                    NetOutgoingMessage resp = peer.CreateMessage();
                                    resp.Write(peerName);
                                    resp.Write(GAME_NAME);
                                    peer.SendDiscoveryResponse(resp, msg.SenderEndPoint);
                                }
                                else
                                {
                                    myIPEndPoint = msg.SenderEndPoint;
                                }
                            }
                            else if (type == (short)SentDataType.connectRequest)
                            {
                                // Raise event causing user to get connection request
                                if (ConnectionRequest != null)
                                {
                                    pendingConnectRequester = msg.SenderEndPoint;
                                    ConnectionRequest(msg.ReadString(), EventArgs.Empty);
                                }
                            }
                            else if (type == (short)SentDataType.ping)
                            {
                                if (peer.Connections.Count == 0)
                                {
                                    NetOutgoingMessage resp = peer.CreateMessage();
                                    resp.Write((short)SentDataType.pingResponse);
                                    peer.SendUnconnectedMessage(resp, msg.SenderEndPoint);
                                }
                            }
                            else if (type == (short)SentDataType.pingResponse)
                            {
                                foreach(peerType p in availablePeers)
                                {
                                    if (p.address == msg.SenderEndPoint)
                                    {
                                        p.lastPing = DateTime.Now;
                                    }
                                }
                            }
                            break;
                            #endregion

                        default:
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;
                    }
                    peer.Recycle(msg);
                }
            }
        }

        private void addPeerToList(string foundPeerName, string foundPeerGameName, System.Net.IPEndPoint foundPeerIPEndPoint)
        {
            if (myIPEndPoint != null)
            {
                peerType peerToAdd = new peerType();
                peerToAdd.address = foundPeerIPEndPoint;
                peerToAdd.name = foundPeerName;
                peerToAdd.lastPing = DateTime.Now;

                bool same = foundPeerIPEndPoint == myIPEndPoint;
                if (!same)
                {
                    bool add = true;
                    foreach (peerType p in availablePeers)
                    {
                        if (p.address.Equals(peerToAdd.address))
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add)
                    {
                        availablePeers.Add(peerToAdd);
                    }
                }
            }
        }

        private bool listContainsIPEndPoint(System.Net.IPEndPoint endPoint)
        {
            foreach (peerType p in availablePeers)
            {
                if (p.address.Equals(endPoint))
                {
                    return true;
                }
            }
            return false;
        }

        private void getPeerId(System.Net.IPEndPoint recipient)
        {
            Console.WriteLine("Sending getPeerId to " + recipient);
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((short)SentDataType.idRequest);            
            peer.SendUnconnectedMessage(message, recipient);
        }

        private void sendId(System.Net.IPEndPoint recipient)
        {
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((short)SentDataType.idResponse);
            message.Write(myId);
            peer.SendUnconnectedMessage(message, recipient);
        }

        public class peerType
        {
            public System.Net.IPEndPoint address { get; set; }
            public string name { get; set; }
            public peerType() { }
            public DateTime lastPing { get; set; }
        }

        public class moveType
        {
            public short quad { get; set; }
            public short position { get; set; }
            public bool isClockwise { get; set; }
        }

        private void sendGlobalChat(string chatMessage)
        {
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((short)SentDataType.globalChat);
            message.Write(chatMessage);
            
        }

        public void SendMove(short quad, short position, bool isClockwise)
        {
            NetOutgoingMessage message = peer.CreateMessage();
            message.Write((short)SentDataType.move);
            message.Write(quad);
            message.Write(position);
            message.Write(isClockwise);
            peer.SendMessage(message, peer.Connections[0], NetDeliveryMethod.ReliableOrdered);
        }

        //private void RaiseDiscovered(object msg, EventArgs e)
        //{
        //    Discovered(msg, e);
        //}

        private void ConnectToPeer(peerType peerToConnectTo)
        {
            NetOutgoingMessage msg = peer.CreateMessage();
            msg.Write((short)SentDataType.connectRequest);
            msg.Write(peerName);
            peer.SendUnconnectedMessage(msg, peerToConnectTo.address);
        }

        public void ConnectUsingIndex(int index)
        {
            ConnectToPeer(availablePeers[index]);
        }

        public void AcceptConnection()
        {
            if (pendingConnectRequester != null)
            {
                peer.Connect(pendingConnectRequester);
            }
        }

        public void DeclineConnection()
        {
            
            pendingConnectRequester = null;
        }
    }
}
