// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2009 LShift Ltd., Cohesive Financial
//   Technologies LLC., and Rabbit Technologies Ltd.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v1.1:
//
//---------------------------------------------------------------------------
//   The contents of this file are subject to the Mozilla Public License
//   Version 1.1 (the "License"); you may not use this file except in
//   compliance with the License. You may obtain a copy of the License at
//   http://www.rabbitmq.com/mpl.html
//
//   Software distributed under the License is distributed on an "AS IS"
//   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//   License for the specific language governing rights and limitations
//   under the License.
//
//   The Original Code is The RabbitMQ .NET Client.
//
//   The Initial Developers of the Original Code are LShift Ltd,
//   Cohesive Financial Technologies LLC, and Rabbit Technologies Ltd.
//
//   Portions created before 22-Nov-2008 00:00:00 GMT by LShift Ltd,
//   Cohesive Financial Technologies LLC, or Rabbit Technologies Ltd
//   are Copyright (C) 2007-2008 LShift Ltd, Cohesive Financial
//   Technologies LLC, and Rabbit Technologies Ltd.
//
//   Portions created by LShift Ltd are Copyright (C) 2007-2009 LShift
//   Ltd. Portions created by Cohesive Financial Technologies LLC are
//   Copyright (C) 2007-2009 Cohesive Financial Technologies
//   LLC. Portions created by Rabbit Technologies Ltd are Copyright
//   (C) 2007-2009 Rabbit Technologies Ltd.
//
//   All Rights Reserved.
//
//   Contributor(s): ______________________________________.
//
//---------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace RabbitMQ.Client.Examples {
    public class PerfTest {
        private static readonly byte[] Message = new byte[0];

        public static int Main(string[] args) {
            try {
                if (args.Length < 2) {
                    Console.Error.WriteLine("Usage: PerfTest <hostname>[:<portnumber>] <number of messages>");
                    Console.Error.WriteLine("RabbitMQ .NET client version "+typeof(IModel).Assembly.GetName().Version.ToString());
                    return 1;
                }

                string serverAddress = args[0];
                int messageCount = int.Parse(args[1]);
                
                using (IConnection conn = new ConnectionFactory().CreateConnection(serverAddress))
                {
                    Stopwatch sendTimer = new Stopwatch();
                    Stopwatch sendUnkeyedTimer = new Stopwatch();
                    Stopwatch receiveTimer = new Stopwatch();
                        
                    using (IModel receiveCh = conn.CreateModel()) {
                        string q = receiveCh.QueueDeclare();
                        
                        using (IModel sendCh = conn.CreateModel()) {
                            sendTimer.Start();

                            for (int i = 0; i < messageCount; ++i) {
                                sendCh.BasicPublish("", q, null, Message);
                            }
                        }
                        sendTimer.Stop();
                        
                        using (IModel sendCh = conn.CreateModel()) {
                            sendUnkeyedTimer.Start();

                            for (int i = 0; i < messageCount; ++i) {
                                sendCh.BasicPublish("", "", null, Message);
                            }
                        }
                        sendUnkeyedTimer.Stop();

                        QueueingBasicConsumer consumer = new QueueingBasicConsumer(receiveCh);
                        receiveCh.BasicConsume(q, true, null, consumer);
                        receiveTimer.Start();
                        
                        for (int i = 0; i < messageCount; ++i) {
                            consumer.Queue.Dequeue();
                        }
                        receiveTimer.Stop();
                    }

                    Console.WriteLine("Performance Test Completed");
                    Console.WriteLine("Send (unkeyed): {0}Hz", ToHertz(sendUnkeyedTimer.ElapsedMilliseconds, messageCount));
                    Console.WriteLine("Send:           {0}Hz", ToHertz(sendTimer.ElapsedMilliseconds, messageCount));
                    Console.WriteLine("Receive:        {0}Hz", ToHertz(receiveTimer.ElapsedMilliseconds, messageCount));

                    return 0;
                }
            } catch (Exception e) {
                Console.Error.WriteLine(e);
                return 2;
            }
        }

        private static double ToHertz(long milliseconds, int messageCount) {
            double secsPerMessage = ((double) milliseconds)/((double) messageCount)/1000f;
            return 1/secsPerMessage;
        }
    }
}