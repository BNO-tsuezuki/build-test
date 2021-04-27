import { take, call, put } from 'redux-saga/effects';

import HttpFetch from 'sagas/util/HttpFetch';

import * as MatchingStore from 'store/Matching';
import * as ModalOverlayStore from 'store/ModalOverlay';

export default function* ForceMatchmake() {

  while (true) {
		
    const action = yield take(MatchingStore.actionCreators.forceMatchmakeRequest);

    let taskNo = { taskNo:-1 };
    yield put(ModalOverlayStore.actionCreators.pushWaitingCompleteTask(taskNo));

    const res1 = yield call( HttpFetch, "api/Matchmake/ForceMatchmake", action.payload);
    if (res1.error != null && res1.error.msg != null) {
      yield put(ModalOverlayStore.actionCreators.killTask(taskNo));
      yield put(ModalOverlayStore.actionCreators.pushMsgTask({msg:res1.error.msg}));
      continue;
    }
    
    const res2 = yield call(HttpFetch, "api/Matchmake/GetEntries", {matchingArea:res1.matchingArea});
    if (res2.error != null && res2.error.msg != null) {
      yield put(ModalOverlayStore.actionCreators.killTask(taskNo));
      yield put(ModalOverlayStore.actionCreators.pushMsgTask({msg:res2.error.msg}));
      continue;
    }
    yield put(MatchingStore.actionCreators.getEntriesResponse(res2));

    yield put(ModalOverlayStore.actionCreators.killTask(taskNo))
  }
}
