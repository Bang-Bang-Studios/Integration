using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pentago_Networking
{
    class PentagoNetwork
    {
        enum SentDataType {move, privateChat, globalChat, idRequest, idResponse, connectRequest}

        const int PORT_NUMBER = 32458;
        const string GAME_NAME = "Dragon Horde";
        int myId = new Random().Next();
        static int otherId;
        System.Net.IPEndPoint myIPEndPoint;

        NetPeerConfiguration config;
        NetPeer peer;
        string peerName;
        bool peerOn = false;

        public List<peerType> availablePeers {get; private set;}


        public PentagoNetwork(string playerName)
        {
            availablePeers = new List<peerType>();

            config = new NetPeerConfiguration("Pentago");
            // Enable DiscoveryResponse messages
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.MaximumConnections = 32;
            config.Port = PORT_NUMBER;

            peer = new NetPeer(config);
            peer.Start();
            peerOn = true;

            Console.WriteLine("Peer Started");


            Thread t = new Thread(waitForMessages);
            peerName = playerName;
            t.Start();

            Console.WriteLine(peerName);

            peer.DiscoverLocalPeers(PORT_NUMBER);
        }

        public void discoverPeers()
        {
            peer.DiscoverLocalPeers(PORT_NUMBER);
        }

        private void waitForMessages()
        {
            while (peerOn)
            {
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
                            break;

                        case NetIncomingMessageType.ConnectionApproval:
                            msg.SenderConnection.Approve();
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

        private void ConnectToPeer(peerType peerToConnectTo)
        {
            peer.Connect(peerToConnectTo.address);
        }

        public void ConnectUsingIndex(int index)
        {
            ConnectToPeer(availablePeers[index]);
        }
    }
}
