import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { EnvInfoState, actionCreators } from 'store/EnvInfo';


type Props =
  EnvInfoState
& typeof actionCreators;


class Footer extends React.Component<Props, {}> {
  render() {

    return <div style={{ marginLeft: "16px", color: "white" }}>
      MasterData ver. {this.props.mastarDataVersion}
    </div>;
  }
}

export default connect(
  (state: AppState) => state.envInfo,
  actionCreators
)(Footer);
