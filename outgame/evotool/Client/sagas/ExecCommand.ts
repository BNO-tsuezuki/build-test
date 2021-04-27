import { take, call, put } from 'redux-saga/effects';

import HttpFetch from 'sagas/util/HttpFetch';

import * as MatchingStore from 'store/Matching';

export default function* ExecCommand() {

  while (true) {
		
    const action = yield take(MatchingStore.actionCreators.execCommandRequest);

    const res = yield call( HttpFetch, "/api/Matchmake/ExecCommand", action.payload);

    if (res.error != null && res.error.msg != null) {
      console.log( res.error.msg );
    }

  }

}
