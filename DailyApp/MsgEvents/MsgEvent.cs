using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace DailyApp.MsgEvents
{
    /// <summary>
    /// 发布订阅 消息model
    /// </summary>
    public class MsgEvent:PubSubEvent<string>
    {

    }
}
