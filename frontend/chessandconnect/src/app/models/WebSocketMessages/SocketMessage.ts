import { SocketCommunicationType } from "../../enums/SocketCommunicationType";

export class SocketMessage {
    type!: SocketCommunicationType;
}

export class SocketMessageGeneric<T> extends SocketMessage {
    data!: T;
}

export class GameSocketMessage<T> extends SocketMessageGeneric<T> {
    override type: SocketCommunicationType = SocketCommunicationType.GAME;
}

export class ConnectionSocketMessage<T> extends SocketMessageGeneric<T> {
    override type: SocketCommunicationType = SocketCommunicationType.CONNECTION;
}

export class ChatSocketMessage<T> extends SocketMessageGeneric<T> {
    override type: SocketCommunicationType = SocketCommunicationType.CHAT;
}
