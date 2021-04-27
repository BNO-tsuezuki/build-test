import * as React from 'react';
import * as DnD from 'react-dnd';

import { actionCreators } from 'store/Matching';


interface Property {
  roomIdx?:number;
  teamIdx?:number;
  connectDropTarget?: DnD.ConnectDropTarget;
}
type Props = Property & typeof actionCreators;

class EntryDropTarget extends React.Component<Props, {}> {

  render() {

    if( this.props.connectDropTarget === undefined ) return;

    return this.props.connectDropTarget(
      <div style={{ position:"absolute", left:"0%", top:"0%", right:"0%", bottom:"0%", border:"solid 1px" }}>

      </div>
    );
  }
}

export interface DragEntryInfo {
	entryId: number;
}
export const DNDTYPE_ENTRY: string = 'DNDTYPE_ENTRY';

const dropSpec = {
  drop(props: Props, monitor: DnD.DropTargetMonitor, component: EntryDropTarget) {

    const info = monitor.getItem() as DragEntryInfo;

    props.moveEntry2({ entryIdSrc:info.entryId, roomIdx:props.roomIdx, teamIdx:props.teamIdx});
	}
};

export default DnD.DropTarget(
  DNDTYPE_ENTRY,
	dropSpec,
	(connect) => ({
		connectDropTarget: connect.dropTarget(),
	})
)(EntryDropTarget);
