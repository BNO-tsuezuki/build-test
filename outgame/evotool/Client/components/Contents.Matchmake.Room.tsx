import * as React from 'react';

import { MatchingState, actionCreators, ForceMatchmakeRequest, SemiAutoMatchmakeRequest } from 'store/Matching';

import EntryDropTarget from 'components/Contents.Matchmake.EntryDropTarget';
import Entry from 'components/Contents.Matchmake.Entry';

import CircleBtn from 'components/widgets/CircleBtn';

interface Property {
  roomIndex:number;
}
type Props = Property & MatchingState & typeof actionCreators;


export default class MatchmakeRoom extends React.Component<Props, {}> {

  private command:string = "";

  render() {

    let roomIdx = this.props.roomIndex;
    let room = this.props.rooms[roomIdx];
    let battleServer = this.props.battleServers.find(bs => bs.matchId == room.matchId);
    if (battleServer === undefined) return;
    let battleServerSessionId = battleServer.sessionId;


    let teamPlayersCnt = [0, 0,];
    battleServer.players.forEach(p => {
      teamPlayersCnt[p.side]++;
    });
    room.entries.forEach((team, side) => {
      team.forEach(entryId => {
        let entry = this.props.entries.find(e => e.entryId == entryId);
        if (undefined !== entry) {
          teamPlayersCnt[side] += entry.players.length;
        }
      });
    });

 
    let style = {background:"rgba(255,255,255,0.5)", margin:"10px", padding:"4px"};
    if( 1 <= battleServer.state ) style = {background:"rgba(255,255,255,1)", margin:"10px", padding:"4px"};
    if( 3 <= battleServer.state ) style = {background:"rgba(0,0,0,0.75)", margin:"10px", padding:"4px"};


    var semiAutoMatchmakeTargetEntries = this.props.freeEntries;
    semiAutoMatchmakeTargetEntries = semiAutoMatchmakeTargetEntries.concat(room.entries[0]);
    semiAutoMatchmakeTargetEntries = semiAutoMatchmakeTargetEntries.concat(room.entries[1]);
    let semiAutoMatchmakeData: SemiAutoMatchmakeRequest = {
      matchingArea: this.props.matchingArea,
      matchId: room.matchId,
      entryIds: semiAutoMatchmakeTargetEntries,
    };

    let forceMatchmakeData: ForceMatchmakeRequest = {
      matchingArea: this.props.matchingArea,
      matchId: room.matchId,
      entries: [],
    };

    room.entries.forEach((team, teamIdx) => {
      team.forEach(id => {
        forceMatchmakeData.entries.push({ entryId: id, side: teamIdx });
      });
    });

    let icon = "../images/ManualMatchmake.png";
    if (battleServer.autoMatchmakeTarget) {
      icon = "../images/AutoMatchmake.png";
    }
      
    return <div style={style}>

      <div>
        <div style={{ float: "left" }}>
          <img style={{ marginLeft: "4px", height: "24px" }} src={icon} /> <a style={{ color: "#ff0000", fontWeight: "bold" }}>{battleServer.description}</a>({battleServer.label})<br />
          {battleServer.serverName}@{battleServer.ipAddr}:{battleServer.port}<br />
          [{battleServer.rule}]-[{battleServer.mapId}]<br />
          [{battleServer.region}]-[{battleServer.owner}]<br />
          <div>
            <input style={{}} type="text" name="command" placeholder="開発コマンド"
              onChange={(e) => { this.command = e.target.value; }} />
            <CircleBtn text="exec" size={32} color={0x222222} fontSize={10}
              onClick={() => { this.props.execCommandRequest({ battleServerSessionId: battleServerSessionId, command: this.command }); }}
              enable={true}
              href="#" />
          </div>
        </div>

        <div style={{ float: "right" }}>
          <CircleBtn text="SemiAuto!" size={80} color={0xe1e14d} fontSize={15}
            onClick={() => { this.props.semiAutoMatchmakeRequest(semiAutoMatchmakeData); }}
            enable={battleServer.state == 0}
            href="#" />
          <CircleBtn text="Matching!" size={90} color={0x56d648} fontSize={18}
            onClick={() => { this.props.forceMatchmakeRequest(forceMatchmakeData); }}
            enable={true}
            href="#" />
        </div>
      </div>

      <div style={{display:"table", tableLayout:"fixed", borderCollapse:"separate", borderSpacing:"6px", width:"100%"}}>

        <div style={{ display:"table-cell", background:"lightpink", margin:"10px", padding:"4px"}}>
          <EntryDropTarget roomIdx={this.props.roomIndex} teamIdx={0} {...this.props} />
          Earthnoid ({teamPlayersCnt[0]})
          {
            battleServer.players.map((player, idx) => {
              if (player.side != 0) return null;

              return <div style={{ border: "solid 1px", pointerEvents: "none" }}>
                <div style={{ padding: "4px" }}>
                  <span style={{ padding: "0px 16px", background: "rgba(132,86,239,0.75)" }}>
                    {player.groupNo}.{player.playerName} [{Math.floor(player.rating)}]
                  </span>
                </div>
              </div>;
            })
          }
          {
            room.entries[0].map((id, idx) => {
              return <Entry key={idx} entryId={id} {...this.props} />;
            })
          }
        </div>

        <div style={{ display:"table-cell", background:"lightskyblue", margin:"10px", padding:"4px"}}>
          <EntryDropTarget roomIdx={this.props.roomIndex} teamIdx={1} {...this.props} />
          Spacenoid ({teamPlayersCnt[1]})
          {
            battleServer.players.map((player, idx) => {
              if (player.side != 1) return null;

              return <div style={{ border: "solid 1px", pointerEvents: "none" }}>
                <div style={{ padding: "4px" }}>
                  <span style={{ padding: "0px 16px", background: "rgba(132,86,239,0.75)" }}>
                    {player.groupNo}.{player.playerName} [{Math.floor(player.rating)}]
                  </span>
                </div>
              </div>;
            })
          }
          {
            room.entries[1].map((id, idx) => {
              return <Entry key={idx} entryId={id} {...this.props} />;
            })
          }
        </div>

      </div>

    </div>;

  }
}
