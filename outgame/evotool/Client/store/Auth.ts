import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';


// state
export interface AuthState{
  login: boolean;
  account: string;
}
const defaultState: AuthState = {
  login:false,
  account:'ログインしていません',
};


//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {
	loginRequest: ACFactory<{ account:string, password:string }>('loginRequest'),
	loginResponse: ACFactory<{ account:string }>('loginResponse'),
  logoutRequest: ACFactory<{}>('logoutRequest'),
  logoutResponse: ACFactory<{}>('logoutResponse'),
};

const subReducer = reducerWithoutInitialState<AuthState>()
  .case(actionCreators.loginResponse, (state, payload) => {
    state.login = true;
    state.account = payload.account;
	  return state;
  })
  .case(actionCreators.logoutResponse, (state, payload) => {
    state.login = false;
    state.account = defaultState.account;
    return state;
  });

export const reducer: Redux.Reducer<AuthState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
