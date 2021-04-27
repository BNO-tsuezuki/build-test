import { combineReducers, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';

import rootSaga from 'sagas/Root';

import * as AuthStore from 'store/Auth';
import * as ModalOverlayStore from 'store/ModalOverlay';
import * as AccountsStore from 'store/Accounts';
import * as MatchingStore from 'store/Matching';
import * as GrantItemStore from 'store/GrantItem';
import * as ClientLauncherStore from 'store/ClientLauncher';
import * as EnvInfoStore from 'store/EnvInfo';


export interface AppState {
	auth: AuthStore.AuthState;
  modalOverlay: ModalOverlayStore.ModalOverlayState;
  accounts: AccountsStore.AccountsState;
  matching: MatchingStore.MatchingState;
  grantItem: GrantItemStore.GrantItemState;
  clientLauncher: ClientLauncherStore.ClientLauncherState;
  envInfo: EnvInfoStore.EnvInfoState;
}

console.log('----<<CreateStore>>----');
export const store = (() => {

	const sagaMiddleware = createSagaMiddleware();

	const store = createStore(
		combineReducers<AppState>({
			auth: AuthStore.reducer,
      modalOverlay: ModalOverlayStore.reducer,
      accounts: AccountsStore.reducer,
      matching: MatchingStore.reducer,
      grantItem: GrantItemStore.reducer,
      clientLauncher: ClientLauncherStore.reducer,
      envInfo:EnvInfoStore.reducer,
		}),
		applyMiddleware(
			sagaMiddleware
		)
	);

	sagaMiddleware.run(rootSaga);

	return store;

})();
