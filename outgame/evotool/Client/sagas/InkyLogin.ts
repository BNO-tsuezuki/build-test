import { take, call, put } from 'redux-saga/effects';

import HttpFetch from 'sagas/util/HttpFetch';

import * as ClientLauncherStore from 'store/ClientLauncher';
import * as ModalOverlayStore from 'store/ModalOverlay';

export default function* InkyLogin() {

  let httpHeader = {
    "X-BNEA-Api-Id": "JJkE9NH9zDcwEw8brf5uBCPfQ2xAc66J",
    "X-BNEA-Api-Secret": "2XwEgnmFYfscQuGqTG2b45fBzxGQB3mb",
    "Authorization": "",
  };

  while (true) {
		
    const action = yield take(ClientLauncherStore.actionCreators.inkyLoginRequest);

    let taskNo = { taskNo:-1 };
    yield put(ModalOverlayStore.actionCreators.pushWaitingCompleteTask(taskNo));

    const res1 = yield call(HttpFetch, "https://stg-api.bnea.io/login",
      action.payload,
      httpHeader
    );
    if (res1.status != "success") {
      yield put(ClientLauncherStore.actionCreators.inkyLoginResponse(res1));
      yield put(ModalOverlayStore.actionCreators.killTask(taskNo));
      continue;
    }

    httpHeader.Authorization = res1.data.token_schema + " " + res1.data.access_token;

    const res2 = yield call(HttpFetch, "https://stg-api.bnea.io/login/GDEVO-PC/temp",
      {
        "product_type": "LAUNCHER",
        "redirect_uri": "",
      },
      httpHeader
    );

    yield put(ClientLauncherStore.actionCreators.inkyLoginResponse(res2));
    yield put(ModalOverlayStore.actionCreators.killTask(taskNo));
  }
}
