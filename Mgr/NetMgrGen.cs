using Google.Protobuf;
using System.Collections.Generic;

namespace TestUDPServer
{
    /// <summary>
    /// NetMgr接收网络消息部分
    ///     由编辑器生成
    /// </summary>
    public partial class NetMgr
    {
        /// <summary>
        /// 接收到消息
        /// </summary>
        private void OnReceive(NetMessageId messageId, byte[] contentBytes)
        {
            IMessage data = null;
            switch (messageId)
            {
                case NetMessageId.ReqCharacterInfo:
                    data = ProtobufGen.ReqCharacterInfo.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqCreateCharacter:
                    data = ProtobufGen.ReqCreateCharacter.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqLogin:
                    data = ProtobufGen.ReqLogin.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqLogout:
                    data = ProtobufGen.ReqLogout.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqRegister:
                    data = ProtobufGen.ReqRegister.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqVisitorLogin:
                    data = ProtobufGen.ReqVisitorLogin.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqCallBoss:
                    data = ProtobufGen.ReqCallBoss.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqPushCard:
                    data = ProtobufGen.ReqPushCard.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqPassCard:
                    data = ProtobufGen.ReqPassCard.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqEnterRoom:
                    data = ProtobufGen.ReqEnterRoom.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqExitRoom:
                    data = ProtobufGen.ReqExitRoom.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqReady:
                    data = ProtobufGen.ReqReady.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqQuitReady:
                    data = ProtobufGen.ReqQuitReady.Parser.ParseFrom(contentBytes);
                    break;
                case NetMessageId.ReqQuickVoice:
                    data = ProtobufGen.ReqQuickVoice.Parser.ParseFrom(contentBytes);
                    break;
            }

            if (data != null)
            {
                CallNetMessageEvent(messageId, data);
            }
        }

        #region 发送消息

        /// <summary>
        /// 下发角色信息
        /// </summary>
        /// <param name="resCode">返回码（0 成功 1 角色不存在）</param>
        /// <param name="characterBean">角色，返回码为0时有效</param>
        public void ResCharacterInfo(int resCode, ProtobufGen.CharacterBean characterBean)
        {
            ProtobufGen.ResCharacterInfo data = new ProtobufGen.ResCharacterInfo();
            data.ResCode = resCode;
            data.CharacterBean = characterBean;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResCharacterInfo, data);
            Send(NetMessageId.ResCharacterInfo, totalBytes);
        }

