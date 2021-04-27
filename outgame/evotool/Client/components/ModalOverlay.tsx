import * as React from 'react';
import * as Overlays from 'react-overlays';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { ModalOverlayState, actionCreators, TaskType, MsgTask } from 'store/ModalOverlay';


type Props = 
  ModalOverlayState
& typeof actionCreators;


class ModalOverlay extends React.Component<Props, {}>{
  
  render() {

    if( this.props.taskStack.length == 0 ) return null;

    let body = <div></div>;
    let task = this.props.taskStack[0];

    if (task.type == TaskType.Msg) {

      let msgTask = task as MsgTask;

      body = <div style={{textAlign:"center"}}>
        <div style={{margin:"20px", color:"white" }}>{msgTask.msg}</div>
        <button type='button' style={{padding:"10px 40px"}} 
          onClick={ (e) => { this.props.killTask({taskNo:msgTask.taskNo}); } } >
          OK
        </button>
      </div>;
    }

    if (task.type == TaskType.WaitingComplete) {
      body = <div style={{textAlign:"center"}}>
        <div style={{margin:"20px", color:"white" }}>Waiting...</div>
      </div>;
    }

    return <Overlays.Modal show={true} className='modal' >
      <div style={{display:"flex", justifyContent:"center", alignItems:"center",
                      position:"absolute", left:"0", top:"0", right:"0", bottom:"0"}}> 
        {body}
			</div>
		</Overlays.Modal>;
	}
}

export default connect(
	(state: AppState) => state.modalOverlay,
  actionCreators
)(ModalOverlay);
