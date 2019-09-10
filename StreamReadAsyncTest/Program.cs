//
// Program.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2019 Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Threading;

namespace StreamReadAsyncTest
{
	class MainClass
	{
		/// <summary>
		/// Uses AsyncPump from:
		/// https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/
		/// </summary>
		public static void Main (string[] args)
		{
			AsyncPump.Run (async () => {
				await RunTests ();
			});
			Console.ReadKey ();
		}

		static async Task RunTests ()
		{
			Console.WriteLine ("ThreadId={0} [Start]", Thread.CurrentThread.ManagedThreadId);

			var directory = Path.GetDirectoryName (typeof (MainClass).Assembly.Location);
			var fileName = Path.Combine (directory, "test.txt");

			await Task.Run (() => {
				Thread.Sleep (400);
			});

			// Thread id has not changed.
			Console.WriteLine ("ThreadId={0} [After await Task.Run]", Thread.CurrentThread.ManagedThreadId);

			using (Stream stream = File.OpenRead (fileName)) {
				int count = (int)stream.Length;
				byte[] buf = new byte[count];
				await stream.ReadAsync (buf, 0, count);

			}

			// Thread id has changed here.
			Console.WriteLine ("ThreadId={0} [After stream.ReadAsync]", Thread.CurrentThread.ManagedThreadId);
		}
	}
}
