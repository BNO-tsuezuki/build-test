import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { AuthState, actionCreators } from 'store/Auth';

import Login from 'components/Header.Login';
import Logout from 'components/Header.Logout';

type Props = 
  AuthState
& typeof actionCreators;


class Header extends React.Component<Props, {}> {

  render() {

    const account = this.props.account;
    const caption = (this.props.login)?"ログアウト":"ログイン";
    const ref = (this.props.login)?"Logout":"Login";
    const component = (this.props.login)?Logout:Login;
    const onClick = (this.props.login)
                      ?() => {const obj = this.refs.Logout as Logout; obj.show(); }
                      :() => {const obj = this.refs.Login as Login; obj.show(); };

    return <div>
    
      <ul style={{listStyleType: "none", paddingLeft: "0", display: "flex", justifyContent: "flex-end"}}>
        <pre>{account}  </pre>
        <li style={{width: "5%", textAlign: "center" }}>
          <button style={{ width: "100%", color: "#fff", fontSize: "12px", fontWeight: "bold", padding: "10px 0",
                            textDecoration: "none", display: "block", backgroundColor: "rgba(64,64,64,0.3)" }}
                  type="button"
                  onClick={(e) => { onClick(); } }>
            {caption}
          </button>
        </li>
      </ul>

      {React.createElement(component, Object.assign( {}, this.props, {ref:ref} ))}

    </div>;
  }
}

export default connect(
	(state: AppState) => state.auth,
  actionCreators
)(Header);
