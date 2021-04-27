import * as React from 'react';
import * as Overlays from 'react-overlays';

import CircleBtn from 'components/widgets/CircleBtn';

import * as AuthStore from 'store/Auth';

interface Property {
}
type Props = Property & typeof AuthStore.actionCreators;

interface State {
	show: boolean;
}

export default class Logout extends React.Component<Props, State>{
  
  componentWillMount() {
		this.setState({show:false});
	}

  show() {
		this.setState({ show: true });
	}

  render() {
		
    const { show } = this.state;

    let style = {
      padding:"10px",
      fontSize:"1.3em",
      fontFamily:"Arial, sans-serif",
      color:"#aaa",
      border:"solid 1px #ccc",
      margin:"0 0 20px",
      width:"640px",
    }

		return <Overlays.Modal show={show} className='modal' >
      <div style={{display:"flex", justifyContent:"center", alignItems:"center",
                      position:"absolute", left:"0", top:"0", right:"0", bottom:"0"}}>
        <div style={{textAlign:"center", outline:"none" }}>

          <div style={{margin:"40px", color:"white"}}>
            ログアウトしますか？
          </div>

          <div>
            <CircleBtn text="ログアウト" size={120} color={0xf9a9ae} fontSize={20}
              onClick={
                () => {
                  this.setState({show:false});
                  this.props.logoutRequest({});
                }
              }
              enable={true}
              href="#"
            />
            <CircleBtn text="キャンセル" size={120} color={0xcfcf8e} fontSize={20}
              onClick={
                ()=>{
                  this.setState({show:false});
                }
              }
              enable={true}
              href="#"
            />
          </div>
        
        </div>
      </div>
    </Overlays.Modal>;
	}
}
