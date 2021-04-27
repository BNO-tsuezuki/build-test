import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';


interface Account {
  account: string;
  type: number;
  privilegeLevel: number;
  playerName: string;
}

export interface AccountsState{

  accounts: Account[];
  
}
const defaultState: AccountsState = {

  accounts:[],
};



//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {
	
  createRequest: ACFactory<{ account:string, password:string, nickname:string }>('createRequest'),
  createResponse: ACFactory<Account>('createResponse'),
  
  getListRequest: ACFactory<{}>('getListRequest'),
  getListResponse: ACFactory<{ accounts: Account[] }>('getListResponse'),

  changePrivilegeRequest: ACFactory<{ account:string, type:number, privilegeType:number, set:boolean }>('changePrivilegeRequest'),
  changePrivilegeResponse: ACFactory<{ account: string, privilegeLevel: number }>('changePrivilegeResponse'),

};

const subReducer = reducerWithoutInitialState<AccountsState>()
  .case(actionCreators.createResponse, (state, payload) => {
    return state;
  })
  .case(actionCreators.getListResponse, (state, payload) => {
    state.accounts = payload.accounts;
    return state;
  })
  .case(actionCreators.changePrivilegeResponse, (state, payload) => {

    let account = state.accounts.find(a => a.account == payload.account);
    if (account != undefined) {
      account.privilegeLevel = payload.privilegeLevel;
    }
    return state;
  });

export const reducer: Redux.Reducer<AccountsState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
