import { ConnectionType } from "../../enums/ConnectionType";

export class ConnectionModel {
    type!: ConnectionType;
    userId!: number;
    usersCounter!: number;
  }