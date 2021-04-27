import { take, call, put } from 'redux-saga/effects';

import HttpFetch from 'sagas/util/HttpFetch';

import * as AccountsStore from 'store/Accounts';
import * as ModalOverlayStore from 'store/ModalOverlay';

export default function* CreateAccount() {

  while (true) {
		
    const action = yield take(AccountsStore.actionCreators.createRequest);

    let taskNo = { taskNo:-1 };
    yield put(ModalOverlayStore.actionCreators.pushWaitingCompleteTask(taskNo));
    const res = yield call( HttpFetch, "/api/LocalAccount/Create", action.payload);
    yield put(ModalOverlayStore.actionCreators.killTask(taskNo));


    if (res.error != null && res.error.msg != null) {
      yield put(ModalOverlayStore.actionCreators.pushMsgTask({ msg: res.error.msg }));
    }
    else {
      yield put(ModalOverlayStore.actionCreators.pushMsgTask({msg: "アカウントが作成されました。"+res.account}));
    }
  }
}
