import { SocketCommunicationType } from "../../enums/SocketCommunicationType";

export class SocketMessage {
    Type!: SocketCommunicationType;
}

export class SocketMessageGeneric<T> extends SocketMessage {
    Data!: T;
}

export class GameSocketMessage<T> extends SocketMessageGeneric<T> {
    override Type: SocketCommunicationType = SocketCommunicationType.GAME;
}

export class ConnectionSocketMessage<T> extends SocketMessageGeneric<T> {
    override Type: SocketCommunicationType = SocketCommunicationType.CONNECTION;
}

export class ChatSocketMessage<T> extends SocketMessageGeneric<T> {
    override Type: SocketCommunicationType = SocketCommunicationType.CHAT;
}
