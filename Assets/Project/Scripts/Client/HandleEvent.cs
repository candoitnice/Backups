using System.Collections;
using System.Collections.Generic;

public delegate void HandleEventServetState(GameServerState state);
public delegate void HandleEventUserChange(User user);
public delegate void HandleEventRoomChange(Room room);
public delegate void HandleGameProcessing(GameState state);
public delegate void HandeEventMove(int index);
public delegate void HandleEventHouseListEvent(List<House> house);
public delegate void HandleEventAdsEvent(List<Ads> ads);
public delegate void HandleEventUserLoginEvent(User user);
public delegate void HandleEventUserLoginOutEvent(User user);
public delegate void HandleEventGameResultEvent(GameResult result);
public delegate void HandleEventGameMsgEvent(GameMsg msg);

public delegate void HandleEventGameList(GameList game);

public delegate void HandleEventGameMsg(GameMsg msg);
public delegate void HandleEventGameProcess(GameDataPacket state);
public delegate void HandleEventSetting(KinectSetting setting);
public delegate void HandlerEventGetGift(User send, Gift gift);

public delegate void HandlerEventUserReCharge(User user, User enmy);


