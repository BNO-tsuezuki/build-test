import { take, call, put } from 'redux-saga/effects';
import { ActionCreator } from 'typescript-fsa';

import HttpFetch from 'sagas/util/HttpFetch';


export default function* HttpRequestModeless(uri: string, reqActionCreator: ActionCreator<any>, resActionCreator: ActionCreator<any> ) {

	while (true) {
		
    const action = yield take(reqActionCreator);

    const res = yield call( HttpFetch, uri, action.payload);

    if (res.error != null && res.error.msg != null) {
    }
    else {
      yield put(resActionCreator(res));
    }

  }
}
