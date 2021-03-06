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
using System.IO;
using System.Collections.Generic;

using RabbitMQ.Client;

namespace RabbitMQ.Client.Content {
    ///<summary>Interface for constructing application messages.</summary>
    ///<remarks>
    /// Subinterfaces provide specialized data-writing methods. This
    /// base interface deals with the lowest common denominator:
    /// bytes, with no special encodings for higher-level objects.
    ///</remarks>
    public interface IMessageBuilder {
        ///<summary>Returns the default MIME content type for messages
        ///this instance constructs, or null if none is available or
        ///relevant.</summary>
	string GetDefaultContentType();

        ///<summary>Retrieves the dictionary that will be used to
        ///construct the message header table. It is of type <see cref="System.Collections.Generic.IDictionary{TKey,TValue}" /></summary>
        IDictionary<string, object> Headers { get; }

	///<summary>Retrieve the Stream being used to construct the message body.</summary>
	Stream BodyStream { get; }

	///<summary>Write a single byte into the message body, without
	///encoding or interpretation.</summary>
	IMessageBuilder RawWrite(byte b);

	///<summary>Write a byte array into the message body, without
	///encoding or interpretation.</summary>
	IMessageBuilder RawWrite(byte[] bytes);

	///<summary>Write a section of a byte array into the message
	///body, without encoding or interpretation.</summary>
	IMessageBuilder RawWrite(byte[] bytes, int offset, int length);

	///<summary>Finish and retrieve the content header for transmission.</summary>
	IContentHeader GetContentHeader();

	///<summary>Finish and retrieve the content body for transmission.</summary>
	byte[] GetContentBody();
    }
}
