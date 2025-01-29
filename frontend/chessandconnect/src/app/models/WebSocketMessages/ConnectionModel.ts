import { ConnectionType } from "../../enums/ConnectionType";

export class ConnectionModel {
  Type!: ConnectionType;
  UserId!: number;
  UsersCounter!: number;
}