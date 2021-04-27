import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { GrantItemState, actionCreators } from 'store/GrantItem';

import GrantItemPlayer from 'components/Contents.GrantItem.Player';
import CircleBtn from 'components/widgets/CircleBtn';

type Props = 
  GrantItemState
& typeof actionCreators;


class GrantItem extends React.Component<Props, {}> {

  private searchKey:string = "";

  render() {
    return <>
      <hr />
        アイテム与奪
      <hr />
      
      <div style={{margin:"8px", background:"rgba(0,0,255,0.4)"}}>
        
        <h4　style={{margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"20px", }}>
          だれに？
        </h4>

        <div style={{padding:"4px"}}>
          <input style={{width:"30%", height:"30px"}} type="text" name="playerName"
                placeholder="[プレイヤー名]  or  [アカウント]"
                onChange={(e) => { this.searchKey = e.target.value;}} 
          />
          <CircleBtn text="検索" size={24} color={0xdd4422} fontSize={8}
            onClick={() => { 
              this.props.searchPlayerRequest({searchKey:this.searchKey});} 
            }
            enable={true}
            href="#"
          />
        </div>
      
      </div>

      <GrantItemPlayer {...this.props} />

    </>
  }
}

export default connect(
	(state: AppState) => state.grantItem,
  actionCreators
)(GrantItem);
