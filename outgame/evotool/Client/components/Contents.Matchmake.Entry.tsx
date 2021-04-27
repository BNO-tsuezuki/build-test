import * as React from 'react';
import { findDOMNode } from 'react-dom';
import * as DnD from 'react-dnd';

import { MatchingState, actionCreators, InsertPos } from 'store/Matching';

import { DragEntryInfo, DNDTYPE_ENTRY } from 'components/Contents.Matchmake.EntryDropTarget';

interface Property {
  entryId: number;
  connectDropTarget?: DnD.ConnectDropTarget;
  connectDragSource?: DnD.ConnectDragSource;
}
type Props = Property & MatchingState & typeof actionCreators;


class MatchmakeEntry extends React.Component<Props, {}> {

  render() {

    if( this.props.connectDragSource === undefined ) return;
    if (this.props.connectDropTarget === undefined) return;

    let entry = this.props.entries.find(e => e.entryId == this.props.entryId);
    if (undefined === entry) return null;

    let icon = "../images/casualmatch.png";
    if (entry.matchType != 0) {
      icon = "../images/rankmatch.png";
    }


    return  this.props.connectDragSource(
      this.props.connectDropTarget(
        <div style={{ border: "solid 1px" }} >
          <div style={{ margin: "4px", background: "rgba(86,239,132,0.75)" }}>
            <img style={{ height: "20px" }} src={icon} />
            ({entry.players.length}) [{entry.rating}]
            {
              entry.players.map((player, idx) => {
                return <span key={idx} > <br />{player.playerName} [{player.rating}]</span>; 
              })
            }
          </div>
        </div>
      )
    );
  }
}

const dragSpec = {
	beginDrag(props: Props, monitor: DnD.DragSourceMonitor): DragEntryInfo {
    return {
      entryId: props.entryId,
		};
	}
};

const dropSpec = {
  drop(props: Props, monitor: DnD.DropTargetMonitor, component: MatchmakeEntry) {

    const info = monitor.getItem() as DragEntryInfo;

    const rect = (findDOMNode(component) as Element).getBoundingClientRect();
		const dropPos = monitor.getClientOffset();
    const insertPos = (dropPos.y < (rect.top + (rect.height / 2))) ? InsertPos.Above : InsertPos.Below;

    props.moveEntry1({ entryIdSrc:info.entryId, entryIdDst:props.entryId, pos:insertPos});
	}
};

export default DnD.DropTarget(
  DNDTYPE_ENTRY,
  dropSpec,
  (connect) => ({
		connectDropTarget: connect.dropTarget(),
	})
)(
  DnD.DragSource(
  DNDTYPE_ENTRY,
	dragSpec,
	(connect, monitor) => ({
		connectDragSource: connect.dragSource(),
    isDragging: monitor.isDragging(),
	})
  )(MatchmakeEntry)
);
