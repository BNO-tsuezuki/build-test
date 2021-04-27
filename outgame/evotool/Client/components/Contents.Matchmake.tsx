import * as React from 'react';
import { connect } from 'react-redux';

import { AppState } from 'store/_App';
import { MatchingState, actionCreators } from 'store/Matching';

import EntryDropTarget from 'components/Contents.Matchmake.EntryDropTarget';
import Room from 'components/Contents.Matchmake.Room';
import Entry from 'components/Contents.Matchmake.Entry';
import CircleBtn from 'components/widgets/CircleBtn';

type Props = 
  MatchingState
& typeof actionCreators;


class Matchmake extends React.Component<Props, {}> {

  render() {

    let freePlayersCnt = 0;
    this.props.freeEntries.forEach(fe => {
      let entry = this.props.entries.find(e => e.entryId == fe);
      if (undefined !== entry) {
        freePlayersCnt += entry.players.length;
      }
    });

    return <>

      <hr />
      マッチメイク
        <CircleBtn text="Asia" size={64} color={0xcfcf8e} fontSize={14}
          onClick={
            () => {
              this.props.getEntriesRequest({matchingArea:0});
            }
          }
          enable={true}
          href="#"
        />
        <CircleBtn text="N. America" size={64} color={0xcfcf8e} fontSize={10}
          onClick={
            () => {
              this.props.getEntriesRequest({matchingArea:1});
            }
          }
          enable={true}
          href="#"
        />
      <hr />

      <div>
      </div>

      <div style={{ overflowY: "scroll", position: "absolute", left: "0%", right: "40%", top: "90px", bottom: "0%", background: "rgba(111, 230, 232, 0.5)" }}>
        {
          this.props.rooms.map((room, idx) => {
            return <Room key={idx} roomIndex={idx} {...this.props} />;
          })
        }
      </div>

      <div style={{ overflowY: "scroll", position: "absolute", left: "60%", right: "0%", top: "90px", bottom: "0%", background: "rgba(232, 230, 111,0.5)" }}>

        <EntryDropTarget {...this.props} />

        Waiting Players ... ({freePlayersCnt})
        {
          this.props.freeEntries.map((entryId, idx) => {
            return <Entry key={idx} entryId={entryId} {...this.props} />;
          })
        }

      </div>

    </>;

    // TODO あとでやる
  }
}

export default connect(
	(state: AppState) => state.matching,
  actionCreators
)(Matchmake);
