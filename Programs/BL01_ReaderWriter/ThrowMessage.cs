using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL01_ReaderWriter
{
    class ThrowMessage
    {
        /// <summary>
        /// 状態モニタに表示されるメッセージキュー
        /// </summary>
        public static Queue queue = new Queue();                        //モニタメッセージを保存するキュー
        public static AutoResetEvent qlock = new AutoResetEvent(true);  //キュー読み書き排他処理用

        public static void mesg(string m)
        {
            qlock.WaitOne();
            {
                queue.Enqueue(m);
            }
            qlock.Set();
        }
    }
}
