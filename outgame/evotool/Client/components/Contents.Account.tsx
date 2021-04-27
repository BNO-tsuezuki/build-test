import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { AccountsState, actionCreators } from 'store/Accounts';


import AccountCreate from 'components/Contents.Account.Create';
import ListElement from 'components/Contents.Account.ListElement';

import CircleBtn from 'components/widgets/CircleBtn';


type Props = 
  AccountsState
& typeof actionCreators;

class Account extends React.Component<Props, {}> {

    render() {

        return <>
          <hr />
            アカウント管理
            <CircleBtn text="Update" size={64} color={0xcfcf8e} fontSize={14}
              onClick={
                () => {
                  this.props.getListRequest({});
                }
              }
            enable={true}
            href="#"
            />
            <CircleBtn text="新規作成" size={80} color={0xf9a9ae} fontSize={18}
              onClick={() => { const obj = this.refs.AccountCreate as AccountCreate; obj.show(); }}
            enable={true}
            href="#"
            />
          <hr />

          <AccountCreate ref="AccountCreate" {...this.props} />

          <ListElement index={-1} {...this.props} />
          <div style={{ overflowY: "scroll", position: "absolute", left: "0%", right: "0%", top: "236px", bottom: "0%", background: "rgba(111, 230, 232, 0.5)" }}>
            {
              this.props.accounts.map((account, idx) => {
                return <ListElement key={idx} index={idx} {...this.props} />
              })
            }
          </div>

        </>;
    }
}

export default connect(
	(state: AppState) => state.accounts,
  actionCreators
)(Account);
