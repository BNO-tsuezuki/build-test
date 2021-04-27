import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { ClientLauncherState, actionCreators } from 'store/ClientLauncher';

import CircleBtn from 'components/widgets/CircleBtn';

type Props = 
  ClientLauncherState
& typeof actionCreators;


class ClientLauncher extends React.Component<Props, {}> {

  private email: string = "";
  private password: string = "";


  render() {

    let style = {
      padding: "10px",
      fontSize: "1.3em",
      fontFamily: "Arial, sans-serif",
      color: "#aaa",
      border: "solid 1px #ccc",
      margin: "0 0 20px",
      width: "640px",
    }

    let hrefSetting = "../others/EvoLaunchSetting.zip?t=" + Date.now();

    return <>
      <hr />
        クライアントランチャ
      <hr />

      <div style={{ textAlign: "center", outline: "none", position: "absolute", left: "0%", right: "0%", top: "32px", bottom: "0%", background: "rgba(51, 130, 232, 0.6)" }}>

        <div style={{ margin: "16px", color: "white" }}>
          email
          <div>
            <input style={style} type="text" name="account" placeholder="email"
              onChange={(e) => { this.email = e.target.value; }} />
          </div>
        </div>

        <div style={{ margin: "16px", color: "white" }}>
          password
          <div>
            <input style={style} type="password" name="password" placeholder="password"
              onChange={(e) => { this.password = e.target.value; }} />
          </div>
        </div>

        <div>
          <CircleBtn text="Login" size={80} color={0xf9a9ae} fontSize={16}
            onClick={
              () => {
                this.props.inkyLoginRequest({
                  email: this.email,
                  password: this.password,
                });
              }
            }
            enable={true}
            href="#"
          />
        </div>

        {this.props.message}

        <div style={{ textAlign: "center", margin: "32px", marginTop: "80px", background: "rgba(0,255,0,0.4)" }}>
          
          <div>
            <CircleBtn text="Launch" size={100} color={0xa9aef9} fontSize={16}
              onClick={() => { }}
              enable={this.props.logined}
              href={this.props.launchHref}
            />
          </div>
        </div>

        <div style={{ textAlign: "right", outline: "none", position: "absolute", right:"0%", bottom: "0%" }}>
          最初に<a href={hrefSetting}>起動設定bat</a>をダウンロードし、管理者として実行してください。
        </div>

      </div>
    </>
  }
}

export default connect(
  (state: AppState) => state.clientLauncher,
  actionCreators
)(ClientLauncher);
