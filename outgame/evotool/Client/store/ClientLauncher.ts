import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';
import { stat } from 'fs';


export interface ClientLauncherState{
  message: string;
  launchHref: string;
  logined: boolean;
}

const defaultState: ClientLauncherState = {
  message: "Not logined.",
  launchHref: "",
  logined: false,
};

//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {

  inkyLoginRequest: ACFactory<{ email: string, password: string }>('inkyLoginRequest'),
  inkyLoginResponse: ACFactory<{ status: string, message: string, data: { access_token: string } }>('inkyLoginResponse'),
};


const subReducer = reducerWithoutInitialState<ClientLauncherState>()
  .case(actionCreators.inkyLoginResponse, (state, payload) => {

    if (payload.status == "success") {
      state.message = "Logined.";
      state.logined = true;
      state.launchHref = "devevostart:" + payload.data.access_token + "&type=1000";
    }
    else {
      state.message = "Not logined.";
      state.logined = false;
    }

    return state;
  })


export const reducer: Redux.Reducer<ClientLauncherState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
