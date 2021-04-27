import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';
import { stat } from 'fs';


interface Item {
  itemId:string;
  itemType:string;
  displayName:string;
  description:string;
  owned:boolean;
  isDefault:boolean;
}

export interface Player {
  playerId:number;
  playerName:string;
  initialLevel:number;
}

export interface GrantItemState{
  player: Player|null;
  items: {[key:string]:Item[]};
}

const defaultState: GrantItemState = {
  player: null,
  items: {},
};


//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {
  searchPlayerRequest: ACFactory<{ searchKey: string }>('searchPlayerRequest'),
  searchPlayerResponse: ACFactory<{ player: Player | null, items: Item[] }>('searchPlayerResponse'),
  getAllRequest: ACFactory<{ playerId: number }>('getAllRequest'),
  getAllResponse: ACFactory<{ playerId: number, itemIds: string[] }>('getAllResponse'),
  getResetAllRequest: ACFactory<{ playerId: number }>('getResetAllRequest'),
  getResetAllResponse: ACFactory<{ playerId: number, itemIds: string[] }>('getResetAllResponse'),
  switchOwnedRequest: ACFactory<{ playerId: number, itemId: string }>('switchOwnedRequest'),
  switchOwnedResponse: ACFactory<{ playerId: number, itemId: string, owned: boolean }>('switchOwnedResponse'),
  resetInitialLevelFlgRequest: ACFactory<{ playerId:number, flgIndex: number }>('resetInitialLevelFlgRequest'),
  resetInitialLevelFlgResponse: ACFactory<{ playerId:number, initialLevel:number }>('resetInitialLevelFlgResponse'),
};


const subReducer = reducerWithoutInitialState<GrantItemState>()
  .case(actionCreators.searchPlayerResponse, (state, payload) => {

    state.player = payload.player;
    state.items = {};

    payload.items.forEach(i => {

      if (state.items[i.itemType] == null) {
        state.items[i.itemType] = [];
      }
      state.items[i.itemType].push(i);
    });

    return state;
  })
  .case(actionCreators.getAllResponse, (state, payload) => {

    if (state.player != null && state.player.playerId == payload.playerId) {
      payload.itemIds.forEach(itemId => {
        for (let type in state.items) {
          var item = state.items[type].find(i => i.itemId == itemId);
          if (item != null) {
            item.owned = true;
            break;
          }
        }
      });
    }

    return state;
    })
  .case(actionCreators.getResetAllResponse, (state, payload) => {

    if (state.player != null && state.player.playerId == payload.playerId) {
      payload.itemIds.forEach(itemId => {
        for (let type in state.items) {
          var item = state.items[type].find(i => i.itemId == itemId);
          if (item != null) {
            item.owned = false;
            break;
          }
        }
      });
    }

    return state;
    })
  .case(actionCreators.switchOwnedResponse, (state, payload) => {

    if (state.player != null && state.player.playerId == payload.playerId) {
      for (let type in state.items) {
        let item = state.items[type].find(item => item.itemId == payload.itemId);
        if (item != null) {
          item.owned = payload.owned;
          break;
        }
      }
    }
    
    return state;
  })
  .case(actionCreators.resetInitialLevelFlgResponse, (state, payload) => {
    
    if (state.player != null && state.player.playerId == payload.playerId) {
      state.player.initialLevel = payload.initialLevel;
    }

    return state;
  })


export const reducer: Redux.Reducer<GrantItemState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
