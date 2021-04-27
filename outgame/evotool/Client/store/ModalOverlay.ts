import * as Redux from 'redux';
import actionCreatorFactory from 'typescript-fsa';
import { reducerWithoutInitialState } from 'typescript-fsa-reducers';
import { start } from 'repl';


export enum TaskType {
  Msg,
  WaitingComplete,
}

class Task {
  
  type:TaskType;
  taskNo:number;
  
  constructor( type:TaskType, taskNo:number) {

    this.type = type;
    this.taskNo = taskNo;
  }
}

export class MsgTask extends Task {

  msg:string;

  constructor( taskNo:number, msg:string) {
    super( TaskType.Msg, taskNo );
    this.msg = msg;
  }
}

export class WaitingCompleteTask extends Task {
  constructor( taskNo:number ) {
    super( TaskType.WaitingComplete, taskNo );
  }
}

// state
export interface ModalOverlayState{

  taskStack:Task[];
  taskNo:number;
}
const defaultState: ModalOverlayState = {

  taskStack:[],
  taskNo:0,
};


//--------------------------------------------------------------
// action & action creator
//--------------------------------------------------------------
const ACFactory = actionCreatorFactory();
export const actionCreators = {

	pushMsgTask: ACFactory<{ msg:string }>('pushMsgTask'),
  pushWaitingCompleteTask: ACFactory <{taskNo:number}>('pushWaitingCompleteTask'),

  killTask: ACFactory<{ taskNo:number }>('killTask'),
};

const subReducer = reducerWithoutInitialState<ModalOverlayState>()
  .case(actionCreators.pushMsgTask, (state, payload) => {

    state.taskStack.push( new MsgTask( state.taskNo++, payload.msg ) );

    return state;
  })
  .case(actionCreators.pushWaitingCompleteTask, (state, payload) => {

    payload.taskNo = state.taskNo;
    state.taskStack.push( new WaitingCompleteTask( state.taskNo++ ) );

    return state;
  })
  .case(actionCreators.killTask, (state, payload) => {

    let idx = state.taskStack.findIndex((task)=>task.taskNo==payload.taskNo);
    if (0 <= idx) {
      state.taskStack.splice( idx, 1);
    }
    
    return state;
  });

  
export const reducer: Redux.Reducer<ModalOverlayState> = (state = defaultState, action) => {

	return subReducer( JSON.parse(JSON.stringify(state)), action);
}
