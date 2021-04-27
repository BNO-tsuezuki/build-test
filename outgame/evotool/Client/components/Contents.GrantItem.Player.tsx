import * as React from 'react';

import { GrantItemState, Player, actionCreators } from 'store/GrantItem';

import CircleBtn from 'components/widgets/CircleBtn';

interface Property {

}
type Props = Property & GrantItemState & typeof actionCreators;

export default class GrantItemPlayer extends React.Component<Props, {}> {

  render() {

    if (this.props.player == null) {
      return <div /> 
    }

    let initialLevelFlg = ["Name", "Tutorial"];

    let initialItemFlg = ["Get", "Reset"];

    let player = this.props.player;

    return <>
      <div style={{background:"rgba(255,0,128,0.6)", fontSize:"18px"}}>
        <div>
          [name]:{player.playerName}
        </div>
        <div style={{ display:"flex", flexDirection:"row", flexWrap:"wrap"}}>
          {
            initialLevelFlg.map((n,idx) => {
              return <div key={idx}
                  style={{ background:(player.initialLevel&(1<<idx))?"rgba(255,255,0,0.8)":"rgba(128,128,0,0.7)",
                                  margin: "4px", width: "8%", textAlign: "center", fontSize:"12px" }}
                   onDoubleClick={
                     (e) => {
                       if (player.initialLevel & (1 << idx))
                         this.props.resetInitialLevelFlgRequest({playerId:player.playerId, flgIndex:idx});
                     }
                   }
                >
                {n}リセット
              </div>
            })
          }
        </div>
        <div style={{ display: "flex", flexDirection: "row", flexWrap: "wrap" }}>
          {
            initialItemFlg.map((n, idx) => {
              return <div key={idx}
                  style={{
                    background: "rgba(64,255,0,0.8)",
                    margin: "4px", width: "8%", textAlign: "center", fontSize: "12px", borderStyle: "GROOVE"
                  }}
                  onDoubleClick={
                    (e) => {
                      if (idx == 0) { this.props.getAllRequest({ playerId: player.playerId }); }
                      else { this.props.getResetAllRequest({ playerId: player.playerId }); }
                    }
                  }
                >
                  All {n} !
              </div>
            })
          }
        </div>
      </div>
      <div style={{ overflowY:"scroll", position:"absolute", left:"0%", right:"0%", top:"200px", bottom:"0%", background:"rgba(111, 230, 232, 0.5)" }}>
        {
          Object.keys(this.props.items).map((type, idx)=> {
              return <div key={idx} style={{ margin:"12px"}}>
                <div style={{ background:"rgba(0,0,0,0.7)", color:"white" }}> 
                  {type}
                </div>
                <div style={{ display:"flex", flexDirection:"row", flexWrap:"wrap"}}>
                  {
                    this.props.items[type].map((item, idx) => {
                      return <div key={idx}
                        style={{ background:item.owned?"rgba(255,255,0,0.8)":"rgba(128,128,0,0.7)",
                                  border:item.isDefault?"solid 2px #404040":"none",
                                  margin: "4px", width: "16%", textAlign: "center" }}
                        onDoubleClick={
                          (e) => { if( !item.isDefault || !item.owned )
                            this.props.switchOwnedRequest({playerId:player.playerId, itemId:item.itemId});}
                        } 
                      >
                         {item.isDefault?'*':''}{item.displayName}
                      </div>
                    })
                  }
                </div>
              </div>
          })
        }
      </div>

    </>
  }

}
