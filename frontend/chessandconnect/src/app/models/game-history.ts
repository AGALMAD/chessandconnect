import { Play } from "./play"

export interface GamesHistory {
    id: number
    totalPages: number
    details: Play[]
}