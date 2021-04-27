import { take, call, put } from 'redux-saga/effects';
import { ActionCreator } from 'typescript-fsa';

import HttpFetch from 'sagas/util/HttpFetch';

import * as ModalOverlayStore from 'store/ModalOverlay';

export default function* HttpRequest(
  uri: string,
  reqActionCreator: ActionCreator<any>,
  resActionCreator: ActionCreator<any>,
  orgHeaders: Object = {} )
{

	while (true) {
		
    const action = yield take(reqActionCreator);

    let taskNo = { taskNo:-1 };
    yield put(ModalOverlayStore.actionCreators.pushWaitingCompleteTask(taskNo));
    const res = yield call( HttpFetch, uri, action.payload, orgHeaders);
    yield put(ModalOverlayStore.actionCreators.killTask(taskNo))

    
    if (res.error != null && res.error.msg != null) {
      yield put(ModalOverlayStore.actionCreators.pushMsgTask({msg:res.error.msg}));
    }
    else {
      yield put(resActionCreator(res));
    }

  }
}
