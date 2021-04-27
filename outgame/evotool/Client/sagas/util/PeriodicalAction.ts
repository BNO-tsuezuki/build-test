import { take, call, put } from 'redux-saga/effects';
import { ActionCreator } from 'typescript-fsa';
import {delay} from 'redux-saga';


export default function* PeriodicalAction( action: ActionCreator<any>, payload:Object, interval:number ) {

	while (true) {

    yield put( action(payload) );

    yield call(delay, interval );
  }
}
