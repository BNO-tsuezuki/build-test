import * as React from 'react';


import { AccountsState, actionCreators } from 'store/Accounts';

interface Property {
  index: number;
}
type Props = Property & AccountsState & typeof actionCreators;


export default class AccountsListElement extends React.Component<Props, {}> {

  render() {

    let captions: string[] = [
      "",
      "",
      "",
      "",
      "",
      "",
      "テストプレイヤA",
      "テストプレイヤB",
      "チートコマンド",
      "",
    ];

    if (this.props.index < 0) {
      return <div style={{ background: "rgba(140,140,255,0.8)", margin: "2px", padding: "4px" }}>
        <div style={{ display: "flex", flexDirection: "row", flexWrap: "wrap" }}>
          {captions.map((caption, idx) => {
            return <div key={idx}
              style={0 < caption.length
              ? {
                background: "rgba(32,32,255,0.8)", margin: "2px",
                width: "40px", height: "120px", borderStyle: "SOLID", borderWidth: "1px",
                fontSize: "14px", color: "white", writingMode: "vertical-rl", textAlign: "center"
              }
              : {
                background: "rgba(32,32,128,0.4)", margin: "2px",
                width: "40px", height: "120px", borderStyle: "SOLID", borderWidth: "1px",
              }
            }>
              {caption}
            </div>
          })}
        </div>
      </div>;
    }

    let account = this.props.accounts[this.props.index];

    let flgs:boolean[] = [];

    for (var i = 0; i < 10; i++) {
      flgs.push(account.privilegeLevel & (1 << i)?true:false);
    }
    
    return <div style={(this.props.index % 2 == 0)
      ? { background: "rgba(140,255,140,0.5)", margin: "2px", padding: "4px" }
      : { background: "rgba(200,255,200,0.5)", margin: "2px", padding: "4px" }}>

      <div style={{ display: "flex", flexDirection: "row", flexWrap: "wrap" }}>

        {flgs.map((flg, idx) => {
          return <div key={idx}
            style={flg
              ? { background: "rgba(64,255,0,0.8)", margin: "2px", width: "40px", height: "40px", borderStyle: "INSET" }
              : { background: "rgba(16, 32,0,0.4)", margin: "2px", width: "40px", height: "40px", borderStyle: "OUTSET" }
            }
              onClick={captions[idx].length != 0
              ? (e) => { this.props.changePrivilegeRequest({ account: account.account, type: account.type, privilegeType: idx, set: !flg }); }
              : (e) => {}
            }
          />
        })}
        <div style={{ verticalAlign: "middle", fontSize: "24px", marginLeft:"16px" }}>
          {account.playerName} ({account.account})
        </div>
      </div>
      
    </div>;

  }
}
