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


export default class Login extends React.Component<Props, State>{

  private account:string = "";
  private password:string = "";

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
            emailアドレス
            <div>
              <input style={style} type="text" name="account" placeholder="emailアドレス "
                onChange={(e) => { this.account = e.target.value;}} />
            </div>
          </div>

          <div style={{margin:"40px", color:"white"}}>
            パスワード
            <div>
              <input style={style} type="password" name="password" placeholder="パスワード"
                onChange={(e) => { this.password = e.target.value;}} />
            </div>
          </div>

          <div>
            <CircleBtn text="ログイン" size={120} color={0xf9a9ae} fontSize={20}
              onClick={
                () => {
                  this.setState({show:false});
                  
                  this.props.loginRequest({
                    account:this.account,
                    password:this.password,
                  });
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
