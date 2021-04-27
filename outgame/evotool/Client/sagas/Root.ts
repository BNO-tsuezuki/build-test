import { fork } from 'redux-saga/effects';

import HttpRequest from 'sagas/util/HttpRequest';
import HttpRequestModeless from 'sagas/util/HttpRequestModeless';
import PeriodicalAction from 'sagas/util/PeriodicalAction';

import ForceMatchmake from 'sagas/ForceMatchmake';
import CreateAccount from 'sagas/CreateAccount';
import ExecCommand from 'sagas/ExecCommand';
import InkyLogin from 'sagas/InkyLogin';

import * as AuthStore from 'store/Auth';
import * as AccountsStore from 'store/Accounts';
import * as MatchingStore from 'store/Matching';
import * as GrantItemStore from 'store/GrantItem';
import * as ClientLauncherStore from 'store/ClientLauncher';
import * as EnvInfoStore from 'store/EnvInfo';



export default function* root() {


  //yield fork(PeriodicalAction, XXXXXX, {}, 1000 );


  yield fork(HttpRequest,
    "/api/Auth/Login",
    AuthStore.actionCreators.loginRequest,
    AuthStore.actionCreators.loginResponse);

  yield fork(HttpRequest,
    "/api/Auth/Logout",
    AuthStore.actionCreators.logoutRequest,
    AuthStore.actionCreators.logoutResponse);




  yield fork(HttpRequest,
    "/api/Account/GetList",
    AccountsStore.actionCreators.getListRequest,
    AccountsStore.actionCreators.getListResponse);

  yield fork(HttpRequest,
    "/api/Account/ChangePrivilege",
    AccountsStore.actionCreators.changePrivilegeRequest,
    AccountsStore.actionCreators.changePrivilegeResponse);

  yield fork(HttpRequest,
    "api/Matchmake/GetEntries",
    MatchingStore.actionCreators.getEntriesRequest,
    MatchingStore.actionCreators.getEntriesResponse);

  yield fork(HttpRequest,
    "api/Matchmake/SemiAutoMatchmake",
    MatchingStore.actionCreators.semiAutoMatchmakeRequest,
    MatchingStore.actionCreators.semiAutoMatchmakeResponse);


  yield fork(HttpRequest,
    "/api/Grant/SearchPlayer",
    GrantItemStore.actionCreators.searchPlayerRequest,
    GrantItemStore.actionCreators.searchPlayerResponse);

  yield fork(HttpRequest,
    "/api/Grant/ResetInitialLevelFlg",
    GrantItemStore.actionCreators.resetInitialLevelFlgRequest,
    GrantItemStore.actionCreators.resetInitialLevelFlgResponse);

  yield fork(HttpRequest,
    "/api/Grant/SwitchOwnedItem",
    GrantItemStore.actionCreators.switchOwnedRequest,
    GrantItemStore.actionCreators.switchOwnedResponse);

  yield fork(HttpRequest,
    "/api/Grant/GetAllItems",
    GrantItemStore.actionCreators.getAllRequest,
    GrantItemStore.actionCreators.getAllResponse);

  yield fork(HttpRequest,
    "/api/Grant/ResetAllItems",
    GrantItemStore.actionCreators.getResetAllRequest,
    GrantItemStore.actionCreators.getResetAllResponse);


  yield fork(CreateAccount);

  yield fork(ForceMatchmake);

  yield fork(ExecCommand);

  yield fork(InkyLogin);
  
  yield fork(HttpRequestModeless,
    "/api/MasterData/GetVersion",
    EnvInfoStore.actionCreators.getMasterDataVersionRequest,
    EnvInfoStore.actionCreators.getMasterDataVersionResponse
  );
  yield fork(PeriodicalAction, EnvInfoStore.actionCreators.getMasterDataVersionRequest, {}, 10000);

}
