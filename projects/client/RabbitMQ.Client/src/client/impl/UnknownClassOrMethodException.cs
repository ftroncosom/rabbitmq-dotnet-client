// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2013 GoPivotal, Inc.
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
//  The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License
//  at http://www.mozilla.org/MPL/
//
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
//  the License for the specific language governing rights and
//  limitations under the License.
//
//  The Original Code is RabbitMQ.
//
//  The Initial Developer of the Original Code is GoPivotal, Inc.
//  Copyright (c) 2007-2014 GoPivotal, Inc.  All rights reserved.
//---------------------------------------------------------------------------

using System;

// We use spec version 0-9 for common constants such as frame types,
// error codes, and the frame end byte, since they don't vary *within
// the versions we support*. Obviously we may need to revisit this if
// that ever changes.
using CommonFraming = RabbitMQ.Client.Framing.v0_9;

namespace RabbitMQ.Client.Impl
{
    /// <summary>
    /// Thrown when the protocol handlers detect an unknown class
    /// number or method number.
    /// </summary>
    public class UnknownClassOrMethodException : HardProtocolException
    {

        private ushort m_classId;
        private ushort m_methodId;

        ///<summary>The AMQP content-class ID.</summary>
        public ushort ClassId { get { return m_classId; } }

        ///<summary>The AMQP method ID within the content-class, or 0 if none.</summary>
        public ushort MethodId { get { return m_methodId; } }

        public UnknownClassOrMethodException(ushort classId, ushort methodId)
            : base(string.Format("The Class or Method <{0}.{1}> is unknown", classId, methodId))
        {
            m_classId = classId;
            m_methodId = methodId;
        }

        public override string ToString()
        {
            if (m_methodId == 0)
            {
                return base.ToString() + "<" + m_classId + ">";
            }
            else
            {
                return base.ToString() + "<" + m_classId + "." + m_methodId + ">";
            }
        }

        public override ushort ReplyCode { get { return CommonFraming.Constants.NotImplemented; } }
    }
}
