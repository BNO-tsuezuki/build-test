import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';
import { start } from 'repl';


export enum InsertPos {
	Above=0,
	Below=1,
}


interface EntryPlayer {
  playerId: number;
  playerName: string;
  rating: number;
}

interface Entry {
  entryId: number;
  matchType: number;
  rating: number;
  players: EntryPlayer[];
}

interface AssignedPlayer {
  playerId: number;
  playerName: string;
  rating: number;
  groupNo: number;
  side: number;
}

interface BattleServer {
  matchId: string;
  state: number;

  sessionId:string;
  ipAddr:string;
  port:number;
  rule:string;
  mapId: string;
  label: string;
  description: string;
  serverName: string;
  region: string;
  owner: string;
  autoMatchmakeTarget:boolean;
  
  players:AssignedPlayer[];
}

interface Room {
  matchId: string;
  entries: number[][];
}
function defaultRoom(matchId:string): Room {
  return {
    matchId: matchId,
    entries: [[], []],
	};
}


export interface MatchingState{

  battleServers: BattleServer[];
  entries: Entry[];
  
  rooms: Room[];
  freeEntries: number[];

  matchingArea: number;
}

const defaultState: MatchingState = {

  battleServers:[],
  entries:[],

  rooms:[],
  freeEntries: [],

  matchingArea: -1,
};

//--------------------------------------------------------------
// Payload
//--------------------------------------------------------------
export interface SemiAutoMatchmakeRequest
{
  matchingArea: number;

  matchId: string;

  entryIds: number[];
}

export interface ForceMatchmakeRequest
{
  matchingArea: number;

  matchId: string;

  entries: { entryId: number; side: number; }[];
}



//--------------------------------------------------------------
// Utility
//--------------------------------------------------------------
interface EntryPos {
  idx?:number;
  roomIdx?:number;
  teamIdx?:number;
}

function SearchEntryPos(state: MatchingState, entryId: number): EntryPos {

  let idx = state.freeEntries.findIndex(id => id == entryId);
  if (0 <= idx) {
    return { idx:idx };
  }

  for (let i = 0; i < state.rooms.length; i++) {
    let room = state.rooms[i];
    for (let j = 0; j < room.entries.length; j++) {
      let team = room.entries[j];
      for (let k = 0; k < team.length; k++) {
        if (team[k] == entryId) {
          return { idx: k, teamIdx: j, roomIdx: i };
        }
      }
    }
  }

  return {};
}

function RemoveEntry(state: MatchingState, pos: EntryPos): void {

  if (pos.idx === undefined) {
    return;
  }

  if (pos.roomIdx !== undefined && pos.teamIdx !== undefined) {
    state.rooms[pos.roomIdx].entries[pos.teamIdx].splice(pos.idx, 1);
  }
  else {
    state.freeEntries.splice(pos.idx, 1);
  }
}



//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {
  getEntriesRequest: ACFactory<{ matchingArea: number}>('getEntriesRequest'),
  getEntriesResponse: ACFactory<{ battleServers: BattleServer[], entries: Entry[], matchingArea: number }>('getEntriesResponse'),
  semiAutoMatchmakeRequest: ACFactory<SemiAutoMatchmakeRequest>('semiAutoMatchmakeRequest'),
  semiAutoMatchmakeResponse: ACFactory<{ matchId: string, entries: { entryId:number, side:number}[] }>('semiAutoMatchmakeResponse'),
  forceMatchmakeRequest: ACFactory <ForceMatchmakeRequest>('forceMatchmakeRequest'),
  execCommandRequest: ACFactory<{ battleServerSessionId:string, command:string}>("execCommandRequest"),

  moveEntry1: ACFactory<{ entryIdSrc: number, entryIdDst: number, pos: InsertPos }>("moveEntry1"),
  moveEntry2: ACFactory<{ entryIdSrc: number, roomIdx?: number, teamIdx?: number }>("moveEntry2"),
};



const subReducer = reducerWithoutInitialState<MatchingState>()
  .case(actionCreators.getEntriesResponse, (state, payload) => {

    state.battleServers = payload.battleServers;
    state.entries = payload.entries;
    state.matchingArea = payload.matchingArea;


    //存在しなくなったBattleServerのroomを削除
    state.rooms = state.rooms.filter(room =>
      (undefined !==
        state.battleServers.find(bs => bs.matchId == room.matchId)));


    //存在しなくなったEntryをroomから削除
    state.rooms.forEach(room => {
      room.entries.forEach((team, idx) => {
        room.entries[idx] = team.filter(id => undefined !== state.entries.find(p => p.entryId == id));
      });
    });


    //freeEntriesの作成
    //  まずはroomにいるEntryを列挙
    let roomEntries: number[] = [];
    state.rooms.forEach(room => {
      room.entries.forEach(team => team.forEach(id => roomEntries.push(id)));
    });

    // で、roomにいないEntryをfreeEntriesに登録
    state.freeEntries = [];
    state.entries.forEach(e => {
      if (undefined === roomEntries.find(id => id == e.entryId)) {
        state.freeEntries.push(e.entryId);
      }
    });

    //新たなBattleServerをroomに追加
    state.battleServers.forEach((bs) => {
      if (undefined === state.rooms.find(room => room.matchId == bs.matchId)) {
        state.rooms.push(defaultRoom(bs.matchId));
      }
    });

    return state;
  })
  .case(actionCreators.semiAutoMatchmakeResponse, (state, payload) => {

    let roomIdx = state.rooms.findIndex(room => room.matchId == payload.matchId);
    if (roomIdx < 0 ) return state;
    
    payload.entries.forEach((ent, idx) => {
      var srcPos = SearchEntryPos(state, ent.entryId);
      if (srcPos.idx === undefined) return;

      RemoveEntry(state, srcPos);

      state.rooms[roomIdx].entries[ent.side].splice(0, 0, ent.entryId);
    });

    return state;
  })
  .case(actionCreators.moveEntry1, (state, payload) => {

    const { entryIdSrc, entryIdDst, pos } = payload;

    if (entryIdSrc == entryIdDst ) return state;

    var srcPos = SearchEntryPos( state, entryIdSrc );
    if( srcPos.idx === undefined ) return state;

    var dstPos = SearchEntryPos( state, entryIdDst );
    if( dstPos.idx === undefined ) return state;

    RemoveEntry( state, srcPos );

    if (dstPos.roomIdx !== undefined && dstPos.teamIdx !== undefined) {
      state.rooms[dstPos.roomIdx].entries[dstPos.teamIdx].splice(dstPos.idx + pos, 0, entryIdSrc);
    }
    else {
      state.freeEntries.splice(dstPos.idx + pos, 0, entryIdSrc);
    }

    return state;
  })
  .case(actionCreators.moveEntry2, (state, payload) => {

    const { entryIdSrc, roomIdx, teamIdx } = payload;

    var srcPos = SearchEntryPos( state, entryIdSrc );
    if( srcPos.idx === undefined ) return state;

    RemoveEntry( state, srcPos);

    if (roomIdx !== undefined && teamIdx !== undefined) {

      state.rooms[roomIdx].entries[teamIdx].splice(state.rooms[roomIdx].entries[teamIdx].length, 0, entryIdSrc);
    }
    else {
      state.freeEntries.splice(state.freeEntries.length, 0, entryIdSrc);
    }

    return state;
  })


export const reducer: Redux.Reducer<MatchingState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