        /// <summary>
        /// 下发创建角色
        /// </summary>
        /// <param name="resCode">返回码（0 成功 1 昵称不合法 2 昵称已存在）</param>
        /// <param name="characterBean">角色实体</param>
        public void ResCreateCharacter(int resCode, ProtobufGen.CharacterBean characterBean)
        {
            ProtobufGen.ResCreateCharacter data = new ProtobufGen.ResCreateCharacter();
            data.ResCode = resCode;
            data.CharacterBean = characterBean;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResCreateCharacter, data);
            Send(NetMessageId.ResCreateCharacter, totalBytes);
        }

        /// <summary>
        /// 下发错误提示
        /// </summary>
        /// <param name="errorNotice">提示内容</param>
        public void ResErrorNotice(string errorNotice)
        {
            ProtobufGen.ResErrorNotice data = new ProtobufGen.ResErrorNotice();
            data.ErrorNotice = errorNotice;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResErrorNotice, data);
            Send(NetMessageId.ResErrorNotice, totalBytes);
        }

        /// <summary>
        /// 下发登录
        /// </summary>
        /// <param name="resCode">返回码，0 成功 1 账号不存在 2 账号密码不匹配 3 账号已在线</param>
        /// <param name="userId">用户id，返回码为0时有效</param>
        /// <param name="username">用户名，返回码为0时有效</param>
        public void ResLogin(int resCode, long userId, string username)
        {
            ProtobufGen.ResLogin data = new ProtobufGen.ResLogin();
            data.ResCode = resCode;
            data.UserId = userId;
            data.Username = username;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResLogin, data);
            Send(NetMessageId.ResLogin, totalBytes);
        }

        /// <summary>
        /// 下发登出
        /// </summary>
        public void ResLogout()
        {
            ProtobufGen.ResLogout data = new ProtobufGen.ResLogout();
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResLogout, data);
            Send(NetMessageId.ResLogout, totalBytes);
        }

        /// <summary>
        /// 下发注册
        /// </summary>
        /// <param name="resCode">返回码，0 成功 1 账号不合法 2 密码不合法 3 账号已存在</param>
        /// <param name="userId">用户id，返回码为0时有效</param>
        /// <param name="username">用户名，返回码为0时有效</param>
        public void ResRegister(int resCode, long userId, string username)
        {
            ProtobufGen.ResRegister data = new ProtobufGen.ResRegister();
            data.ResCode = resCode;
            data.UserId = userId;
            data.Username = username;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResRegister, data);
            Send(NetMessageId.ResRegister, totalBytes);
        }

        /// <summary>
        /// 下发手牌变化
        /// </summary>
        /// <param name="reqCode">手牌变化类型（1 减少 2 增加）</param>
        /// <param name="handCards">变化的手牌</param>
        public void ResHandCards(int reqCode, IEnumerable<int> handCards)
        {
            ProtobufGen.ResHandCards data = new ProtobufGen.ResHandCards();
            data.ReqCode = reqCode;
            data.HandCards.AddRange(handCards);
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResHandCards, data);
            Send(NetMessageId.ResHandCards, totalBytes);
        }

        /// <summary>
        /// 下发叫地主座次（一人只能叫一次）
        /// </summary>
        /// <param name="willCallBossSeat">将要叫地主座次</param>
        public void ResWillCallBossSeat(int willCallBossSeat)
        {
            ProtobufGen.ResWillCallBossSeat data = new ProtobufGen.ResWillCallBossSeat();
            data.WillCallBossSeat = willCallBossSeat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResWillCallBossSeat, data);
            Send(NetMessageId.ResWillCallBossSeat, totalBytes);
        }

        /// <summary>
        /// 下发叫地主或不叫地主
        /// </summary>
        /// <param name="seat">请求的玩家的座次</param>
        /// <param name="resCode">叫地主或不叫（1 叫地主 2 不叫）</param>
        /// <param name="bossSeat">地主座次，未确定为-1</param>
        public void ResCallBoss(int seat, int resCode, int bossSeat)
        {
            ProtobufGen.ResCallBoss data = new ProtobufGen.ResCallBoss();
            data.Seat = seat;
            data.ResCode = resCode;
            data.BossSeat = bossSeat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResCallBoss, data);
            Send(NetMessageId.ResCallBoss, totalBytes);
        }

        /// <summary>
        /// 下发底牌
        /// </summary>
        /// <param name="holeCards">底牌</param>
        public void ResHoldCard(IEnumerable<int> holeCards)
        {
            ProtobufGen.ResHoldCard data = new ProtobufGen.ResHoldCard();
            data.HoleCards.AddRange(holeCards);
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResHoldCard, data);
            Send(NetMessageId.ResHoldCard, totalBytes);
        }

        /// <summary>
        /// 下发出牌座次
        /// </summary>
        /// <param name="seat">座次</param>
        public void ResPushSeat(int seat)
        {
            ProtobufGen.ResPushSeat data = new ProtobufGen.ResPushSeat();
            data.Seat = seat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResPushSeat, data);
            Send(NetMessageId.ResPushSeat, totalBytes);
        }

        /// <summary>
        /// 下发出牌
        /// </summary>
        /// <param name="seat">出牌的座次</param>
        /// <param name="pushCards">出的牌</param>
        public void ResPushCard(int seat, IEnumerable<int> pushCards)
        {
            ProtobufGen.ResPushCard data = new ProtobufGen.ResPushCard();
            data.Seat = seat;
            data.PushCards.AddRange(pushCards);
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResPushCard, data);
            Send(NetMessageId.ResPushCard, totalBytes);
        }

        /// <summary>
        /// 下发不出
        /// </summary>
        /// <param name="seat">不出的座次</param>
        public void ResPassCard(int seat)
        {
            ProtobufGen.ResPassCard data = new ProtobufGen.ResPassCard();
            data.Seat = seat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResPassCard, data);
            Send(NetMessageId.ResPassCard, totalBytes);
        }

        /// <summary>
        /// 下发游戏结束
        /// </summary>
        public void ResGameOver()
        {
            ProtobufGen.ResGameOver data = new ProtobufGen.ResGameOver();
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResGameOver, data);
            Send(NetMessageId.ResGameOver, totalBytes);
        }

        /// <summary>
        /// 下发进入房间
        /// </summary>
        /// <param name="resCode">返回码，0 成功 1 房间不存在 2 房间已满</param>
        /// <param name="seat">座次，从0开始，返回码为0时有效</param>
        /// <param name="roomCharacterBeans">房间内玩家，返回码为0时有效</param>
        public void ResEnterRoom(int resCode, int seat, IEnumerable<ProtobufGen.RoomCharacterBean> roomCharacterBeans)
        {
            ProtobufGen.ResEnterRoom data = new ProtobufGen.ResEnterRoom();
            data.ResCode = resCode;
            data.Seat = seat;
            data.RoomCharacterBeans.AddRange(roomCharacterBeans);
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResEnterRoom, data);
            Send(NetMessageId.ResEnterRoom, totalBytes);
        }

        /// <summary>
        /// 下发退出房间
        /// </summary>
        public void ResExitRoom()
        {
            ProtobufGen.ResExitRoom data = new ProtobufGen.ResExitRoom();
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResExitRoom, data);
            Send(NetMessageId.ResExitRoom, totalBytes);
        }

        /// <summary>
        /// 下发其他玩家进入房间
        /// </summary>
        /// <param name="roomCharacterBean">房间角色</param>
        public void ResOtherEnterRoom(ProtobufGen.RoomCharacterBean roomCharacterBean)
        {
            ProtobufGen.ResOtherEnterRoom data = new ProtobufGen.ResOtherEnterRoom();
            data.RoomCharacterBean = roomCharacterBean;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResOtherEnterRoom, data);
            Send(NetMessageId.ResOtherEnterRoom, totalBytes);
        }

        /// <summary>
        /// 下发其他玩家退出房间
        /// </summary>
        /// <param name="seat">座次</param>
        public void ResOtherExitRoom(int seat)
        {
            ProtobufGen.ResOtherExitRoom data = new ProtobufGen.ResOtherExitRoom();
            data.Seat = seat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResOtherExitRoom, data);
            Send(NetMessageId.ResOtherExitRoom, totalBytes);
        }

        /// <summary>
        /// 下发准备
        /// </summary>
        /// <param name="seat">座次</param>
        public void ResReady(int seat)
        {
            ProtobufGen.ResReady data = new ProtobufGen.ResReady();
            data.Seat = seat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResReady, data);
            Send(NetMessageId.ResReady, totalBytes);
        }

        /// <summary>
        /// 下发取消准备
        /// </summary>
        /// <param name="seat">座次</param>
        public void ResQuitReady(int seat)
        {
            ProtobufGen.ResQuitReady data = new ProtobufGen.ResQuitReady();
            data.Seat = seat;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResQuitReady, data);
            Send(NetMessageId.ResQuitReady, totalBytes);
        }

        /// <summary>
        /// 下发快捷语音
        /// </summary>
        /// <param name="cfgId">配置id</param>
        public void ResQuickVoice(int cfgId)
        {
            ProtobufGen.ResQuickVoice data = new ProtobufGen.ResQuickVoice();
            data.CfgId = cfgId;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResQuickVoice, data);
            Send(NetMessageId.ResQuickVoice, totalBytes);
        }

        /// <summary>
        /// 下发下注信息
        /// </summary>
        /// <param name="multiple">倍数</param>
        /// <param name="beanNum">欢乐豆数量</param>
        public void ResBet(int multiple, int beanNum)
        {
            ProtobufGen.ResBet data = new ProtobufGen.ResBet();
            data.Multiple = multiple;
            data.BeanNum = beanNum;
            byte[] totalBytes = CreateTotalBytes(NetMessageId.ResBet, data);
            Send(NetMessageId.ResBet, totalBytes);
        }

        #endregion
    }
}
