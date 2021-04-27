import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';


// state
export interface EnvInfoState{
  mastarDataVersion: string;
}
const defaultState: EnvInfoState = {
  mastarDataVersion:'XXX-XXX-XXX'
};


//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {
  getMasterDataVersionRequest: ACFactory<{}>('getMasterDataVersionRequest'),
  getMasterDataVersionResponse: ACFactory<{ masterDataVersion: string }>('getMasterDataVersionResponse'),
};

const subReducer = reducerWithoutInitialState<EnvInfoState>()
  .case(actionCreators.getMasterDataVersionResponse, (state, payload) => {
    state.mastarDataVersion = payload.masterDataVersion;
    return state;
  });


export const reducer: Redux.Reducer<EnvInfoState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
